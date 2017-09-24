using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class CouchResponseObject : JObject
    {
        public CouchResponseObject(JObject obj) : base(obj) { }
        public int StatusCode { get; set; }
    }
}
