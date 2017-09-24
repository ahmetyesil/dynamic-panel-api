using CouchNet;
using CouchNet.Helper;
using Database;
using Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketServer.Handlers.Queue.Messages;

namespace Managers.DatabaseManagers
{
    public class RouletteDBManager : ManagerBase<RouletteDBManager>
    {
        public Roulette GetRoulette(string ID)
        {
            try
            {
                return DatabaseManager.Instance.Roulettes.GetDocument<Roulette>(ID);
            }
            catch
            {
                return null;
            }
        }

        public Roulette Save(Roulette Roulette)
        {
            try
            {
                return DatabaseManager.Instance.Roulettes.SaveDocument<Roulette>(Roulette);
            }
            catch
            {
                return null;
            }
        }

        public Roulette JoinRoulette(User User, Room room, Roulette roulette, string Choice, int Coin, Dictionary<string, int> DecreaseData)
        {
            try
            {
                Reason reason = new Reason();
                reason.LogType = LogTypes.Out;
                reason.StuffName = "socket";
                reason.Type = CoinlogTypes.Game;
                reason.SubType = GameTypes.Roulette;
                reason.RelatedID = roulette.Id;

                UserDBManager.Instance.Decrease(User, DecreaseData, reason);
                roulette.Bets.Add(new RouletteBet() { UserID = User.Id, Choice = Choice, BetCoins = Coin, Date = DateTime.Now });
                return Save(roulette);
            }
            catch
            {
                return null;
            }
        }

        public Roulette FinishRoulette(string RouletteID)
        {
            try
            {
                var roulette = GetRoulette(RouletteID);
                if (roulette.Status != GameStatus.Completed)
                {
                    foreach (var bet in roulette.Bets)
                    {
                        if (roulette.WinnerNumberValue == bet.Choice)
                        {
                            Reason reason = new Reason();
                            reason.LogType = LogTypes.In;
                            reason.StuffName = "socket";
                            reason.Type = CoinlogTypes.Game;
                            reason.SubType = GameTypes.Roulette;
                            reason.RelatedID = RouletteID;

                            bet.Withdraw = bet.BetCoins * roulette.Multiplier;
                            Dictionary<string, int> IncreaseData = new Dictionary<string, int>();
                            IncreaseData.Add(Changes.Coin, bet.Withdraw);
                            UserDBManager.Instance.Increase(bet.UserID, IncreaseData, reason);
                        }
                    }

                    roulette.TotalDepositCoins = roulette.Bets.Sum(t => t.BetCoins);
                    roulette.TotalWithdrawCoins = roulette.Bets.Sum(t => t.Withdraw);
                    roulette.EndDate = DateTime.Now;
                    roulette.Status = GameStatus.Completed;
                    return Save(roulette);
                }
                return roulette;
            }
            catch
            {
                return null;
            }
        }

        public Roulette CreateRoulette(Roulette old_roulette, Room room)
        {
            try
            {
                var roulette = new Roulette();
                roulette.Bets = new List<RouletteBet>();
                roulette.EndDate = DateTime.Now.AddSeconds(room.RoundTime + room.RoundWaitTime);
                roulette.PrivateHash = old_roulette.PrivateHash;
                roulette.PublicHash = old_roulette.PublicHash;
                roulette.RoomID = old_roulette.RoomID;
                roulette.RoundID = old_roulette.RoundID + 1;
                roulette.StartDate = DateTime.Now;
                roulette.Status = GameStatus.Ongoing;
                roulette.Multiplier = 2;

                if (old_roulette.EndDate.Day != DateTime.Today.Day || old_roulette.EndDate.Month != DateTime.Today.Month && old_roulette.EndDate.Year != DateTime.Today.Year)
                {
                    roulette.PrivateHash = RandomManager.Instance.CreateHash();
                    roulette.PublicHash = RandomManager.Instance.CreateHash();
                }

                var hash = HashManager.Instance.Sha256(roulette.PrivateHash + "-NOGAME_WIN-" + roulette.PublicHash + "-" + roulette.RoundID);
                var roll = (int)(HashManager.Instance.HexToInt64(hash.Substring(0, 8)) % 15);
                roulette.WinnerNumber = roll;
                if (roll > 7)
                {
                    roulette.WinnerNumberValue = RouletteChoices.Black;
                }
                else if (roll == 0)
                {
                    roulette.WinnerNumberValue = RouletteChoices.Green;
                }
                else
                {
                    roulette.WinnerNumberValue = RouletteChoices.Red;
                }

                if (roulette.WinnerNumberValue == RouletteChoices.Green)
                {
                    roulette.Multiplier = 14;
                }

                return Save(roulette);
            }
            catch
            {
                return null;
            }
        }

        public Roulette GetLastRoulette()
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Limit = 1;
                options.Descending = true;
                return DatabaseManager.Instance.Roulettes.View<Roulette>("getByRoundId", options, "game").Items.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public List<Roulette> GetLastRoulettes(int Count)
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Limit = Count;
                options.Descending = true;
                return DatabaseManager.Instance.Roulettes.View<Roulette>("getByRoundId", options, "game").Items;
            }
            catch
            {
                return null;
            }
        }
    }
}
