using CouchNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ChatChannel : BaseObject
    {
        [JsonProperty("type")]
        public string ChatType { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("order_id")]
        public string OrderID { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("show_limit")]
        public int ShowLimit { get; set; }
    }
}
