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
    public class CoinflipDBManager : ManagerBase<CoinflipDBManager>
    {
        public Coinflip CreateCoinflip(User User, Room room, int Coin, Dictionary<string, int> DecreaseData)
        {
            try
            {
                Reason reason = new Reason();
                reason.LogType = LogTypes.Out;
                reason.StuffName = "socket";
                reason.Type = CoinlogTypes.Game;
                reason.SubType = GameTypes.Coinflip;

                UserDBManager.Instance.Decrease(User, DecreaseData, reason);
                Coinflip coinFlip = new Coinflip();
                coinFlip.OwnerUserID = User.Id;
                coinFlip.TotalDepositCoins = Coin;
                coinFlip.RoomID = room.Id;
                coinFlip.PrivateHash = RandomManager.Instance.CreateHash();
                coinFlip.WinPercentage = (float)RandomManager.Instance.CreatePercentage();
                coinFlip.PublicHash = HashManager.Instance.Md5(coinFlip.WinPercentage + ":" + coinFlip.PrivateHash);
                coinFlip.StartDate = DateTime.Now;
                coinFlip.EndDate = DateTime.Now.AddHours(3);
                coinFlip.Status = GameStatus.Ongoing;
                coinFlip.WithdrawStoppage = room.Stoppage;
                return DatabaseManager.Instance.Coinflips.CreateDocument<Coinflip>(coinFlip);
            }
            catch
            {
                return null;
            }
        }

        public Coinflip GetCoinflip(string ID)
        {
            try
            {
                return DatabaseManager.Instance.Coinflips.GetDocument<Coinflip>(ID);
            }
            catch
            {
                return null;
            }
        }

        public Coinflip JoinCoinflip(User User, Coinflip Coinflip, int Coin, Dictionary<string, int> DecreaseData)
        {
            try
            {
                Dictionary<string, int> winPriceData = new Dictionary<string, int>();

                Reason reason = new Reason();
                reason.LogType = LogTypes.Out;
                reason.StuffName = "socket";
                reason.Type = CoinlogTypes.Game;
                reason.SubType = GameTypes.Coinflip;
                reason.RelatedID = Coinflip.Id;

                User = UserDBManager.Instance.Decrease(User, DecreaseData, reason);
                Coinflip.OpponentUserID = User.Id;
                Coinflip.EndDate = DateTime.Now;
                Coinflip.Status = GameStatus.Completed;
                Coinflip.TotalDepositCoins *= 2;
                Coinflip.TotalWithdrawCoins = (Coinflip.TotalDepositCoins * (100 - Coinflip.WithdrawStoppage)) / 100;

                winPriceData[Changes.Coin] = Coinflip.TotalWithdrawCoins;

                reason = new Reason();
                reason.LogType = LogTypes.In;
                reason.StuffName = "socket";
                reason.Type = CoinlogTypes.Game;
                reason.SubType = GameTypes.Coinflip;
                reason.RelatedID = Coinflip.Id;

                if (Coinflip.WinPercentage <= 50)
                {
                    Coinflip.WinnerType = CoinflipWinnerTypes.Owner;
                    Coinflip.WinnerUserID = Coinflip.OwnerUserID;
                    UserDBManager.Instance.Increase(Coinflip.OwnerUserID, winPriceData, reason);
                }
                else
                {
                    Coinflip.WinnerType = CoinflipWinnerTypes.Opponent;
                    Coinflip.WinnerUserID = Coinflip.OpponentUserID;
                    UserDBManager.Instance.Increase(User, winPriceData, reason);
                }

                return this.Save(Coinflip);
            }
            catch
            {
                return null;
            }
        }

        public Coinflip Save(Coinflip Coinflip)
        {
            try
            {
                return DatabaseManager.Instance.Coinflips.SaveDocument<Coinflip>(Coinflip);
            }
            catch
            {
                return null;
            }
        }

        public List<Coinflip> GetActiveCoinflips(string RoomID)
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Key = new KeyOptions(GameStatus.Ongoing, RoomID);
                return DatabaseManager.Instance.Coinflips.View<Coinflip>("getByStatus", options, "game").Items;
            }
            catch
            {
                return null;
            }
        }
    }
}
