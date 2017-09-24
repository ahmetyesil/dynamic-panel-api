using System;
using System.Linq;
using CouchNet;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Constants;
using CouchNet.Helper;

namespace Database
{
    public class User : BaseObject
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("mail")]
        public string Mail { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("coins")]
        public int Coin { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("game_login_token")]
        public string GameLoginToken { get; set; }
        [JsonProperty("chat_login_token")]
        public string ChatLoginToken { get; set; }
        [JsonProperty("last_ip")]
        public string LastIP { get; set; }
        [JsonProperty("state")]
        public UserState State { get; set; }
        [JsonProperty("admin_name")]
        public string AdminName { get; set; }
        [JsonProperty("google_auth_secret")]
        public string GoogleAuthSecret { get; set; }
        [JsonProperty("creation_date")]
        public string CreationDate { get; set; }
        [JsonProperty("last_password_change_date")]
        public string LastPasswordChangeDate { get; set; }
        [JsonProperty("last_login_date")]
        public string LastLoginDate { get; set; }
        [JsonProperty("trade_ban_finish_date")]
        public string TradeBanFinishDate { get; set; }
        [JsonProperty("activation_state")]
        public UserActivationState ActivationState { get; set; }
        [JsonProperty("rights")]
        public List<string> Rights { get; set; }
        [JsonProperty("referral")]
        public Referral Referral { get; set; }
        [JsonProperty("referral_settings")]
        public ReferralSettings ReferralSettings { get; set; }
        [JsonProperty("restrictions")]
        public List<UserState> Restrictions { get; set; }

        public void Set(Dictionary<string, object> list)
        {
            foreach (var elem in list)
            {
                string key = elem.Key;
                switch (key)
                {
                    case Changes.Password:
                        {
                            Password = elem.Value.ToString();
                        }
                        break;
                    case Changes.Mail:
                        {
                            Mail = elem.Value.ToString();
                        }
                        break;
                    case Changes.Phone:
                        {
                            Phone = elem.Value.ToString();
                        }
                        break;
                    case Changes.Avatar:
                        {
                            Avatar = elem.Value.ToString();
                        }
                        break;
                    case Changes.Name:
                        {
                            Name = elem.Value.ToString();
                        }
                        break;
                    case Changes.GameLoginToken:
                        {
                            GameLoginToken = elem.Value.ToString();
                        }
                        break;
                    case Changes.ChatLoginToken:
                        {
                            ChatLoginToken = elem.Value.ToString();
                        }
                        break;
                    case Changes.LastIP:
                        {
                            LastIP = elem.Value.ToString();
                        }
                        break;
                    case Changes.State:
                        {
                            State = ((JObject)elem.Value).ToObject<UserState>();
                        }
                        break;
                    case Changes.AdminName:
                        {
                            AdminName = elem.Value.ToString();
                        }
                        break;
                    case Changes.GoogleAuthSecret:
                        {
                            GoogleAuthSecret = elem.Value.ToString();
                        }
                        break;
                    case Changes.CreationDate:
                        {
                            CreationDate = elem.Value.ToString();
                        }
                        break;
                    case Changes.LastPasswordChangeDate:
                        {
                            LastPasswordChangeDate = elem.Value.ToString();
                        }
                        break;
                    case Changes.LastLoginDate:
                        {
                            LastLoginDate = elem.Value.ToString();
                        }
                        break;
                    case Changes.TradeBanFinishDate:
                        {
                            TradeBanFinishDate = elem.Value.ToString();
                        }
                        break;
                    case Changes.ActivationState:
                        {
                            ActivationState = ((JObject)elem.Value).ToObject<UserActivationState>();
                        }
                        break;
                    case Changes.Rights:
                        {
                            if (elem.Value == null)
                            {
                                Rights = new List<string>();
                            }
                            else
                            {
                                Rights = ((JArray)elem.Value).ToObject<List<string>>();
                            }
                        }
                        break;
                    case Changes.Referral:
                        {
                            Referral = ((JObject)elem.Value).ToObject<Referral>();
                        }
                        break;
                    case Changes.ReferralSettings:
                        {
                            ReferralSettings = ((JObject)elem.Value).ToObject<ReferralSettings>();
                        }
                        break;
                    case Changes.Restrictions:
                        {
                            if (elem.Value == null)
                            {
                                Restrictions = new List<UserState>();
                            }
                            else
                            {
                                Restrictions = ((JObject)elem.Value).ToObject<List<UserState>>();
                            }
                        }
                        break;
                }
            }
        }
    }


    public class Referral
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("referrer_id")]
        public string ReferrerID { get; set; }
    }

    public class ReferralSettings
    {
        [JsonProperty("last_referral_collect_date")]
        public string LastReferralCollectDate { get; set; }
        [JsonProperty("referrer_bonus_coins")]
        public int ReferrerBonusCoins { get; set; }
        [JsonProperty("referred_bonus_coins")]
        public int ReferredBonusCoins { get; set; }
    }

    public class UserState
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("action_date")]
        public string ActionDate { get; set; }
        [JsonProperty("staff")]
        public string Staff { get; set; }
        [JsonProperty("staff_id")]
        public string StaffID { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
        [JsonProperty("is_permanent")]
        public bool IsPermanent { get; set; }
        [JsonProperty("end_date")]
        public string EndDate { get; set; }
    }

    public class UserActivationState
    {
        [JsonProperty("mail_activity")]
        public UserActivation MailActivity { get; set; }
        [JsonProperty("mobile_activity")]
        public UserActivation MobileActivity { get; set; }
    }

    public class UserActivation
    {
        [JsonProperty("is_activated")]
        public bool IsActivated { get; set; }
        [JsonProperty("first_activation_date")]
        public string FirstActivationDate { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("next_activation_date")]
        public string NextActivationDate { get; set; }
    }
}
