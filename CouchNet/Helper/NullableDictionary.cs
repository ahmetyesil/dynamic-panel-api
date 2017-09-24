using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet.Helper
{
    public class NullableDictionary<X, Y>
    {
        private readonly Dictionary<X, Y> dictionary = new Dictionary<X, Y>();

        public void Add(X key, Y value)
        {
            dictionary.Add(key, value);
        }

        public Y this[X key]
        {
            get
            {
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
                else
                {
                    return default(Y);
                }

            }
        }
    }
}
