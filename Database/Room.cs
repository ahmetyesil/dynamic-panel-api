using Newtonsoft.Json;

namespace Database
{
    public class Room : BaseSetting
    {
        [JsonProperty("room_id")]
        public int ID { get; set; }
        [JsonProperty("game_type")]
        public string GameType { get; set; }
        [JsonProperty("stoppage")]
        public int Stoppage { get; set; }
        [JsonProperty("round_time")]
        public int RoundTime { get; set; }
        [JsonProperty("round_wait_time")]
        public int RoundWaitTime { get; set; }
        [JsonProperty("max_bet_count")]
        public int MaxBetCount { get; set; }
        [JsonProperty("min_bet_coins")]
        public int MinBetCoins { get; set; }
        [JsonProperty("max_bet_coins")]
        public int MaxBetCoins { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
