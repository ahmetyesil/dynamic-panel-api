using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class CouchConfiguration
    {
        public CouchConfiguration() { }

        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }


        public static CouchConfiguration Default { get; set; }

        static CouchConfiguration()
        {
            Default = new CouchConfiguration();
        }

        public static void Initialize(string Host, int Port, string Username, string Password)
        {
            Default = new CouchConfiguration();
            Default.Host = Host;
            Default.Port = Port;
            Default.Username = Username;
            Default.Password = Password;
        }
    }
}
