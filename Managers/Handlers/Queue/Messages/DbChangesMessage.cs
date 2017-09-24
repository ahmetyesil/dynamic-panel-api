using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SocketServer.Handlers.Queue.Messages
{
    public class DbChangesMessage : BaseMessage
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("database")]
        public string Database { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("callback")]
        public DbChangesPanelCallbackMessage PanelCallback { get; set; }
        [JsonProperty("socket_callback")]
        public DbChangesSocketCallbackMessage SocketCallback { get; set; }
        [JsonProperty("changes")]
        public Dictionary<string, object> Changes { get; set; }
        [JsonProperty("reason")]
        public Reason Reason { get; set; }
    }

    public class Reason
    {
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
