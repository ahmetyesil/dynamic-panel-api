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
    public class CouchRequest
    {
        private const string INVALID_USERNAME_OR_PASSWORD = "reason=Name or password is incorrect";
        private const string NOT_AUTHORIZED = "reason=You are not authorized to access this db.";
        private const int STREAM_BUFFER_SIZE = 4096;

        private readonly HttpWebRequest request;
        public CouchRequest(string uri) : this(uri, new Cookie(), null) { }

        public CouchRequest(string uri, Cookie authCookie, string eTag)
        {
            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Clear(); //important
            if (!string.IsNullOrEmpty(eTag))
                request.Headers.Add("If-None-Match", eTag);
            request.Headers.Add("Accept-Charset", "utf-8");
            request.Headers.Add("Accept-Language", "en-us");
            request.Accept = "application/json";
            request.Referer = uri;
            request.ContentType = "application/json";
            request.KeepAlive = true;
            if (authCookie != null)
                request.Headers.Add("Cookie", "AuthSession=" + authCookie.Value);
        }

        public CouchRequest(string uri, string username, string password)
        {

            request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Clear();

            if (username != null)
            {
                string authValue = "Basic ";
                string userNAndPassword = username + ":" + password;

                string b64 = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(userNAndPassword));

                authValue = authValue + b64;

                request.Headers.Add("Authorization", authValue);
            }

            request.Headers.Add("Accept-Charset", "utf-8");
            request.Headers.Add("Accept-Language", "en-us");
            request.ContentType = "application/json";
            request.KeepAlive = true;
        }

        public CouchRequest Put()
        {
            request.Method = "PUT";
            return this;
        }

        public CouchRequest Get()
        {
            request.Method = "GET";
            return this;
        }

        public CouchRequest Post()
        {
            request.Method = "POST";
            return this;
        }

        public CouchRequest Delete()
        {
            request.Method = "DELETE";
            return this;
        }

        public CouchRequest Data(Stream data)
        {
            using (var body = request.GetRequestStream())
            {
                var buffer = new byte[STREAM_BUFFER_SIZE];
                var bytesRead = 0;
                while (0 != (bytesRead = data.Read(buffer, 0, buffer.Length)))
                {
                    body.Write(buffer, 0, bytesRead);
                }
            }
            return this;
        }

        public CouchRequest Data(string data)
        {
            using (var body = request.GetRequestStream())
            {
                var encodedData = Encoding.UTF8.GetBytes(data);
                body.Write(encodedData, 0, encodedData.Length);
            }
            return this;
        }

        public CouchRequest Data(byte[] attachment)
        {
            using (var body = request.GetRequestStream())
            {
                body.Write(attachment, 0, attachment.Length);
            }
            return this;
        }

        public CouchRequest Data(IBaseObject obj)
        {
            return Data(ObjectSerializer.Serialize(obj));
        }

        public CouchRequest ContentType(string contentType)
        {
            request.ContentType = contentType;
            return this;
        }

        public CouchRequest Form()
        {
            request.ContentType = "application/x-www-form-urlencoded";
            return this;
        }

        public CouchRequest Json()
        {
            request.ContentType = "application/json";
            return this;
        }

        public CouchRequest Timeout(int? timeoutMs)
        {
            if (timeoutMs.HasValue)
            {
                request.Timeout = timeoutMs.Value;
            }
            else
            {
                request.Timeout = (int)TimeSpan.FromHours(1).TotalMilliseconds;
            }
            return this;
        }

        public HttpWebRequest GetRequest()
        {
            return request;
        }

        public CouchResponse GetCouchResponse()
        {
            bool failedAuth = false;
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    string msg = "";
                    if (isAuthenticateOrAuthorized(response, ref msg) == false)
                    {
                        failedAuth = true;
                        throw new WebException(msg);
                    }

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        throw new CouchException(request, response, response.GetResponseString() + "\n" + request.RequestUri);
                    }

                    return new CouchResponse(response);
                }
            }
            catch (WebException webEx)
            {
                if (failedAuth == true)
                {
                    throw webEx;
                }
                var response = (HttpWebResponse)webEx.Response;
                if (response == null)
                    throw new HttpException("Request failed to receive a response", webEx);
                return new CouchResponse(response);
            }
            throw new HttpException("Request failed to receive a response");
        }

        public HttpWebResponse GetHttpResponse()
        {
            bool failedAuth = false;
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                string msg = "";
                if (isAuthenticateOrAuthorized(response, ref msg) == false)
                {
                    failedAuth = true;
                    throw new WebException(msg, new System.Exception(msg));
                }

                return response;
            }
            catch (WebException webEx)
            {
                if (failedAuth == true)
                {
                    throw;
                }
                var response = (HttpWebResponse)webEx.Response;
                if (response == null)
                    throw new HttpException("Request failed to receive a response", webEx);
                return response;
            }
            throw new HttpException("Request failed to receive a response");
        }

        private bool isAuthenticateOrAuthorized(HttpWebResponse response, ref string message)
        {
            string[] split = response.ResponseUri.Query.Split('&');

            if (split.Length > 0)
            {
                for (int i = 0; i < split.Length; i++)
                {
                    string temp = System.Web.HttpUtility.UrlDecode(split[i]);

                    if (temp.Contains(INVALID_USERNAME_OR_PASSWORD) == true)
                    {
                        message = "Invalid username or password";
                        return false;
                    }
                    else if (temp.Contains(NOT_AUTHORIZED) == true)
                    {
                        message = "Not Authorized to access database";
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
