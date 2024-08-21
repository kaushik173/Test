using System.Text;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Serialization;
using LALoDep.Custom;
using LALoDep.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using LALoDep.Domain.Services;
using LALoDep.Core.NG_pd_login_spResult;

namespace LALoDep.Controllers
{
    public abstract partial class BaseController : Controller
    {
        protected BaseController(): base()
        {
        }

       

        protected override JsonResult Json(object data, string contentType,
            Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected JsonResult ErrorResult(string message)
        {
            return Json(new { Result = "ERROR", Message = message });
        }


        protected JsonResult SuccessResult()
        {
            return Json(new { Result = "OK" });
        }

        protected ActionResult RedirectToLocal(string returnUrl, string landingPage)
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl != "/")
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (!string.IsNullOrEmpty(landingPage))
                    return Redirect("/" + landingPage);
                else
                    return RedirectToAction("Search", "Case");
            }
        }

        public string LogOffHelper(int? pageID = 0)
        {
            var UtilityService = DependencyResolver.Current.GetService<UtilityService>();
            var returnUrl = Url.Action("Login", "Account");
            if (pageID == 1)
                returnUrl = Url.Action("Login", "Account");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;


            if (!string.IsNullOrEmpty(UserEnvironment.UserManager.UserExtended.AdminLoginName))
            {
                var model = new AccountLoginViewModel()
                {
                    Username = UserEnvironment.UserManager.UserExtended.AdminLoginName
                };
                var errorMessage = string.Empty;
                var landingPage = string.Empty;
                 NG_pd_login_spResult oLogin; 

              var authenticate = UserEnvironment.UserManager.Authenticate(model, out errorMessage, out landingPage, out oLogin);

                if (authenticate)
                {
                    authManager.SignIn(new AuthenticationProperties() { IsPersistent = false });
                    returnUrl = Url.Action("SearchJuvenile", "Classic");
                }
            }
            else
            {
                authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }
            return returnUrl;
        }
    }
}