using Newtonsoft.Json;
using Constants;

namespace Database
{
    public class Coinflip : BaseGame
    {
        [JsonProperty("type")]
        public override string Type
        {
            get { return GameTypes.Coinflip; }
        }

        [JsonProperty("owner_user_id")]
        public string OwnerUserID { get; set; }
        [JsonProperty("opponent_user_id")]
        public string OpponentUserID { get; set; }
        [JsonProperty("winner_user_id")]
        public string WinnerUserID { get; set; }
        [JsonProperty("winner_type")]
        public string WinnerType { get; set; }
        [JsonProperty("win_percentage")]
        public float WinPercentage { get; set; }
        [JsonProperty("withdraw_stoppage")]
        public int WithdrawStoppage { get; set; }
    }
}
