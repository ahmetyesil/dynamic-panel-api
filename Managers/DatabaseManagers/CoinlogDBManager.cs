using Constants;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class CoinlogDBManager : ManagerBase<CoinlogDBManager>
    {
        public Coinlog CreateCoinlog(string UserID, int AfterCoin, int Coin, int BeforeCoin, string Type, string SubType, string LogType, string StuffID, string StuffName, string RelatedId)
        {
            try
            {
                var coinlog = new Coinlog();
                coinlog.AfterCoin = AfterCoin;
                coinlog.BeforeCoin = BeforeCoin;
                coinlog.Coin = Math.Abs(Coin);
                coinlog.Date = DateTime.Now;
                coinlog.RelatedID = RelatedId;
                coinlog.StuffID = StuffID;
                coinlog.StuffName = StuffName;
                coinlog.Type = Type;
                coinlog.SubType = SubType;
                coinlog.LogType = LogType;
                coinlog.UserID = UserID;
                return DatabaseManager.Instance.Coinlogs.CreateDocument<Coinlog>(coinlog);
            }
            catch
            {
                return null;
            }
        }

    }
}