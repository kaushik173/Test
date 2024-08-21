using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LALoDep.Core.Enums;
using LALoDep.Custom;
using Microsoft.Practices.ServiceLocation;
using StructureMap.Attributes;

namespace Jcats.SD.UI.Areas.Mobile.Custom.Attribute
{
    public class MobileClaimsAuthorizeAttribute : AuthorizeAttribute
    {

        [SetterProperty]
        public UserManager UserManager { get; set; }
        private RouteValueDictionary RedirectRoute { get; set; }
        public bool OnlySystemAdmin { get; set; }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User as ClaimsPrincipal;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            UserManager = UserEnvironment.UserManager;
            var userData = UserManager.UserExtended;
            if (OnlySystemAdmin && userData.SystemAdminFlag != 1)
            {
                return false;
            }
            var routeValues = httpContext.Request.RequestContext.RouteData.Values;
            if (routeValues != null && routeValues.ContainsKey("action") && routeValues.ContainsKey("controller"))
            {

                var status = UserManager.MobileHasAccessTo(httpContext);
                switch (status)
                {
                    case LogonStatusCode.AccessDenied:
                        RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "AccessDenied" },
                        };
                        return false;

                    case LogonStatusCode.SessionTimeout:
                        RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "SessionTimeout" },
                        };
                        return false;

                    case LogonStatusCode.InvalidGuid:
                        if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] != null && System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] == "true")
                        {
                            return true;
                        }
                        RedirectRoute = new RouteValueDictionary {
                            { "controller", "Home" },
                            { "action", "InvalidGuid" },
                        };
                        return false;

                    default: break;
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
                filterContext.Result = new RedirectToRouteResult(RedirectRoute);
            }
        }
    }
}