using CouchNet;
using Newtonsoft.Json;

namespace Database
{
    public class BaseSetting : BaseObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
