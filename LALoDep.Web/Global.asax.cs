
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using System.IO;
using Bugsnag;
using System.Net;
using System.Web.Http;

namespace LALoDep
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //these lines must be first call as we are forcing app to US TLS1.2
            if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            //Bootstrapper.Configure();

            var license = new Aspose.Words.License();
            license.SetLicense("Aspose.Words.lic");
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            StructureMap.Web.Pipeline.HttpContextLifecycle.DisposeAndClearAll();
        }
        protected void Application_BeginRequest()
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();


            //if (System.Web.Configuration.WebConfigurationManager.AppSettings["SSLOn"] == "true")
            //{
            //    IsSecurePage(true);
            //}

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Bugsnag.AspNet.Client.Current.Notify(ex, error =>
            {

                error.Event.User = new Bugsnag.Payload.User
                {
                    Id = LALoDep.Custom.UserEnvironment.UserManager.UserExtended.UserID.ToString(),
                    Name = LALoDep.Custom.UserEnvironment.UserManager.UserExtended.FullName,

                };
                if (HttpContext.Current.Request.HttpMethod == "POST")
                {
                    var metadata = new Dictionary<string, string>();
                    foreach (var item in HttpContext.Current.Request.Form.AllKeys)
                    {
                        metadata.Add(item, HttpContext.Current.Request.Form[item]);
                    }

                    error.Event.Metadata.Add("Form Data", metadata);
                }


                error.Event.Severity = Severity.Error;


                if (LALoDep.Custom.UserEnvironment.UserManager.UserExtended.CaseID > 0)
                {

                    var metadata = new Dictionary<string, string>();



                    metadata.Add("CaseID", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.CaseID.ToString());
                    metadata.Add("Case Name", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.Client);

                    metadata.Add("Case #", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.PDAPDNumber);
                    metadata.Add("Appt. Date", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.ApptDate);
                    metadata.Add("Status", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.Status);
                    metadata.Add("Attorney", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.Attorney);
                    metadata.Add("Jcats #", LALoDep.Custom.UserEnvironment.UserManager.UserExtended.CaseJcatsNumber);

                    error.Event.Metadata.Add("Case Session Data", metadata);
                }


            });
            //   Response.Redirect("/Home/Error");


        }
        /*    public void IsSecurePage(bool isSecure)
            {
                var HostingEnvironment = System.Configuration.ConfigurationManager.AppSettings["HostingEnvironment"].ToString();

                switch (HostingEnvironment.ToLower())
                {
                    case "ec2lb":

                        if (isSecure)
                        {
                            if (HttpContext.Current.Request.Headers["X-FORWARDED-PROTO"].ToString().ToLower() != "https")
                            {
                                var port = HttpContext.Current.Request.Url.Port.ToString();
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http://", "https://").Replace(":" + port, ""));
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Request.Headers["X-FORWARDED-PROTO"].ToString().ToLower() == "https")
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("https://", "http://"));
                            }
                        }

                        break;

                    case "dev":

                        if (isSecure)
                        {
                            if (!HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("https"))
                            {
                                var port = HttpContext.Current.Request.Url.Port.ToString();

                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http://", "https://").Replace(":" + port, ""));
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Request.IsSecureConnection)
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("https://", "http://"));
                            }
                        }

                        break;

                    case "localhost":

                        if (isSecure)
                        {
                            if (!HttpContext.Current.Request.IsSecureConnection)
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http://", "https://"));
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Request.IsSecureConnection)
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("https://", "http://"));
                            }
                        }

                        break;

                    case "nossl":

                        //TODO: do nothing because we don't care about ssl for this environment
                        break;

                    default:

                        if (isSecure)
                        {
                            if (!HttpContext.Current.Request.IsSecureConnection)
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http://", "https://"));
                            }
                        }
                        else
                        {
                            if (HttpContext.Current.Request.IsSecureConnection)
                            {
                                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("https://", "http://"));
                            }
                        }

                        break;
                }
            }
          */
        protected void Application_AuthorizeRequest()
        {
            if (!Request.Url.LocalPath.ToLower().Contains("/api/") && !Request.Url.LocalPath.ToLower().Contains("/swagger"))
            {
                if (!this.Context.User.Identity.IsAuthenticated && Request.Url.LocalPath.ToLower() != "/mobile/account/login" && Request.Url.LocalPath.ToLower().Contains("/mobile"))
                {
                    Response.Redirect("~/Mobile/Account/Login");//"?ReturnUrl=" + Request.Url.AbsoluteUri);

                }
                else if (this.Context.User.Identity.IsAuthenticated && !Request.Url.LocalPath.ToLower().Contains("/resetpassword") && !Request.Url.LocalPath.ToLower().Contains("/forgotpassword") && !Request.Url.LocalPath.ToLower().Contains("/g/") && Request.QueryString["_uniquerequest"] == null && !Request.Url.LocalPath.ToLower().Contains("/assets/") && !Request.Url.LocalPath.ToLower().Contains("/scripts/") && !Request.Url.LocalPath.ToLower().Contains("/css/") && !Request.Url.LocalPath.ToLower().Contains("/content/") && !Request.Url.LocalPath.ToLower().Contains("/devicedetector/") && !Request.Url.LocalPath.ToLower().Contains("/fileupload/") && !Request.Url.LocalPath.ToLower().Contains("/autocomplete/"))
                {
                    if (Request.UrlReferrer != null && Request.QueryString["newSession"] == null)
                    {
                        Response.Redirect("~/g/" + GetRefrererGuid() + "/" + Request.Url.AbsolutePath + Request.Url.Query);//"?ReturnUrl=" + Request.Url.AbsoluteUri);

                    }
                    else Response.Redirect("~/g/" + Guid.NewGuid().ToString("N") + "/" + (Request.Url.AbsolutePath == "/" ? "Case/Search" : Request.Url.AbsolutePath + Request.Url.Query));//"?ReturnUrl=" + Request.Url.AbsoluteUri);
                }
            }

        }
        string GetRefrererGuid()
        {
            var fullUrl = Request.UrlReferrer.ToString();
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            if (questionMarkIndex != -1) // There is a QueryString
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
            }

            // Arranges
            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            // Extract the data    
            var values = routeData.Values;
            if (!routeData.Values.ContainsKey("guid") || routeData.Values["guid"].ToString() == "")
                routeData.Values["guid"] = Guid.NewGuid().ToString("N");

            return values["guid"].ToString();
        }

        void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            e.Dismiss();
            return;

            // don't log 
            var messages = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath("~/App_Data/ElmahErrorFilter.txt"));
            foreach (var message in messages)
            {
                if (!message.IsNullOrEmpty())
                {
                    if (e.Exception.Message.Contains(message))
                    {
                        e.Dismiss();

                    }
                }
            }

        }
        void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            e.Dismiss();
            return;
            // Don't email me about files not being found
            var messages = System.IO.File.ReadAllLines(HttpContext.Current.Server.MapPath("~/App_Data/ElmahErrorFilter.txt"));
            foreach (var message in messages)
            {
                if (!message.IsNullOrEmpty())
                {
                    if (e.Exception.Message.Contains(message))
                    {
                        e.Dismiss();

                    }
                }
            }
        }
    }
}
