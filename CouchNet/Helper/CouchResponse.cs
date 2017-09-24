using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet.Helper
{
    public class CouchResponse
    {
        private readonly string responseString;
        private readonly HttpStatusCode statusCode;
        private readonly string statusDescription;
        private readonly string etag;

        public CouchResponse(HttpWebResponse response)
        {
            responseString = response.GetResponseString();
            statusCode = response.StatusCode;
            statusDescription = response.StatusDescription;
            etag = response.Headers["ETag"];
        }

        public string ResponseString { get { return responseString; } }
        public HttpStatusCode StatusCode { get { return statusCode; } }
        public string StatusDescription { get { return statusDescription; } }
        public string ETag { get { return etag; } }
        public CouchResponseObject GetJObject()
        {
            var resp = new CouchResponseObject(JObject.Parse(responseString));
            resp.StatusCode = (int)statusCode;
            return resp;
        }

        public Document GetCouchDocument()
        {
            var jobj = JObject.Parse(responseString);
            return new Document(jobj);
        }

    }
}
