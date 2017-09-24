using CouchNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ChatMessage : BaseObject
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("channel_id")]
        public string ChannelID { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
