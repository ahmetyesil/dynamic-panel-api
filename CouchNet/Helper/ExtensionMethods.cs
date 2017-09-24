using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CouchNet.Helper
{
    public static class ExtensionMethods
    {
        public static Document GetCouchDocument(this HttpWebResponse response)
        {
            var jobj = JObject.Parse(response.GetResponseString());
            return new Document(jobj);
        }

        public static string GetResponseString(this HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    if (string.IsNullOrEmpty(result)) return null;
                    return result;
                }
            }
        }

        public static CouchResponseObject GetJObject(this HttpWebResponse response)
        {
            var resp = new CouchResponseObject(JObject.Parse(response.GetResponseString()));
            resp.StatusCode = (int)response.StatusCode;
            return resp;
        }


        public static ViewOptions GetOptions(this PageableModel model)
        {
            var options = new ViewOptions();
            options.Descending = model.Descending;
            options.StartKey.Add(model.StartKey);
            options.Skip = model.Skip;
            options.Stale = model.Stale;
            options.StartKeyDocId = model.StartKeyDocId;
            options.EndKeyDocId = model.EndKeyDocId;
            options.Limit = model.Limit.HasValue ? model.Limit : 10;
            return options;
        }

        public static void UpdatePaging(this PageableModel model, ViewOptions options, ViewResult result)
        {
            int count = result.Rows.Count();
            var limit = options.Limit.HasValue ? options.Limit.Value : 10;
            model.Limit = limit;
            int rowsMinusOffset = (result.TotalRows - result.OffSet);
            model.ShowPrev = result.OffSet != 0 && !(model.Descending && (rowsMinusOffset <= count));
            model.ShowNext = (result.TotalRows > options.Limit + result.OffSet) || options.Descending.GetValueOrDefault();
            string skipPrev = result.OffSet < limit ? "" : "&skip=1";
            string skipNext = rowsMinusOffset < limit ? "" : "&skip=1";
            JToken lastRow;
            JToken firstRow;
            if (options.Descending.HasValue && options.Descending.Value)
            {
                lastRow = result.Rows.FirstOrDefault();
                firstRow = result.Rows.LastOrDefault();
                model.StartIndex = (rowsMinusOffset - limit) < 1 ? 1 : (rowsMinusOffset - limit + 1);
            }
            else
            {
                lastRow = result.Rows.LastOrDefault();
                firstRow = result.Rows.FirstOrDefault();
                model.StartIndex = result.OffSet + 1;
            }

            var startIndexPlusCount = model.StartIndex + count - 1;
            model.EndIndex = result.TotalRows == 0 ? 0 : startIndexPlusCount;
            if (count == 0) model.EndIndex = model.StartIndex = 0;

            model.TotalRows = result.TotalRows;
            string prevStartKey = firstRow != null ? "&startkey=" + HttpUtility.UrlEncode(firstRow.Value<string>("key")) + "&StartKeyDocId=" + firstRow.Value<string>("id") : "";
            string nextStartKey = lastRow != null ? "&startkey=" + HttpUtility.UrlEncode(lastRow.Value<string>("key")) + "&StartKeyDocId=" + lastRow.Value<string>("id") : "";
            model.NextUrlParameters = "?limit=" + model.Limit + nextStartKey + skipNext;
            model.PrevUrlParameters = "?limit=" + model.Limit + prevStartKey + skipPrev + "&descending=true";
        }
    }
}
