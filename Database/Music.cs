using CouchNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Music : BaseObject
    {
        [JsonProperty("youtube_url")]
        public string YoutubeUrl { get; set; }
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("fail_reason")]
        public string FailReason { get; set; }
        [JsonProperty("video_length")]
        public int VideoLength { get; set; }
        [JsonProperty("video_file")]
        public string VideoFile { get; set; }
        [JsonProperty("video_title")]
        public string VideoTitle { get; set; }
        [JsonProperty("coins")]
        public int Coin { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
    }
}
