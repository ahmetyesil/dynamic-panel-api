using Newtonsoft.Json;
using System.Collections.Generic;

namespace SocketServer.Handlers.Queue.Messages
{
    public class DbChangesPanelCallbackMessage : BaseMessage
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("payload")]
        public Dictionary<string, string> Payloads { get; set; }
    }
}
