using CouchNet;
using CouchNet.Helper;
using Database;
using Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class JackpotDBManager : ManagerBase<JackpotDBManager>
    {
        public Jackpot GetJackpot(string ID)
        {
            try
            {
                return DatabaseManager.Instance.Jackpots.GetDocument<Jackpot>(ID);
            }
            catch
            {
                return null;
            }
        }

        public Jackpot Save(Jackpot Jackpot)
        {
            try
            {
                return DatabaseManager.Instance.Jackpots.SaveDocument<Jackpot>(Jackpot);
            }
            catch
            {
                return null;
            }
        }

        public Jackpot GetActiveJackpot(string RoomID)
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Key = new KeyOptions(GameStatus.Ongoing, RoomID);
                options.Limit = 1;
                var items = DatabaseManager.Instance.Jackpots.View<Jackpot>("getByStatus", options, "game").Items;
                return items.Count > 0 ? items[0] : null;
            }
            catch
            {
                return null;
            }
        }

        public Jackpot CreateJackpot(Room room)
        {
            try
            {
                var jackpot = new Jackpot();
                jackpot.Bets = new List<JackpotBet>();
                jackpot.EndDate = DateTime.Now;
                jackpot.PrivateHash = RandomManager.Instance.CreateHash();
                jackpot.RoomID = room.Id;
                jackpot.StartDate = DateTime.Now;
                jackpot.Status = GameStatus.Ongoing;
                jackpot.WinPercentage = (float)RandomManager.Instance.CreatePercentage();
                jackpot.WithdrawStoppage = room.Stoppage;
                jackpot.PublicHash = HashManager.Instance.Md5(jackpot.WinPercentage + ":" + jackpot.PrivateHash);
                return Save(jackpot);
            }
            catch
            {
                return null;
            }
        }
    }
}
