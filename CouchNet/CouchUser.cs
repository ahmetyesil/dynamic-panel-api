using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet
{
    public class CouchUser : Document
    {
        public CouchUser(JObject jobj) : base(jobj) { }

        public IEnumerable<string> Roles
        {
            get
            {
                if (!this["roles"].HasValues)
                {
                    yield return null;
                }
                foreach (var role in this["roles"].Values())
                {
                    yield return role.Value<string>();
                }
            }
        }
    }
}
