using Newtonsoft.Json.Linq;
using Database;
using Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketServer.Handlers.Queue.Messages;
using Newtonsoft.Json;

namespace Managers.DatabaseManagers
{
    public class UserDBManager : ManagerBase<UserDBManager>
    {

        public delegate void UserDataChangedDelegate(User user);
        public event UserDataChangedDelegate UserDataChanged;


        public User GetUser(string ID)
        {
            try
            {
                return DatabaseManager.Instance.Users.GetDocument<User>(ID);
            }
            catch
            {
                return null;
            }
        }

        public User Save(User user)
        {
            try
            {
                user = DatabaseManager.Instance.Users.SaveDocument<User>(user);
                if (UserDataChanged != null)
                {
                    UserDataChanged(user);
                }
                return user;
            }
            catch
            {
                return null;
            }
        }

        public User Set(User user, Dictionary<string, object> data)
        {
            try
            {
                user.Set(data);
                return Save(user);
            }
            catch
            {
                return null;
            }
        }

        public User Increase(User user, Dictionary<string, int> data, Reason reason)
        {
            try
            {
                int beforeCoin = user.Coin;
                foreach (var elem in data)
                {
                    switch (elem.Key)
                    {
                        case Changes.Coin:
                            {
                                user.Coin += elem.Value;
                            }
                            break;
                    }
                }
                user = Save(user);
                SendToCoinlog(user.Id, reason, data[Changes.Coin], beforeCoin, user.Coin);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public User Decrease(User user, Dictionary<string, int> data, Reason reason)
        {
            try
            {
                int beforeCoin = user.Coin;
                foreach (var elem in data)
                {
                    switch (elem.Key)
                    {
                        case Changes.Coin:
                            {
                                user.Coin -= elem.Value;
                            }
                            break;
                    }
                }
                user = Save(user);
                SendToCoinlog(user.Id, reason, data[Changes.Coin], beforeCoin, user.Coin);
                return user;
            }
            catch
            {
                return null;
            }
        }

        private void SendToCoinlog(string UserID, Reason reason, int Coin, int BeforeCoin, int AfterCoin)
        {
            if (reason != null)
            {
                CoinlogMessage message = new CoinlogMessage();
                message.AfterCoin = AfterCoin;
                message.Coin = Coin;
                message.BeforeCoin = BeforeCoin;
                message.LogType = reason.LogType;
                message.RelatedID = reason.RelatedID;
                message.StuffID = reason.StuffID;
                message.StuffName = reason.StuffName;
                message.SubType = reason.SubType;
                message.Type = reason.Type;
                message.UserID = UserID;

                QueueManager.Instance.Queue(Exchanges.DbChanges, message, Routes.Coinlog);
            }
        }

        public User Increase(string Id, Dictionary<string, int> winPriceData, Reason reason)
        {
            try
            {
                var user = GetUser(Id);
                return Increase(user, winPriceData, reason);
            }
            catch
            {
                return null;
            }
        }

        public User Decrease(string Id, Dictionary<string, int> winPriceData, Reason reason)
        {
            try
            {
                var user = GetUser(Id);
                return Decrease(user, winPriceData, reason);
            }
            catch
            {
                return null;
            }
        }

        public User DbChange(DbChangesMessage message, User doc = null)
        {
            try
            {
                if (doc == null)
                {
                    doc = DatabaseManager.Instance.Users.GetDocument<User>(message.ID);
                }

                switch (message.Method)
                {
                    case Methods.Set:
                        {
                            doc.Set(message.Changes);
                        }
                        break;
                    case Methods.Increase:
                    case Methods.Decrease:
                        {
                            var data = message.Changes.ToString();
                            var list = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);
                            foreach (var elem in list)
                            {
                                switch (elem.Key)
                                {
                                    case Changes.Coin:
                                        {
                                            var coin = elem.Value;
                                            if (message.Method == Methods.Increase)
                                            {
                                                doc.Coin += coin;
                                            }
                                            else
                                            {
                                                doc.Coin -= coin;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                }
                return Save(doc);
            }
            catch
            {
                return null;
            }
        }
    }
}
