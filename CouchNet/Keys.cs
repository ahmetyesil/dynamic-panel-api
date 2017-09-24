using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class Keys
    {
        public Keys()
        {
            Values = new List<string>();
        }

        [JsonProperty("keys")]
        public List<string> Values { get; set; }

    }
}
