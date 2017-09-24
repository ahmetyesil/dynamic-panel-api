using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouchNet.Helper
{
    public static class ObjectSerializer
    {
        private static readonly JsonSerializerSettings settings;

        static ObjectSerializer()
        {
            settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Include;
            settings.DateFormatString = "yyyy-MM-dd H:mm:ss";
            settings.ContractResolver = new CustomPropertyNamesContractResolver();
        }

        public static bool DefaultValueIfFailedSerialization = true;
        public static T Deserialize<T>(string json)
        {
            if (!DefaultValueIfFailedSerialization)
                return JsonConvert.DeserializeObject<T>(json, settings);
            try
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return default(T);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }
    }
}
