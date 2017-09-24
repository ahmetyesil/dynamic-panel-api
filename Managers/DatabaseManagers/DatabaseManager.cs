using CouchNet;
using System.Text;
using System.Dynamic;
using Newtonsoft.Json;
using Constants;
using System.Configuration;
using Database;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using SocketServer.Handlers.Queue.Messages;

namespace Managers
{
    public class DatabaseManager : ManagerBase<DatabaseManager>
    {
        public CouchDatabase Users { get; set; }
        public CouchDatabase Coinflips { get; set; }
        public CouchDatabase Roulettes { get; set; }
        public CouchDatabase Jackpots { get; set; }
        public CouchDatabase Rooms { get; set; }
        public CouchDatabase ChatChannels { get; set; }
        public CouchDatabase PublicChatLogs { get; set; }
        public CouchDatabase PrivateChatLogs { get; set; }
        public CouchDatabase Coinlogs { get; set; }
        public CouchDatabase Musics { get; set; }
        public CouchDatabase Reports { get; set; }


        public override void Initialize()
        {
            base.Initialize();
            var HostName = ConfigurationManager.AppSettings["couch_host"];
            var Port = int.Parse(ConfigurationManager.AppSettings["couch_port"]);
            var UserName = ConfigurationManager.AppSettings["couch_username"];
            var Password = ConfigurationManager.AppSettings["couch_password"];
            CouchConfiguration.Initialize(HostName, Port, UserName, Password);
            Users = new CouchDatabase(Databases.Users);
            Coinflips = new CouchDatabase(Databases.Coinflips);
            Roulettes = new CouchDatabase(Databases.Roulettes);
            Jackpots = new CouchDatabase(Databases.Jackpots);
            Rooms = new CouchDatabase(Databases.Settings);
            ChatChannels = new CouchDatabase(Databases.ChatChannels);
            PublicChatLogs = new CouchDatabase(Databases.PublicChatLogs);
            PrivateChatLogs = new CouchDatabase(Databases.PrivateChatLogs);
            Coinlogs = new CouchDatabase(Databases.Coinlogs);
            Musics = new CouchDatabase(Databases.Musics);
            Reports = new CouchDatabase(Databases.Reports);
        }
    }
}
