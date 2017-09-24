using CouchNet.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CouchNet
{
    public class CouchDatabase : CouchBase
    {
        private readonly string databaseBaseUri;
        private string defaultDesignDoc = null;
        public CouchDatabase(string baseUri, string databaseName, string username, string password)
            : base(username, password)
        {
            this.baseUri = baseUri;
            this.databaseBaseUri = baseUri + databaseName;
        }

        public CouchDatabase(Uri uri)
        {
            this.baseUri = uri.AbsoluteUri.Replace(uri.AbsolutePath, "");
            this.databaseBaseUri = uri.AbsoluteUri;
        }

        public CouchDatabase(CouchConfiguration config, string databaseName)
            : base(config.Username, config.Password)
        {
            this.baseUri = "http://" + config.Host + ":" + config.Port + "/";
            this.databaseBaseUri = this.baseUri + databaseName;
        }

        public CouchDatabase(string databaseName) : this(CouchConfiguration.Default, databaseName) { }

        public CouchResponseObject CreateDocument(string id, string jsonForDocument)
        {
            var jobj = JObject.Parse(jsonForDocument);
            if (jobj.Value<object>("_rev") == null)
                jobj.Remove("_rev");
            var resp = GetRequest(databaseBaseUri + "/" + id).Put().Json().Data(jobj.ToString(Formatting.None)).GetCouchResponse();
            return resp.GetJObject();
        }

        public CouchResponseObject CreateDocument(IBaseObject doc)
        {
            var serialized = ObjectSerializer.Serialize(doc);
            if (doc.Id != null)
            {
                return CreateDocument(doc.Id, serialized);
            }
            return CreateDocument(serialized);
        }

        public T CreateDocument<T>(T doc) where T : IBaseObject
        {
            var serialized = ObjectSerializer.Serialize(doc);
            serialized = Regex.Replace(serialized, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            serialized = serialized.Replace(",\"_id\":null", "");
            serialized = serialized.Replace(",\"_rev\":null", "");
            var jobj = GetRequest(databaseBaseUri + "/").Post().Json().Data(serialized).GetCouchResponse().GetJObject();
            doc.Id = jobj["id"].ToObject<string>();
            doc.Rev = jobj["rev"].ToObject<string>();
            return (T)doc;
        }

        public CouchResponseObject CreateDocument(string jsonForDocument)
        {
            // var json = JObject.Parse(jsonForDocument);
            var jobj = GetRequest(databaseBaseUri + "/").Post().Json().Data(jsonForDocument).GetCouchResponse().GetJObject();
            return jobj;
        }

        public CouchResponseObject DeleteDocument(string id, string rev)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(rev))
                throw new Exception("Both id and rev must have a value that is not empty");
            return GetRequest(databaseBaseUri + "/" + id + "?rev=" + rev).Delete().Form().GetCouchResponse().GetJObject();
        }

        public string GetDocumentSource(string id, bool attachments)
        {
            var resp = GetRequest(String.Format("{0}/{1}{2}", databaseBaseUri, id, attachments ? "?attachments=true" : string.Empty)).Get().Json().GetCouchResponse();
            if (resp.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            return resp.ResponseString;
        }

        public T GetDocument<T>(string id, bool attachments)
        {
            var data = GetDocumentSource(id, attachments);
            if (data == null)
            {
                return default(T);
            }
            return ObjectSerializer.Deserialize<T>(data);
        }

        public string GetDocumentSource(string id)
        {
            return GetDocumentSource(id, false);
        }

        public T GetDocument<T>(string id)
        {
            return GetDocument<T>(id, false);
        }

        public string GetDocumentSource(Guid id, bool attachments)
        {
            return GetDocumentSource(id.ToString(), attachments);
        }

        public T GetDocument<T>(Guid id, bool attachments)
        {
            return GetDocument<T>(id.ToString(), attachments);
        }

        public string GetDocumentSource(Guid id)
        {
            return GetDocumentSource(id, false);
        }

        public T GetDocument<T>(Guid id)
        {
            return GetDocument<T>(id, false);
        }

        public Document GetDocument(string id, bool attachments)
        {
            var resp = GetRequest(String.Format("{0}/{1}{2}", databaseBaseUri, id, attachments ? "?attachments=true" : string.Empty)).Get().Json().GetCouchResponse();
            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;
            return resp.GetCouchDocument();
        }

        public Document GetDocument(string id)
        {
            return GetDocument(id, false);
        }

        public ViewResult GetDocuments(Keys keyList)
        {
            return GetDocuments<object>(keyList);
        }

        public ViewResult<T> GetDocuments<T>(Keys keyList)
        {
            ViewOptions viewOptions = new ViewOptions
            {
                IncludeDocs = true,
                Keys = keyList.Values.Select(x => new KeyOptions(x)).ToArray()
            };
            return GetDocuments<T>(viewOptions);
        }

        public ViewResult GetDocuments(ViewOptions viewOptions)
        {
            return GetDocuments<object>(viewOptions);
        }

        public ViewResult<T> GetDocuments<T>(ViewOptions viewOptions)
        {
            CouchResponse resp = GetRequest(viewOptions, databaseBaseUri + "/_all_docs").GetCouchResponse();
            if (resp == null)
                return null;
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;
            ViewResult<T> vw = new ViewResult<T>(resp, null);
            return vw;
        }

        public BulkDocumentResponses SaveDocuments(Documents docs, bool all_or_nothing)
        {
            string uri = databaseBaseUri + "/_bulk_docs";
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(docs);
            if (all_or_nothing == true)
            {
                uri = uri + "?all_or_nothing=true";
            }

            CouchResponse resp = GetRequest(uri).Post().Json().Data(data).GetCouchResponse();
            if (resp == null)
            {
                throw new System.Exception("Response returned null.");
            }

            if (resp.StatusCode != HttpStatusCode.Created)
            {
                throw new System.Exception("Response returned with a HTTP status code of " + resp.StatusCode + " - " + resp.StatusDescription);
            }

            string x = resp.ResponseString;
            BulkDocumentResponses bulk = Newtonsoft.Json.JsonConvert.DeserializeObject<BulkDocumentResponses>(x);
            return bulk;
        }

        public BulkDocumentResponses SaveDocuments<T>(IEnumerable<T> models, bool all_or_nothing = false) where T : BaseObject
        {
            Models docs = new Models();
            docs.Values.AddRange(models);

            string uri = databaseBaseUri + "/_bulk_docs";
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(docs);
            if (all_or_nothing == true)
            {
                uri = uri + "?all_or_nothing=true";
            }

            CouchResponse resp = GetRequest(uri).Post().Json().Data(data).GetCouchResponse();
            if (resp == null)
            {
                throw new System.Exception("Response returned null.");
            }

            if (resp.StatusCode != HttpStatusCode.Created)
            {
                throw new System.Exception("Response returned with a HTTP status code of " + resp.StatusCode + " - " + resp.StatusDescription);
            }

            string x = resp.ResponseString;
            BulkDocumentResponses bulk = Newtonsoft.Json.JsonConvert.DeserializeObject<BulkDocumentResponses>(x);
            return bulk;
        }

        public CouchResponseObject AddAttachment(string id, byte[] attachment, string filename, string contentType)
        {
            var doc = GetDocument(id);
            return AddAttachment(id, doc.Rev, attachment, filename, contentType);
        }

        public CouchResponseObject AddAttachment(string id, string rev, byte[] attachment, string filename, string contentType)
        {
            return GetRequest(string.Format("{0}/{1}/{2}?rev={3}", databaseBaseUri, id, filename, rev)).Put().ContentType(contentType).Data(attachment).GetCouchResponse().GetJObject();
        }

        public CouchResponseObject AddAttachment(string id, Stream attachmentStream, string filename, string contentType)
        {
            var doc = GetDocument(id);
            return AddAttachment(id, doc.Rev, attachmentStream, filename, contentType);
        }

        public CouchResponseObject AddAttachment(string id, string rev, Stream attachmentStream, string filename, string contentType)
        {
            return GetRequest(string.Format("{0}/{1}/{2}?rev={3}", databaseBaseUri, id, filename, rev)).Put().ContentType(contentType).Data(attachmentStream).GetCouchResponse().GetJObject();
        }

        public Stream GetAttachmentStream(Document doc, string attachmentName)
        {
            return GetAttachmentStream(doc.Id, doc.Rev, attachmentName);
        }

        public Stream GetAttachmentStream(string docId, string rev, string attachmentName)
        {
            return GetRequest(string.Format("{0}/{1}/{2}", databaseBaseUri, docId, HttpUtility.UrlEncode(attachmentName))).Get().GetHttpResponse().GetResponseStream();
        }

        public Stream GetAttachmentStream(string docId, string attachmentName)
        {
            var doc = GetDocument(docId);
            if (doc == null) return null;
            return GetAttachmentStream(docId, doc.Rev, attachmentName);
        }

        public CouchResponseObject DeleteAttachment(string id, string rev, string attachmentName)
        {
            return GetRequest(string.Format("{0}/{1}/{2}?rev={3}", databaseBaseUri, id, attachmentName, rev)).Json().Delete().GetCouchResponse().GetJObject();
        }

        public CouchResponseObject DeleteAttachment(string id, string attachmentName)
        {
            var doc = GetDocument(id);
            return DeleteAttachment(doc.Id, doc.Rev, attachmentName);
        }

        public CouchResponseObject SaveDocument(IBaseObject document)
        {
            if (document.Rev == null)
                return CreateDocument(document);

            var resp = GetRequest(string.Format("{0}/{1}?rev={2}", databaseBaseUri, document.Id, document.Rev)).Put().Form().Data(document).GetCouchResponse();
            return resp.GetJObject();
        }

        public T SaveDocument<T>(T document) where T : IBaseObject
        {
            if (document.Rev == null)
                return CreateDocument<T>(document);
            var resp = GetRequest(string.Format("{0}/{1}?rev={2}", databaseBaseUri, document.Id, document.Rev)).Put().Form().Data(document).GetCouchResponse();
            var jobj = resp.GetJObject();
            document.Id = jobj["id"].ToObject<string>();
            document.Rev = jobj["rev"].ToObject<string>();
            return (T)document;
        }

        public ViewResult<T> View<T>(string viewName, string designDoc)
        {
            return View<T>(viewName, null, designDoc);
        }

        public ViewResult<T> View<T>(string viewName)
        {
            ThrowDesignDocException();
            return View<T>(viewName, defaultDesignDoc);
        }
        public ViewResult View(string viewName)
        {
            ThrowDesignDocException();
            return View(viewName, new ViewOptions());
        }

        public JObject ViewCleanup()
        {
            return CheckAccepted(GetRequest(databaseBaseUri + "/_view_cleanup").Post().Json().GetCouchResponse());
        }

        public JObject Compact()
        {
            return CheckAccepted(GetRequest(databaseBaseUri + "/_compact").Post().Json().GetCouchResponse());
        }

        public JObject Compact(string designDoc)
        {
            return CheckAccepted(GetRequest(databaseBaseUri + "/_compact/" + designDoc).Post().Json().GetCouchResponse());
        }

        private static JObject CheckAccepted(CouchResponse resp)
        {
            if (resp == null)
            {
                throw new System.Exception("Response returned null.");
            }

            if (resp.StatusCode != HttpStatusCode.Accepted)
            {
                throw new System.Exception(string.Format("Response return with a HTTP Code of {0} - {1}", resp.StatusCode, resp.StatusDescription));
            }

            return resp.GetJObject();
        }


        public string Show(string showName, string docId)
        {
            ThrowDesignDocException();
            return Show(showName, docId, defaultDesignDoc);
        }

        private void ThrowDesignDocException()
        {
            if (string.IsNullOrEmpty(defaultDesignDoc))
                throw new Exception("You must use SetDefaultDesignDoc prior to using this signature.  Otherwise explicitly specify the design doc in the other overloads.");
        }

        public string Show(string showName, string docId, string designDoc)
        {
            var uri = string.Format("{0}/_design/{1}/_show/{2}/{3}", databaseBaseUri, designDoc, showName, docId);
            var req = GetRequest(uri);
            return req.GetCouchResponse().ResponseString;
        }
        public ListResult List(string listName, string viewName, ViewOptions options, string designDoc)
        {
            var uri = string.Format("{0}/_design/{1}/_list/{2}/{3}{4}", databaseBaseUri, designDoc, listName, viewName, options.ToString());
            var req = GetRequest(uri);
            return new ListResult(req.GetRequest(), req.GetCouchResponse());
        }

        public ListResult List(string listName, string viewName, ViewOptions options)
        {
            ThrowDesignDocException();
            return List(listName, viewName, options, defaultDesignDoc);
        }

        public void SetDefaultDesignDoc(string designDoc)
        {
            this.defaultDesignDoc = designDoc;
        }

        private ViewResult<T> ProcessGenericResults<T>(string uri, ViewOptions options)
        {
            CouchRequest req = GetRequest(options, uri);
            CouchResponse resp = req.GetCouchResponse();

            bool includeDocs = false;
            if (options != null)
            {
                includeDocs = options.IncludeDocs ?? false;
            }

            return new ViewResult<T>(resp, req.GetRequest(), includeDocs);
        }

        public ViewResult<T> View<T>(string viewName, ViewOptions options, string designDoc)
        {
            var uri = string.Format("{0}/_design/{1}/_view/{2}", databaseBaseUri, designDoc, viewName);
            return ProcessGenericResults<T>(uri, options);
        }

        public ViewResult<T> View<T>(string viewName, ViewOptions options)
        {
            ThrowDesignDocException();
            return View<T>(viewName, options, defaultDesignDoc);
        }

        public ViewResult View(string viewName, ViewOptions options, string designDoc)
        {
            var uri = string.Format("{0}/_design/{1}/_view/{2}", databaseBaseUri, designDoc, viewName);
            return ProcessResults(uri, options);
        }

        public ViewResult View(string viewName, ViewOptions options)
        {
            ThrowDesignDocException();
            return View(viewName, options, this.defaultDesignDoc);
        }
        private ViewResult ProcessResults(string uri, ViewOptions options)
        {
            CouchRequest req = GetRequest(options, uri);
            CouchResponse resp = req.GetCouchResponse();
            return new ViewResult(resp, req.GetRequest());
        }

        private CouchRequest GetRequest(ViewOptions options, string uri)
        {
            if (options != null)
                uri += options.ToString();
            CouchRequest request = GetRequest(uri, options == null ? null : options.Etag).Get().Json();
            if (options != null && options.isAtKeysSizeLimit)
            {
                string keys = "{\"keys\": [" + String.Join(",", options.Keys.Select(k => k.ToRawString()).ToArray()) + "]}";
                request.Post().Data(keys);
            }
            return request;
        }


        public ViewResult GetAllDocuments()
        {
            var uri = databaseBaseUri + "/_all_docs";
            return ProcessResults(uri, null);
        }

        public ViewResult GetAllDocuments(ViewOptions options)
        {
            var uri = databaseBaseUri + "/_all_docs";
            return ProcessResults(uri, options);
        }
    }
}
