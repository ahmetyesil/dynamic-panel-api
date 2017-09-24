using CouchNet;
using Constants;
using Database;
using SocketServer.Handlers.Queue.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class MusicDBManager : ManagerBase<MusicDBManager>
    {
        public Music CreateMusic(User User, string Url, int Coin, Dictionary<string, int> DecreaseData)
        {
            try
            {
                Reason reason = new Reason();
                reason.LogType = LogTypes.Out;
                reason.StuffName = "socket";
                reason.Type = CoinlogTypes.Extra;
                reason.SubType = ExtraTypes.Music;

                UserDBManager.Instance.Decrease(User, DecreaseData, reason);
                var music = new Music();
                music.Status = MusicStatus.Waiting;
                music.CreatedAt = DateTime.Now;
                music.UserID = User.Id;
                music.YoutubeUrl = Url;
                music.Coin = Coin;
                music = DatabaseManager.Instance.Musics.CreateDocument<Music>(music);
                return music;
            }
            catch
            {
                return null;
            }
        }

        public Music GetMusic(string ID)
        {
            try
            {
                var music = DatabaseManager.Instance.Musics.GetDocument<Music>(ID);
                return music;
            }
            catch
            {
                return null;
            }
        }

        public Music GetLastMusic()
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Limit = 1;
                options.Descending = true;
                return DatabaseManager.Instance.Musics.View<Music>("getByEndDate", options, "music").Items.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public Music GetNextMusic()
        {
            try
            {
                ViewOptions options = new ViewOptions();
                options.Limit = 1;
                options.StartKey = new CouchNet.Helper.KeyOptions(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return DatabaseManager.Instance.Musics.View<Music>("getByEndDate", options, "music").Items.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public Music Save(Music music)
        {
            try
            {
                return DatabaseManager.Instance.Musics.SaveDocument<Music>(music);
            }
            catch
            {
                return null;
            }
        }
    }
}
