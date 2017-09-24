using CouchNet.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class ViewResult<T> : ViewResult
    {
        private CouchDictionary<T> dict = null;
        public ViewResult(CouchResponse response, HttpWebRequest request, bool includeDocs = false) : base(response, request, includeDocs) { }

        public CouchDictionary<T> Dictionary
        {
            get
            {
                if (dict != null) return dict;
                dict = new CouchDictionary<T>();
                foreach (var row in this.Rows)
                {
                    dict.Add(row.Value<JToken>("key").ToString(Formatting.None), ObjectSerializer.Deserialize<T>(row.Value<string>("value")));
                }
                return dict;
            }
        }

        private List<T> _Items { get; set; }

        public List<T> Items
        {
            get
            {
                if (_Items == null)
                {
                    var values = this.IncludeDocs ? this.RawDocs : this.RawValues;
                    _Items = values.Select(item => ObjectSerializer.Deserialize<T>(item)).ToList();
                }
                return _Items;
            }
        }
    }
    public class ViewResult : IEquatable<ListResult>
    {
        private readonly CouchResponse response;
        private readonly HttpWebRequest request;
        private JObject json = null;

        public JObject Json { get { return json ?? (json = JObject.Parse(response.ResponseString)); } }
        public ViewResult(CouchResponse response, HttpWebRequest request, bool includeDocs = false)
        {
            this.response = response;
            this.request = request;
            this.IncludeDocs = includeDocs;
        }

        public HttpWebRequest Request { get { return request; } }
        public HttpStatusCode StatusCode { get { return response.StatusCode; } }
        public string Etag { get { return response.ETag; } }
        public int TotalRows
        {
            get
            {
                if (Json["total_rows"] == null) throw new CouchException(request, response, Json["reason"].Value<string>());
                return Json["total_rows"].Value<int>();
            }
        }
        public int OffSet
        {
            get
            {
                if (Json["offset"] == null) throw new CouchException(request, response, Json["reason"].Value<string>());
                return Json["offset"].Value<int>();
            }
        }
        public IEnumerable<JToken> Rows
        {
            get
            {
                if (Json["rows"] == null) throw new CouchException(request, response, Json["reason"].Value<string>());
                return (JArray)Json["rows"];
            }
        }

        public IEnumerable<JToken> Docs
        {
            get
            {
                return (JArray)Json["doc"];
            }
        }

        public bool IncludeDocs { get; private set; }
        public JToken[] Keys
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry.Select(item => item["key"]).ToArray();
            }
        }

        public IEnumerable<string> RawRows
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry == null ? new List<string>() : arry.Select(item => item.ToString());
            }
        }

        public IEnumerable<string> RawValues
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry == null ? new List<string>() : arry.Select(item => item["value"].ToString());
            }
        }
        public IEnumerable<string> RawDocs
        {
            get
            {
                var arry = (JArray)Json["rows"];
                return arry == null ? new List<string>() : arry.Select(item => item["doc"].ToString());
            }
        }
        public string RawString
        {
            get { return response.ResponseString; }
        }

        public bool Equals(ListResult other)
        {
            if (string.IsNullOrEmpty(Etag) || string.IsNullOrEmpty(other.Etag)) return false;
            return Etag == other.Etag;
        }

        public override string ToString()
        {
            return response.ResponseString;
        }

        public string FormattedResponse { get { return Json.ToString(Formatting.Indented); } }

        public HttpWebResponse Response
        {
            get { throw new NotImplementedException(); }
        }

    }
}
