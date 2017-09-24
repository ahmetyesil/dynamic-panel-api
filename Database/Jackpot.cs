using System;
using Newtonsoft.Json;
using Constants;
using System.Collections.Generic;

namespace Database
{
    public class Jackpot : BaseGame
    {
        [JsonProperty("type")]
        public override string Type
        {
            get { return GameTypes.Jackpot; }
        }

        [JsonProperty("winner_ticket")]
        public int WinnerTicket { get; set; }
        [JsonProperty("win_percentage")]
        public float WinPercentage { get; set; }
        [JsonProperty("winner_user_id")]
        public string WinnerUserID { get; set; }
        [JsonProperty("withdraw_stoppage")]
        public int WithdrawStoppage { get; set; }
        [JsonProperty("bets")]
        public List<JackpotBet> Bets { get; set; }
    }

    public class JackpotBet
    {
        [JsonProperty("user_id")]
        public string UserID { get; set; }
        [JsonProperty("coins")]
        public int Coins { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("is_winner")]
        public bool IsWinner { get; set; }
        [JsonProperty("first_ticket")]
        public int FirstTicket { get; set; }
        [JsonProperty("last_ticket")]
        public int LastTicket { get; set; }
    }
}
