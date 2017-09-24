using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Handlers.Queue.Messages
{
    public class CoinlogMessage : BaseMessage
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }

        [JsonProperty("coin")]
        public int Coin { get; set; }

        [JsonProperty("after_coin")]
        public int AfterCoin { get; set; }

        [JsonProperty("before_coin")]
        public int BeforeCoin { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("sub_type")]
        public string SubType { get; set; }

        [JsonProperty("log_type")]
        public string LogType { get; set; }

        [JsonProperty("stuff_id")]
        public string StuffID { get; set; }

        [JsonProperty("stuff_name")]
        public string StuffName { get; set; }

        [JsonProperty("related_id")]
        public string RelatedID { get; set; }
    }
}
