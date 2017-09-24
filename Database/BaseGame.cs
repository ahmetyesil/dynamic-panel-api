using System;
using CouchNet;
using Newtonsoft.Json;

namespace Database
{
    public abstract class BaseGame : BaseObject
    {
        [JsonProperty("round_id")]
        public int RoundID { get; set; }
        [JsonProperty("type")]
        public abstract string Type { get; }
        [JsonProperty("total_deposit_coins")]
        public int TotalDepositCoins { get; set; }
        [JsonProperty("total_withdraw_coins")]
        public int TotalWithdrawCoins { get; set; }
        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }
        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }
        [JsonProperty("room_id")]
        public string RoomID { get; set; }
        [JsonProperty("public_hash")]
        public string PublicHash { get; set; }
        [JsonProperty("private_hash")]
        public string PrivateHash { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
