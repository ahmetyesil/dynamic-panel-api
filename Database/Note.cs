using System;
using CouchNet;
using Newtonsoft.Json;

namespace Database
{
    public class Note : BaseObject
    {
        [JsonProperty("note_type")]
        public string Type { get; set; }
        [JsonProperty("note_text")]
        public string Text { get; set; }
        [JsonProperty("note_related_id")]
        public string RelatedID { get; set; }
        [JsonProperty("note_stuff")]
        public string Stuff { get; set; }
        [JsonProperty("note_stuff_user_id")]
        public string StuffUserID { get; set; }
        [JsonProperty("note_level")]
        public string Level { get; set; }
        [JsonProperty("private")]
        public string Private { get; set; }
        [JsonProperty("note_created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
