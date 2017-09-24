using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet.Helper
{
    public class CustomPropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);
            if (objectType == typeof(DateTime))
            {
                contract.Converter = new CustomIsoDateTimeConverter("yyyy-MM-dd HH:mm:ss");
            }

            return contract;
        }
    }
}
