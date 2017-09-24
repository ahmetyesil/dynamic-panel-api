using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet.Helper
{
    public abstract class CouchBase
    {
        protected readonly string username;
        protected readonly string password;
        protected string baseUri;
        private TTLDictionary<string, Cookie> cookiestore = new TTLDictionary<string, Cookie>();
        protected int? timeout;

        protected CouchBase() { }
        protected CouchBase(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public void SetTimeout(int timeoutMs)
        {
            timeout = timeoutMs;
        }

        protected CouchRequest GetRequest(string uri)
        {
            return GetRequest(uri, null);
        }

        protected CouchRequest GetRequest(string uri, string etag)
        {
            CouchRequest request = new CouchRequest(uri, username, password);
            if (timeout.HasValue)
            {
                request.Timeout(timeout.Value);
            }
            return request;
        }
    }
}
