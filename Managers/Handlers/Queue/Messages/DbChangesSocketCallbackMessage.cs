using Newtonsoft.Json;
using System.Collections.Generic;

namespace SocketServer.Handlers.Queue.Messages
{
    public class DbChangesSocketCallbackMessage : BaseMessage
    {
        [JsonProperty("room_id")]
        public string RoomID { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("coins")]
        public int Coin { get; set; }
        [JsonProperty("properties")]
        public Dictionary<string, string> Properties { get; set; }
    }
}
