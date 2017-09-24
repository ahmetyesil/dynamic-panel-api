using CouchNet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CouchNet
{
    public class CouchClient : CouchBase
    {
        private readonly string baseUri;

        public CouchClient() : this(CouchConfiguration.Default) { }
        public CouchClient(CouchConfiguration config) : base(config.Username, config.Password) { this.baseUri = "http://" + config.Host + ":" + config.Port + "/"; }

        public CouchResponseObject TriggerReplication(string source, string target, bool continuous)
        {
            var request = GetRequest(baseUri + "_replicate");
            var options = new ReplicationOptions(source, target, continuous);
            var response = request.Post().Data(options.ToString()).GetCouchResponse();
            return response.GetJObject();
        }

        public CouchResponseObject TriggerReplication(string source, string target)
        {
            return TriggerReplication(source, target, false);
        }

        public CouchDatabase GetDatabase(string databaseName)
        {
            return new CouchDatabase(baseUri, databaseName, username, password);
        }

        public bool HasDatabase(string databaseName)
        {
            var request = GetRequest(baseUri + databaseName).Timeout(-1);

            var response = request.GetCouchResponse();
            var pDocResult = new Document(response.ResponseString);

            if (pDocResult["error"] == null)
            {
                return (true);
            }
            if (pDocResult.Value<string>("error") == "not_found")
            {
                return (false);
            }
            throw new Exception(pDocResult.Value<string>("error"));
        }

        public bool HasUser(string userId)
        {
            return GetUser(userId) != null;
        }

        public Document GetUser(string userId)
        {
            var db = new CouchDatabase(baseUri, "_users", username, password);
            userId = "org.couchdb.user:" + HttpUtility.UrlEncode(userId);
            return db.GetDocument(userId);
        }

        public UniqueIdentifiers GetUUID(int count)
        {

            string request = baseUri + "_uuids";
            if (count > 50)
            {
                count = 50;
            }
            request = request + "?count=" + Convert.ToString(count);
            var x = GetRequest(request);
            string str = x.Get().Json().GetCouchResponse().GetJObject().ToString();
            UniqueIdentifiers y = Newtonsoft.Json.JsonConvert.DeserializeObject<UniqueIdentifiers>(str);
            return y;
        }
    }
}
