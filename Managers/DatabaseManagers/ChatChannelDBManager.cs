using CouchNet;
using CouchNet.Helper;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class ChatChannelDBManager : ManagerBase<ChatChannelDBManager>
    {
        public ChatChannel GetChannel(string ID)
        {
            try
            {
                return DatabaseManager.Instance.ChatChannels.GetDocument<ChatChannel>(ID);
            }
            catch
            {
                return null;
            }
        }

        public List<ChatChannel> GetActives(string Type = null)
        {
            try
            {
                ViewOptions options = null;
                if (!string.IsNullOrWhiteSpace(Type))
                {
                    options = new ViewOptions();
                    options.Key = new KeyOptions(Type);
                }
                return DatabaseManager.Instance.ChatChannels.View<ChatChannel>("getByType", options, "activeChannels").Items;
            }
            catch
            {
                return null;
            }
        }

        public List<ChatChannel> GetActivesByUserID(string UserID)
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Key = new KeyOptions(UserID);
                return DatabaseManager.Instance.ChatChannels.View<ChatChannel>("getByUserID", options, "activeChannels").Items;
            }
            catch
            {
                return null;
            }
        }

        public ChatChannel Save(ChatChannel channel)
        {
            try
            {
                return DatabaseManager.Instance.ChatChannels.SaveDocument<ChatChannel>(channel);
            }
            catch
            {
                return null;
            }
        }
    }
}
