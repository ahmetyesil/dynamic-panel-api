using CouchNet;
using CouchNet.Helper;
using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers.DatabaseManagers
{
    public class SettingsDBManager : ManagerBase<SettingsDBManager>
    {
        public Room GetRoom(string ID)
        {
            try
            {
                return DatabaseManager.Instance.Rooms.GetDocument<Room>(ID);
            }
            catch
            {
                return null;
            }
        }

        public List<Room> GetActiveRooms()
        {
            try
            {
                return DatabaseManager.Instance.Rooms.View<Room>("getActives", "room").Items;
            }
            catch
            {
                return null;
            }
        }

        public MusicSettings GetMusicSetting()
        {
            try
            {
                return DatabaseManager.Instance.Rooms.GetDocument<MusicSettings>("music");
            }
            catch
            {
                return null;
            }
        }
    }
}
