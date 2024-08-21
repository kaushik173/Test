using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LALoDep.Core.Enums;
using LALoDep.Custom.Security;
using Microsoft.Practices.ServiceLocation;
using StructureMap.Attributes;

namespace LALoDep.Custom.Attributes
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {

        [SetterProperty]
        public UserManager UserManager { get; set; }
        private RouteValueDictionary RedirectRoute { get; set; }

        public bool IsPageMainMethod { get; set; }
        public LALoDep.Custom.Security.SecurityToken PageSecurityItemID { get; set; }
        public string CustomSecurityItemIds { get; set; }
        public bool ReturnJsonResponseOnRequestFaild { get; set; }
        public bool IsCasePage { get; set; }
        public bool IsCasePageOnlyCheck { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }
            if (httpContext.Request.Form["CustomSecureId"] != null)
            {
                CustomSecurityItemIds = httpContext.Request.Form["CustomSecureId"];
                IsPageMainMethod = false;
            }


            UserManager = DependencyResolver.Current.GetService<UserManager>();
            var userData = UserManager.UserExtended;
            if (userData.UserID == 0)
            {
                RedirectRoute = new RouteValueDictionary {
                    { "controller", "Home" },
                    { "action", "SessionTimeout" },
                };
                return false;
            }
            if ((IsCasePage || IsCasePageOnlyCheck) && userData.CaseID == 0)
            {
                RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "AccessDenied" },
                        };
                return false;
            }
            if (IsCasePageOnlyCheck)//Just check if Case is in session then allow to run request. and if Case in session then Skip other validation
                return true;

            var routeValues = httpContext.Request.RequestContext.RouteData.Values;
            if (routeValues != null && routeValues.ContainsKey("action") && routeValues.ContainsKey("controller"))
            {
                var actionRoute = string.Format("{0}/{1}", routeValues["controller"], routeValues["action"]);

                if (userData.InitialLogin == 1 && actionRoute != "Account/ChangePasswordRequired")
                {
                    RedirectRoute = new RouteValueDictionary {
                            { "controller", "Account" },
                            { "action", "ChangePasswordRequired" },
                        };
                    return false;

                }
                var status = UserManager.HasAccessTo(httpContext, IsPageMainMethod, (int)PageSecurityItemID, CustomSecurityItemIds);
                switch (status)
                {
                    case LoginStatus.AccessDenied:
                        RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "AccessDenied" },
                        };
                        return false;

                    case LoginStatus.TimedOut:
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] != null && System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] == "true")
                        {
                            return true;
                        }
                        RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "SessionTimeout" },
                        };
                        if (httpContext.Session != null)
                        {
                            httpContext.Session.Abandon();
                            httpContext.Session.RemoveAll();
                            httpContext.Session.Clear();
                        }
                        return false;

                    case LoginStatus.InvalidKey:
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] != null && System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] == "true")
                        {
                            return true;
                        }
                        RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "InvalidGuid" },
                        };
                        if (httpContext.Session != null)
                        {
                            httpContext.Session.Abandon();
                            httpContext.Session.RemoveAll();
                            httpContext.Session.Clear();
                        }
                        return false;
                }
            }

            return true;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {

                if (!ReturnJsonResponseOnRequestFaild)
                {
                    filterContext.Result = new RedirectToRouteResult(RedirectRoute);
                }
                else
                {

                    filterContext.Result = new JsonResult
                    {
                        Data = new { URL = RedirectRoute.First(o => o.Key == "controller").Value.ToString() + "/" + RedirectRoute.First(o => o.Key == "action").Value.ToString(), Status = "Fail" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };


                }
            }
        }
    }
    public class AuthenticationAuthorizeAttribute : AuthorizeAttribute
    {

        [SetterProperty]
        public UserManager UserManager { get; set; }
        private RouteValueDictionary RedirectRoute { get; set; }



        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                RedirectRoute = new RouteValueDictionary {
                    { "controller", "Home" },
                    { "action", "SessionTimeout" },
                };
                return false;
            }


            UserManager = DependencyResolver.Current.GetService<UserManager>();
            var userData = UserManager.UserExtended;
            if (userData.UserID == 0)
            {
                RedirectRoute = new RouteValueDictionary {
                    { "controller", "Home" },
                    { "action", "SessionTimeout" },
                };
                return false;
            }



            return true;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (RedirectRoute != null && filterContext.RequestContext.HttpContext.Request.QueryString["_uniquerequest"] != null)
            {
                filterContext.Result = new HttpStatusCodeResult(403, "User is not authenticated!");

            }
            if (filterContext.RequestContext.HttpContext.Request.QueryString["_uniquerequest"] == null)
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }

            }


        }
    }
}