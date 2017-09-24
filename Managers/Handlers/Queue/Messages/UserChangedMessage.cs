using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer.Handlers.Queue.Messages
{
    public class UserChangedMessage : BaseMessage
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
