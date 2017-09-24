using CouchNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Coinlog : BaseObject
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("sub_type")]
        public string SubType { get; set; }
        [JsonProperty("log_type")]
        public string LogType { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("related_id")]
        public string RelatedID { get; set; }
        [JsonProperty("coins")]
        public int Coin { get; set; }
        [JsonProperty("before_coin")]
        public int BeforeCoin { get; set; }
        [JsonProperty("after_coin")]
        public int AfterCoin { get; set; }
        [JsonProperty("stuff_id")]
        public string StuffID { get; set; }
        [JsonProperty("stuff_name")]
        public string StuffName { get; set; }
    }
}
