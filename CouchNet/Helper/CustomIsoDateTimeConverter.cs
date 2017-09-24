using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CouchNet.Helper
{
    public class CustomIsoDateTimeConverter : IsoDateTimeConverter
    {
        public CustomIsoDateTimeConverter(string dateTimeFormat)
        {
            DateTimeFormat = dateTimeFormat;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var output = (DateTime)base.ReadJson(reader, objectType, existingValue, serializer);
                if (output < new DateTime(1970, 1, 1, 0, 0, 0))
                {
                    output = new DateTime(1970, 1, 1, 0, 0, 0);
                }
                return output;
            }
            catch
            {
                return new DateTime(1970, 1, 1, 0, 0, 0);
            }
        }
    }
}
