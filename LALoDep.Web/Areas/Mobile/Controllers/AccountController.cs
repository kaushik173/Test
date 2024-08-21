using System;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Utility;
using LALoDep.Core.NG_pd_login_spResult;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Domain.Services;
using LALoDep.Models;
using Microsoft.Owin.Security;

namespace LALoDep.Areas.Mobile.Controllers
{
    [AuthenticationAuthorize]
    public partial class AccountController : Controller
    {
        private IUtilityService UtilityService;
         private UserManager UserManager;
         private IAuthenticationManager AuthenticationManager
         {
             get
             {
                 return HttpContext.GetOwinContext().Authentication;
             }
         }
        public AccountController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }


        [AllowAnonymous]
        public virtual ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (Request.Cookies["RememberMe"] != null)
            {
                var rememberCookie = Request.Cookies["RememberMe"];
                ViewBag.RememberMeUsername = Utility.Decrypt(rememberCookie.Value);

            }
            if (Request.QueryString["message"] != null)
            {
                ModelState.AddModelError("", Request.QueryString["message"]);
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Login(AccountLoginViewModel model, string ReturnUrl)
        {
            AuthenticationManager.SignOut("ApplicationCookie");
            var errorMessage = String.Empty;
            var landingPage = String.Empty; NG_pd_login_spResult oLogin;
            if (ModelState.IsValid)
            {
                if (UserManager.Authenticate(model, out errorMessage, out landingPage, out oLogin))
                {
                    var identity = UserManager.CreateIdentity(model);

                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

                    #region Remember Me Cookie

                    if (Request.Form["rememberme"] != null)
                    {
                        var rememberCookie = new HttpCookie("RememberMe")
                        {
                            Value = Utility.Encrypt(model.Username),
                            Expires = DateTime.Now.AddYears(1)
                        };
                        Response.Cookies.Add(rememberCookie);

                    }
                    else if (Request.Cookies["RememberMe"] != null)
                    {
                        var rememberCookie = new HttpCookie("RememberMe") { Expires = DateTime.Now.AddDays(-1d) };
                        Response.Cookies.Add(rememberCookie);

                    }

                    #endregion

                    return RedirectToAction("Index", "MyCalendar", new { Area = "Mobile" });
                    //  return RedirectToLocal(ReturnUrl, landingPage);

                }
            }


            if (Request.Cookies["RememberMe"] != null)
            {
                var rememberCookie = Request.Cookies["RememberMe"];
                ViewBag.RememberMeUsername = Utility.Decrypt(rememberCookie.Value);

            }
            ModelState.AddModelError("", errorMessage);
            return View(model);
        }

        public virtual ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            Session.RemoveAll(); Session.Abandon();
            if (!string.IsNullOrEmpty(UserManager.UserExtended.AdminLoginName))
            {
                var message = "";
                var landingPage = "";
                var model = new AccountLoginViewModel()
                {
                    AvailHeight = 0,
                    AvailWidth = 0,
                    Height = 0,
                    Width = 0,
                    Username = UserManager.UserExtended.AdminLoginName
                };
                authManager.SignOut("ApplicationCookie");
                var isAuthenticated = UserManager.LoginAs(model, out message, out landingPage);
                if (isAuthenticated)
                {
                    var identity = UserManager.CreateIdentity(model);
                    authManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                    return RedirectToAction("Search", "Users");
                }
            }
            else
            {
                authManager.SignOut("ApplicationCookie");
            }
            return RedirectToAction("Login", "Account", new { area = "Mobile" });
        }

        private ActionResult RedirectToLocal(string returnUrl, string landingPage)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (!string.IsNullOrEmpty(landingPage))
                {
                    if (landingPage == "Account/ChangePasswordRequired")
                        return RedirectToAction("ChangePassword", "Account");
                    else
                        return Redirect("/" + landingPage);
                }
                else
                    return RedirectToAction("Index", "MyCalendar");
            }
        }


    }
}