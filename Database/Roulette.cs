using System;
using Newtonsoft.Json;
using Constants;
using System.Collections.Generic;

namespace Database
{
    public class Roulette : BaseGame
    {
        [JsonProperty("type")]
        public override string Type
        {
            get { return GameTypes.Roulette; }
        }

        [JsonProperty("winner_number")]
        public int WinnerNumber { get; set; }
        [JsonProperty("winner_number_value")]
        public string WinnerNumberValue { get; set; }
        [JsonProperty("multiplier")]
        public int Multiplier { get; set; }
        [JsonProperty("bets")]
        public List<RouletteBet> Bets { get; set; }
    }

    public class RouletteBet
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("choice")]
        public string Choice { get; set; }
        [JsonProperty("coins")]
        public int BetCoins { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("withdraw_coins")]
        public int Withdraw { get; set; }

    }
}
