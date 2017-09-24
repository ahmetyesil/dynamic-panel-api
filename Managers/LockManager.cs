using System.Collections.Generic;

namespace Managers
{
    public class LockManager : ManagerBase<LockManager>
    {
        public object SocketManagerConnectionLock = new object();
        public object SocketManagerUserHandlerLock = new object();

        public Dictionary<string, object> UserHandlerSubscribeLock = new Dictionary<string, object>();
        public Dictionary<string, object> RoomHandlerSubscribeLock = new Dictionary<string, object>();
        public Dictionary<string, object> RoomHandlerGameLock = new Dictionary<string, object>();
    }
}
