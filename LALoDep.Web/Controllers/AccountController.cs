using System;
using System.Web;
using System.Web.Mvc;
using Jcats.CA.UI.Custom;
using LALoDep.Core.NG_pd_login_spResult;
using LALoDep.Core.NG_sp.com_LogReset;
using Microsoft.Owin.Security;
using LALoDep.Custom;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Utility;
using LALoDep.Models;
using LALoDep.Models.Administration;
using System.Linq;
using LALoDep.Core.NG_sp.com_Login;
using LALoDep.Domain.com_Jcats;
using System.Security.Claims;
using LALoDep.Custom.Attributes;
using LALoDep.Core.Custom.Extensions;
using System.Web.Security;

namespace LALoDep.Controllers
{

    public partial class AccountController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public AccountController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public virtual ActionResult Login(string ReturnUrl)
        {
            //Session.RemoveAll(); Session.Abandon();
            //AuthenticationManager.SignOut("ApplicationCookie");
            //HttpCookie authCookie = new HttpCookie(".AspNet.ApplicationCookie", "");
            //authCookie.Expires = DateTime.Now.AddYears(-1);
            //Response.Cookies.Add(authCookie);

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

          var maintenanceMessage =  UtilityService.ExecStoredProcedureScalar("NG_pd_GetMaintenanceMessage_sp", null);
            if (maintenanceMessage != null)
            {
                ViewBag.MaintenanceMessage = maintenanceMessage;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
      //  [ValidateAntiForgeryToken]
        public virtual ActionResult Login(AccountLoginViewModel model, string ReturnUrl)
        {

            Session.RemoveAll(); Session.Abandon();
            AuthenticationManager.SignOut("ApplicationCookie");
            HttpCookie authCookie = new HttpCookie(".AspNet.ApplicationCookie", "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(authCookie);
            var errorMessage = String.Empty;
            var landingPage = String.Empty;
            NG_pd_login_spResult oLogin;
            if (ModelState.IsValid)
            {
                if (UserManager.Authenticate(model, out errorMessage, out landingPage, out oLogin))
                {
                    var identity = UserManager.CreateIdentity(model, initialLogin: (landingPage == "Account/ChangePasswordRequired" ? 1 : 0), oLogin: oLogin);

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
                    UtilityFunctions.DeleteFilesFromDocumentDownloadFolderPath();
                    #endregion

                    return RedirectToLocal(ReturnUrl, landingPage);
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
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var action = filterContext.RequestContext.RouteData.Values["action"] as string;
            var controller = filterContext.RequestContext.RouteData.Values["controller"] as string;

            if ((filterContext.Exception is HttpAntiForgeryException) &&
                action == "Login" &&
                controller == "Account")
            {


                filterContext.ExceptionHandled = true;

                // redirect/show error/whatever?
                filterContext.Result = new RedirectResult("/Home/AntiForgery");
            }
        }
        public virtual ActionResult LogOff()
        {
            UtilityFunctions.DeleteFilesFromDocumentDownloadFolderPath();
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
                AuthenticationManager.SignOut("ApplicationCookie");
                authManager.SignOut("ApplicationCookie");
                HttpCookie authCookie = new HttpCookie(".AspNet.ApplicationCookie", "");
                authCookie.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(authCookie);
            }
            return RedirectToAction("Login", "Account");
        }

        private ActionResult RedirectToLocal(string returnUrl, string landingPage)
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


        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
        

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ForgotPassword(AccountForgotPasswordViewModel model)
        {
            var errorMessage = String.Empty;
            if (ModelState.IsValid)
            {
                var code = UserManager.GeneratePasswordResetToken(model, DateTime.Now.AddHours(24), out errorMessage);
                
                    if (!string.IsNullOrEmpty(code))
                {
                    var callbackUrl = string.Format("{0}://{1}/Account/ResetPassword?code={2}", (Request.IsSecureConnection ? "https" : "http"), Request.Url.Host, code);

                   
                    var emailRecipient = new EmailRecipient
                    {
                        ToAddress = model.EmailAddress
                    };
                    emailRecipient.CustomData.Add("href", callbackUrl);
                    new MailController().ResetPassword(emailRecipient).Deliver();
                    ViewBag.EmailSent = true;

                }
            }

            ModelState.AddModelError("", errorMessage);
            return View(model);



        }


        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            Guid gcode;
            const string errorMessage = "Reset Password link expired";
            if (Guid.TryParse(code, out gcode))
            {
                NG_com_LogResetPasswordValidateCode_spResult result;
                if (UserManager.ValidatePasswordCode(gcode, out result))
                {

                    var model = new AccountResetPasswordViewModel
                    {
                        Username = result.Username,
                        Code = gcode
                    };
                    return View(model);
                }
            }
            return RedirectToAction("Login", new { message = errorMessage });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ResetPassword(AccountResetPasswordViewModel model)
        {

            var errorMessage = "";
            if (ModelState.IsValid)
            {
                NG_com_LogResetPasswordValidateCode_spResult result;
                if (UserManager.ValidatePasswordCode(model.Code, out result))
                {
                     var db = new DbManager();
                    db.AddInParam("UserName", model.Username);
                    var userid = db.ExecuteScalar("select JcatsUserID from jcatsuser where jcatsuserloginname = @UserName", System.Data.CommandType.Text);


                    var changePassword = UtilityService.ExecStoredProcedureWithResults<NG_pd_ChangePassword_spResult>("NG_pd_ChangePassword_sp", new NG_pd_ChangePassword_spParams()
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        ChangeUserID = userid.ToInt(),
                        CurrentPassword = "",
                        NewPassword = model.Password,
                        ResetFlag = 0,
                        UserID = UserManager.UserExtended.UserID

                    }).FirstOrDefault();
                    if (changePassword != null)
                    {
                        if (changePassword.Status == "SUCCESS")
                        {

                            if (UserManager.ResetPassword(model, out errorMessage))
                            {
                                ViewBag.PasswordReset = true;
                            }
                            else
                            {
                                ModelState.AddModelError("", errorMessage);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", changePassword.StatusMessage);

                        }


                    }

                }
                else
                {
                    errorMessage = "Reset Password link expired";
                    return RedirectToAction("Login", new { message = errorMessage });
                }
            }
            ModelState.AddModelError("", errorMessage);
            return View(model);
        }


        [AuthenticationAuthorize]
        public virtual ActionResult GeneratePassword()
        {
            return View();
        }
        //  [AuthenticationAuthorize]
        [HttpPost]

        public virtual ActionResult GeneratePassword(FormCollection collection)
        {

            if (collection["LegacyPassword"] != null)
            {

                return Content(PasswordHash.CreateHash(collection["LegacyPassword"]));
            }
            var hashPassowd = PasswordHash.CreateHash(collection["Password"]);

            ViewBag.Password = hashPassowd;
            return View();
        }
        [AuthenticationAuthorize]
        public virtual ActionResult ChangePasswordRequired()
        {
            var viewModel = new AccountFirstLoginViewModel();
            var pd_JcatsUserGet_spParams = new LALoDep.Domain.PD_JcatsUser.pd_JcatsUserGet_spParams
            {
                JcatsUserID = UserManager.UserExtended.UserID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };

            var userInfo = UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.PD_JcatsUser.pd_JcatsUserGet_spResults>("pd_JcatsUserGet_sp", pd_JcatsUserGet_spParams).FirstOrDefault();
            if (userInfo != null)
            {
                viewModel.JcatsUserLoginName = userInfo.JcatsUserLoginName;
            }
            return View(viewModel);


        }
        [HttpPost]
        [AuthenticationAuthorize]

        public virtual ActionResult ChangePasswordRequired(AccountFirstLoginViewModel model)
        {
            string errorMessage = string.Empty;
            string succesMessage = string.Empty;
            if (ModelState.IsValid)
            {

                var ng_com_newlogin_spParams = new NG_com_newlogin_spParams()
                {
                    UserName = model.JcatsUserLoginName
                };
                var jcatsUser = UtilityService.ExecStoredProcedureWithResults<NG_com_newlogin_spResult>("NG_pd_newlogin_sp", ng_com_newlogin_spParams).FirstOrDefault();
                if (jcatsUser != null)
                {
                    #region for Change Password
                    if (!string.IsNullOrEmpty(model.OldPassword))
                    {
                        var oldPasswordVerified = PasswordHash.ValidatePassword(model.OldPassword, jcatsUser.NG_JcatsUserPassword);
                        if (oldPasswordVerified)
                        {
                            var changePassword = UtilityService.ExecStoredProcedureWithResults<NG_pd_ChangePassword_spResult>("NG_pd_ChangePassword_sp", new NG_pd_ChangePassword_spParams()
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                ChangeUserID = UserManager.UserExtended.UserID,
                                CurrentPassword = model.OldPassword,
                                NewPassword = model.NewPassword,
                                ResetFlag = 0,
                                UserID = UserManager.UserExtended.UserID

                            }).FirstOrDefault();
                            if (changePassword != null)
                            {
                                if (changePassword.Status == "SUCCESS")
                                {
                                    if (!UserManager.UpdatePassword(model.JcatsUserLoginName,
                                    model.NewPassword))
                                    {
                                        errorMessage = "Change password faild";
                                    }
                                    else
                                    {
                                        succesMessage = "Password changed successfully!";
                                        var identity = (ClaimsIdentity)this.User.Identity;
                                        identity.RemoveClaim(identity.FindFirst("InitialLogin"));
                                        identity.AddClaim(new Claim("InitialLogin", "0"));

                                        HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
                                        HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

                                        return Json(new { URL ="/"+ UserManager.UserExtended.DefaultLandingPage, errorMessage, succesMessage }, JsonRequestBehavior.AllowGet);
                                        //return Redirect("/");
                                    }
                                }
                                else
                                {
                                    errorMessage = changePassword.StatusMessage.Replace(Environment.NewLine, "");
                                }
                            }

                        }
                        else
                        {
                            errorMessage = "Your current password is incorrect";
                        }
                    }
                    #endregion for Change Password
                }
                else
                {
                    errorMessage = "User not exists in system";
                }
            }
            //if (!string.IsNullOrEmpty(errorMessage))
            //    ModelState.AddModelError("error", errorMessage);

            return Json(new { URL = Url.Action(MVC.Account.ChangePasswordRequired()), errorMessage, succesMessage }, JsonRequestBehavior.AllowGet);
            //return View(model);
        }
    }
}