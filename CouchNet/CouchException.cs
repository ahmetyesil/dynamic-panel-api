using CouchNet.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class CouchException : Exception
    {
        private readonly HttpWebRequest request;
        private readonly HttpWebResponse response;

        public CouchException(HttpWebRequest request, HttpWebResponse response, string mesg)
            : base(BuildExceptionMessage(mesg, request))
        {
            this.request = request;
            this.response = response;
        }

        public CouchException(HttpWebRequest request, CouchResponse response, string mesg)
            : base(BuildExceptionMessage(mesg, request))
        {
        }

        public HttpWebRequest Request { get { return request; } }
        public HttpWebResponse Response { get { return response; } }

        private static string BuildExceptionMessage(string msg, HttpWebRequest request)
        {
            string excpetionMsg = string.Format("{0} {1}", request.RequestUri, msg);
            return excpetionMsg;
        }
    }
}
