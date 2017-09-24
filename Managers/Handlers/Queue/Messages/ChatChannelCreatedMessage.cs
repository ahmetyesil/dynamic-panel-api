using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Handlers.Queue.Messages
{
    public class ChatChannelNotificationMessage : BaseMessage
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
