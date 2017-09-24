using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class Documents
    {
        public Documents()
        {
            Values = new List<Document>();
        }

        [JsonProperty("docs")]
        public List<Document> Values { get; set; }
    }

    public class Models
    {
        public Models()
        {
            Values = new List<IBaseObject>();
        }

        [JsonProperty("docs")]
        public List<IBaseObject> Values { get; set; }
    }
}
