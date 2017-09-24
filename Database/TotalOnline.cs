using CouchNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class TotalOnline : BaseObject
    {
        [JsonProperty("type")]
        public string Type
        {
            get
            {
                return "maximum_online_count";
            }
        }

        [JsonProperty("total_online")]
        public int Online { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
