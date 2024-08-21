using System.IO;
using System.Web.Script.Serialization;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.NG_sp.com_JcatsUserPreferences;
using Jcats.SD.UI.ViewModels;
using LALoDep.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Custom.Attributes;
using Microsoft.Owin.Security;
using System.Security.Claims;
using LALoDep.Domain.NG_com;

namespace LALoDep.Controllers
{

    public partial class HomeController : BaseController
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public HomeController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        [AuthenticationAuthorize]
        public virtual ActionResult Index()
        {
            return View();
        }
        private void SignOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
        }

        [AllowAnonymous]
        public virtual ActionResult AccessDenied()
        {
            return View();
        }
        [AllowAnonymous]
        public virtual ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult SessionTimeout()
        {
            SignOut();
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult InvalidGuid()
        {
            SignOut();
            return View();
        }

        [AllowAnonymous]
        public virtual ActionResult AntiForgery()
        {
            return View();
        }


        [HttpPost]
        [AuthenticationAuthorize]
        public virtual ActionResult SelectTheme(FormCollection collection)
        {
            if (string.IsNullOrEmpty(collection["ThemeUrl"]))
                return ErrorResult("Theme URL is not found");

            var themeUrl = collection["ThemeUrl"];

            var spParams = new NG_com_JcatsUserPreferencesInsertOrUpdate_spParams
            {
                UserId = UserEnvironment.UserManager.UserExtended.UserID,
                ThemeCssUrl = themeUrl,
                ZoomCssClass = UserEnvironment.UserManager.UserExtended.ZoomCssClass,
                MyCalendarView = "-1"//send -1 so it will not update the MyCalendarView
                ,
                CustomThemeProperties = "",
                PrintDocumentOn = "-1",
                PageLayout = "-1"
            };
            var identity = UserEnvironment.UserManager.UpdateUserPreferences(spParams);

            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return SuccessResult();
        }


        public virtual ActionResult Preference()
        {
            return View();
        }
        [HttpPost]
        [AuthenticationAuthorize]

        public virtual ActionResult ApplyTheme(FormCollection collection)
        {
            var model = new CustomThemeModel
            {
                PrimaryColor = collection["PrimaryColor"],
                PrimaryFontColor = collection["PrimaryFontColor"],
                SecondaryColor = collection["SecondaryColor"],
                SecondaryFontColor = collection["SecondaryFontColor"]
            };

            var json = new JavaScriptSerializer().Serialize(model);
            var spParams = new NG_com_JcatsUserPreferencesInsertOrUpdate_spParams
            {
                UserId = UserEnvironment.UserManager.UserExtended.UserID,
                ThemeCssUrl = "",
                ZoomCssClass = UserEnvironment.UserManager.UserExtended.ZoomCssClass,
                MyCalendarView = "-1"//send -1 so it will not update the MyCalendarView
                ,
                CustomThemeProperties = json,
                PrintDocumentOn = "-1",
                PageLayout = "-1"
            };
            var identity = UserEnvironment.UserManager.UpdateUserPreferences(spParams);

            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return SuccessResult();

        }
        [HttpPost]
        [AuthenticationAuthorize]

        public virtual ActionResult SavePrintDocumentOn(string id)
        {

            var spParams = new NG_com_JcatsUserPreferencesInsertOrUpdate_spParams
            {
                UserId = UserEnvironment.UserManager.UserExtended.UserID,
                ThemeCssUrl = UserEnvironment.UserManager.UserExtended.ThemeUrl,
                ZoomCssClass = UserEnvironment.UserManager.UserExtended.ZoomCssClass,
                MyCalendarView = "-1"//send -1 so it will not update the MyCalendarView
                ,
                CustomThemeProperties = "-1",
                PrintDocumentOn = id,
                PageLayout = "-1"
            };
            var identity = UserEnvironment.UserManager.UpdateUserPreferences(spParams);

            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return SuccessResult();

        }


        [HttpPost]
        [AuthenticationAuthorize]

        public virtual ActionResult SavePageLayout(string id)
        {

            var spParams = new NG_com_JcatsUserPreferencesInsertOrUpdate_spParams
            {
                UserId = UserEnvironment.UserManager.UserExtended.UserID,
                ThemeCssUrl = UserEnvironment.UserManager.UserExtended.ThemeUrl,
                ZoomCssClass = UserEnvironment.UserManager.UserExtended.ZoomCssClass,
                MyCalendarView = "-1"//send -1 so it will not update the MyCalendarView
                ,
                CustomThemeProperties = "-1",
                PrintDocumentOn = "-1",
                PageLayout = id
            };
            var identity = UserEnvironment.UserManager.UpdateUserPreferences(spParams);

            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return SuccessResult();

        }
        [HttpPost]
        [AuthenticationAuthorize]

        public virtual ActionResult SaveHyperlinkUnderline(string id)
        {

            var spParams = new NG_com_JcatsUserPreferencesHyperlinkUnderline_spParams
            {
                UserId = UserEnvironment.UserManager.UserExtended.UserID,
                HyperlinkUnderline = id == "Yes"
            };

            UtilityService.ExecStoredProcedureWithoutResults("NG_com_JcatsUserPreferencesHyperlinkUnderline_sp", spParams);


            var identity = (ClaimsIdentity)User.Identity;
            if (identity.FindFirst("HyperlinkUnderline") != null)
                identity.RemoveClaim(identity.FindFirst("HyperlinkUnderline"));
            identity.AddClaim(new Claim("HyperlinkUnderline", spParams.HyperlinkUnderline ? "True" : "False"));

            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return SuccessResult();

        }
        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Preview(string path)
        {
            try
            {
                var fileName = path.ToDecrypt();
                if (System.IO.File.Exists(Server.MapPath(fileName)))
                {
                    Response.AppendHeader("Content-Disposition", "inline; filename=" + Path.GetFileName(Server.MapPath(fileName)));

                    return File(Server.MapPath(fileName), "application/pdf");
                }
            }
            catch (Exception ex) { }

            return Content("Invalid File Path");
        }
        public virtual ActionResult NewCaseSession(string caseid, string redirectUrl)
        {

            var url = Request.Url.AbsoluteUri;
            url = redirectUrl.ToDecrypt();
            Custom.UserEnvironment.UserManager.UpdateCaseStatusBar(caseid.ToDecrypt().ToInt());
            return Redirect(url);
        }
        [AuthenticationAuthorize]
        public virtual ActionResult Redirect(string Page, string ID)
        {

            if (ID.IsNullOrEmpty() || Page.IsNullOrEmpty())
            {
                return Content("Invalid URL");
            }
            if (UserManager.UserExtended.CaseID != ID.ToInt())
                UserManager.UpdateCaseStatusBar(ID.ToInt());
            return Redirect(string.Format("/g/{0}/{1}?CaseID=" + ID.ToEncrypt(), CaseSummaryBarInfo.GetCurrentWindowID(), Page));
        }


        #region Notification 
        [HttpPost]
        [AuthenticationAuthorize]
        public virtual ActionResult UpdateDeviceToken(string token, string deviceType)
        {
            bool isSuccess = true;
            try
            {
                UtilityService.ExecStoredProcedureWithResults<NG_DeviceTokenIUD_spResult>("NG_DeviceTokenIUD_sp", new NG_DeviceTokenIUD_spParams
                {
                    IUD = "INSERT",
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    DeviceType = deviceType,
                    RecordStateID = 1,
                    Token = token
                }).FirstOrDefault();
            }
            catch { isSuccess = false; }

            return Json(new { isSuccess }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}