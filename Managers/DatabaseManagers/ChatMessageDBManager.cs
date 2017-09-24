using CouchNet;
using CouchNet.Helper;
using Constants;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class ChatMessageDBManager : ManagerBase<ChatMessageDBManager>
    {
        public ChatMessage CreateChatMessage(string UserID, string ChannelID, string Message, bool IsPublic)
        {
            try
            {
                var chatMessage = new ChatMessage();
                chatMessage.ChannelID = ChannelID;
                chatMessage.Date = DateTime.Now;
                chatMessage.Message = Message;
                chatMessage.UserID = UserID;
                chatMessage.State = ChatMessageStates.Active;
                if (IsPublic)
                {
                    return DatabaseManager.Instance.PublicChatLogs.CreateDocument<ChatMessage>(chatMessage);
                }
                else
                {
                    return DatabaseManager.Instance.PrivateChatLogs.CreateDocument<ChatMessage>(chatMessage);
                }
            }
            catch
            {
                return null;
            }
        }

        public List<ChatMessage> GetActiveChatMessages(ChatChannel channel)
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Descending = true;
                options.Limit = channel.ShowLimit;
                options.StartKey = new KeyOptions(channel.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                options.EndKey = new KeyOptions(channel.Id, DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss"));
                var db = DatabaseManager.Instance.PublicChatLogs;
                if (channel.ChatType != ChatChannelType.Public)
                {
                    db = DatabaseManager.Instance.PrivateChatLogs;
                }
                return db.View<ChatMessage>(Views.GetByChannelID, options, DesignDocuments.Active).Items;
            }
            catch
            {
                return null;
            }
        }

        public List<ChatMessage> GetActiveUserChatMessages(string UserID, string ChannelID, int limit = 0)
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Descending = true;
                options.Key = new KeyOptions(ChannelID, UserID);
                if (limit > 0)
                {
                    options.Limit = limit;
                }
                var db = DatabaseManager.Instance.PublicChatLogs;
                if (ChannelID != ChatChannelType.Public)
                {
                    db = DatabaseManager.Instance.PrivateChatLogs;
                }
                return db.View<ChatMessage>(Views.GetByChannelIDAndUserID, options, DesignDocuments.Active).Items;
            }
            catch
            {
                return null;
            }
        }

        public void ClearUserMessage(string UserID)
        {
            var message_list = GetActiveUserChatMessages(UserID, ChatChannelType.Public, 50);
            foreach (var message in message_list)
            {
                if (message.UserID == UserID)
                {
                    message.State = ChatMessageStates.Hidden;
                    Save(message);
                }
            }
        }

        private ChatMessage Save(ChatMessage message)
        {
            try
            {
                if (message.ChannelID == ChatChannelType.Public)
                {
                    return DatabaseManager.Instance.PublicChatLogs.SaveDocument<ChatMessage>(message);
                }
                else
                {
                    return DatabaseManager.Instance.PrivateChatLogs.SaveDocument<ChatMessage>(message);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
