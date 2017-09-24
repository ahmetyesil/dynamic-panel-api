using System;

namespace SocketServer.Handlers
{
    public class BaseHandler : IDisposable
    {
        public virtual void Dispose()
        {
        }
    }
}
