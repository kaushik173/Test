using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Newtonsoft.Json;
namespace Jcats.CA.UI.Custom
{

    namespace JsonRequest
    {
        /// <summary>
        /// Create Http Request, using json, and read Http Response.
        /// </summary>
        public class Request
        {

            public Dictionary<string, string> Header { get; set; } 
            /// <summary>
            /// Url of http server wich request will be created to.
            /// </summary>
            public String URL { get; set; }
            public string SerializedObject { get; set; }
            /// <summary>
            /// HTTP Verb wich will be used. Eg. GET, POST, PUT, DELETE.
            /// </summary>
            public String Verb { get; set; }

            /// <summary>
            /// Request content, Json by default.
            /// </summary>
            public String Content
            {
                get { return "text/json"; }
            }

            /// <summary>
            /// User and Password for Basic Authentication
            /// </summary>
            public Credentials Credentials { get; set; }

            public HttpWebRequest HttpRequest { get; internal set; }
            public HttpWebResponse HttpResponse { get; internal set; }
            public CookieContainer CookieContainer = new CookieContainer();

            /// <summary>
            /// Constructor Overload that allows passing URL and the VERB to be used.
            /// </summary>
            /// <param name="url">URL which request will be created</param>
            /// <param name="verb">Http Verb that will be userd in this request</param>
            public Request(string url, string verb)
            {
                Header = new Dictionary<string, string>();
                URL = url;
                Verb = verb;
            }

            /// <summary>
            /// Default constructor overload without any paramter
            /// </summary>
            public Request()
            {
                Header=new Dictionary<string, string>();
                Verb = "GET";
            }

            public object Execute<TT>(string url, object obj, string verb)
            {
                if (url != null)
                    URL = url;

                if (verb != null)
                    Verb = verb;

                HttpRequest = CreateRequest();

                WriteStream(obj);

                try
                {
                    HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
                }
                catch (WebException error)
                {
                    HttpResponse = (HttpWebResponse)error.Response;
                    return ReadResponseFromError(error);
                }

                return JsonConvert.DeserializeObject<TT>(ReadResponse());
            }

            public object Execute<TT>(string url)
            {
                if (url != null)
                    URL = url;

                HttpRequest = CreateRequest();

                try
                {
                    HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
                }
                catch (WebException error)
                {
                    HttpResponse = (HttpWebResponse)error.Response;
                    return ReadResponseFromError(error);
                }

                return JsonConvert.DeserializeObject<TT>(ReadResponse());
            }

            public object Execute<TT>()
            {
                if (URL == null)
                    throw new ArgumentException("URL cannot be null.");

                HttpRequest = CreateRequest();

                try
                {
                    HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
                }
                catch (WebException error)
                {
                    HttpResponse = (HttpWebResponse)error.Response;
                    return ReadResponseFromError(error);
                }

                return JsonConvert.DeserializeObject<TT>(ReadResponse());
            }

            public object Execute(string url, object obj, string verb)
            {
                if (url != null)
                    URL = url;

                if (verb != null)
                    Verb = verb;

                HttpRequest = CreateRequest();

                WriteStream(obj);
                if (Header.Count > 0)
                {
                    foreach (var item in Header)
                    {
                        HttpRequest.Headers.Add(item.Key,item.Value);
           
                    }
                
                    
                }
                try
                {
                    HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
                }
                catch (WebException error)
                {
                    HttpResponse = (HttpWebResponse)error.Response;
                    return ReadResponseFromError(error);
                }

                return ReadResponse();
            }

            public object Execute(string url)
            {
                if (url != null)
                    URL = url;

                HttpRequest = CreateRequest();

                try
                {
                    HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
                }
                catch (WebException error)
                {
                    HttpResponse = (HttpWebResponse)error.Response;
                    return ReadResponseFromError(error);
                }

                return ReadResponse();
            }

            public object Execute()
            {
                if (URL == null)
                    throw new ArgumentException("URL cannot be null.");

                HttpRequest = CreateRequest();

                try
                {
                    HttpResponse = (HttpWebResponse)HttpRequest.GetResponse();
                }
                catch (WebException error)
                {
                    HttpResponse = (HttpWebResponse)error.Response;
                    return ReadResponseFromError(error);
                }

                return ReadResponse();
            }

            internal HttpWebRequest CreateRequest()
            {
                ServicePointManager.ServerCertificateValidationCallback =
  new RemoteCertificateValidationCallback(delegate(object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                });
             //   ASCIIEncoding encoder = new ASCIIEncoding();
             //   byte[] data = encoder.GetBytes(SerializedObject); // a json object, or xml, whatever...

                var basicRequest = (HttpWebRequest)WebRequest.Create(URL);
                basicRequest.ContentType = Content;
                basicRequest.Method = Verb;
             //   basicRequest.CookieContainer = CookieContainer;
               // basicRequest.GetRequestStream().Write(data, 0, data.Length);
                if (Credentials != null)
                    basicRequest.Headers.Add("Authorization", "Basic" + " " + EncodeCredentials(Credentials));

                return basicRequest;
            }

            internal void WriteStream(object obj)
            {
                if (obj != null)
                {
                    using (var streamWriter = new StreamWriter(HttpRequest.GetRequestStream()))
                    {
                        if (obj is string)
                            streamWriter.Write(obj);
                        else
                            streamWriter.Write(JsonConvert.SerializeObject(obj));
                    }
                }
            }

            internal String ReadResponse()
            {
                if (HttpResponse != null)
                    using (var streamReader = new StreamReader(HttpResponse.GetResponseStream()))
                        return streamReader.ReadToEnd();

                return string.Empty;
            }

            internal String ReadResponseFromError(WebException error)
            {
                using (var streamReader = new StreamReader(error.Response.GetResponseStream()))
                    return streamReader.ReadToEnd();
            }

            internal static string EncodeCredentials(Credentials credentials)
            {
                var strCredentials = string.Format("{0}:{1}", credentials.UserName, credentials.Password);
                var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(strCredentials));

                return encodedCredentials;
            }

        }


        /// <summary>
        /// This class support JsonRequest on sending credentials for Basic Authentication on HTTP server
        /// </summary>
        public class Credentials
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    }
}

