using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class MusicSettings : BaseSetting
    {
        [JsonProperty("video_coins")]
        public int VideoCoins { get; set; }
        [JsonProperty("video_length_limit")]
        public int VideoLengthLimit { get; set; }
    }
}
