using CouchNet.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class Document<T> : Document
    {
        public Document(T obj) : base(ObjectSerializer.Serialize(obj)) { }
    }

    public class Document : JObject, IBaseObject
    {
        [JsonIgnore]
        public string Id
        {
            get
            {
                JToken id;
                return this.TryGetValue("_id", out id) ? id.ToString() : null;
            }
            set { this["_id"] = value; }
        }

        [JsonIgnore]
        public string Rev
        {
            get
            {
                JToken rev;
                return this.TryGetValue("_rev", out rev) ? rev.ToString() : null;
            }
            set { this["_rev"] = value; }
        }



        protected Document() { }
        public Document(JObject jobj)
            : base(jobj)
        {
            JToken tmp;
            if (jobj.TryGetValue("id", out tmp))
                this.Id = tmp.Value<string>();
            if (jobj.TryGetValue("rev", out tmp))
                this.Rev = tmp.Value<string>();
            if (jobj.TryGetValue("_id", out tmp))
                this.Id = tmp.Value<string>();
            if (jobj.TryGetValue("_rev", out tmp))
                this.Rev = tmp.Value<string>();
        }

        public Document(string json) : base(JObject.Parse(json)) { }

        public bool HasAttachment
        {
            get { return this["_attachments"] != null; }
        }

        public IEnumerable<string> GetAttachmentNames()
        {
            var attachment = this["_attachments"];
            if (attachment == null)
                return null;
            return attachment.Select(x => x.Value<JProperty>().Name);
        }
    }
}
