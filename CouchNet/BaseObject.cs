using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchNet
{
    public abstract class BaseObject : IBaseObject
    {
        [JsonProperty("_id")]
        public virtual string Id { get; set; }
        [JsonProperty("_rev")]
        public virtual string Rev { get; set; }
    }
}