using System.Globalization;
using LALoDep.Domain.NG.com_JcatsUser;
using LALoDep.Domain.NG.com_JcatsUserPreferences;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.NG_pd_login_spParams;
using LALoDep.Core.NG_pd_login_spResult;
using LALoDep.Core.NG_sp.com_JcatsUserPreferences;
using LALoDep.Core.PasswordHasher;
using LALoDep.Custom.Security;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using LALoDep.Core.Custom.Utility;
using LALoDep.Models;
using LALoDep.Core.NG_sp.com_Login;
using LALoDep.Core.NG_sp.com_Security;
using LALoDep.Core.NG_sp.com_LogReset;
using LALoDep.Core.Enums;
using LALoDep.Domain.PD_JcatsUser;
using NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult = LALoDep.Custom.Security.NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult;
using LALoDep.Domain;

namespace LALoDep.Custom
{
    public class UserManager
    {
        public UserManager(LALoDep.Domain.Services.IUtilityService utilityService)
        {
            this.UtilityService = utilityService;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session.Timeout = this.UserExtended.SessionTimeOut;

            }
        }
        private IUtilityService UtilityService;
        public string CurrentSessionGuid
        {
            get
            {
                return CaseSummaryBarInfo.GetCurrentWindowID();
            }
        }
        public string NewSessionUrl
        {
            get
            {
                return "/g/" + Guid.NewGuid().ToString("N") + "/Case/Search";
            }
        }
        public virtual UserExtended UserExtended
        {
            get
            {
                var claims = (HttpContext.Current.User.Identity as ClaimsIdentity).Claims;

                var userExtended = new UserExtended();
                if (claims != null && claims.Any())
                {
                    try
                    {


                        if (claims.First(o => o.Type == "UserID") != null)
                            userExtended.UserID = Convert.ToInt32(claims.First(o => o.Type == "UserID").Value);
                        if (claims.First(o => o.Type == "PersonID") != null)
                            userExtended.PersonID = Convert.ToInt32(claims.First(o => o.Type == "PersonID").Value);
                        if (claims.First(o => o.Type == "AgencyID") != null)
                            userExtended.AgencyID = Convert.ToInt32(claims.First(o => o.Type == "AgencyID").Value);
                        if (claims.First(o => o.Type == "BranchID") != null)
                            userExtended.BranchID = Convert.ToInt32(claims.First(o => o.Type == "BranchID").Value);
                        if (claims.First(o => o.Type == "AttorneyRoleID") != null)
                            userExtended.AttorneyRoleID = Convert.ToInt32(claims.First(o => o.Type == "AttorneyRoleID").Value);
                        if (claims.First(o => o.Type == "JcatsGroupID") != null)
                            userExtended.JcatsGroupID = Convert.ToInt32(claims.First(o => o.Type == "JcatsGroupID").Value);
                        if (claims.First(o => o.Type == "Guid") != null)
                            userExtended.Guid = new Guid(claims.First(o => o.Type == "Guid").Value);
                        if (claims.First(o => o.Type == ClaimTypes.GivenName) != null)
                            userExtended.FullName = string.Format("{0} {1}", claims.First(o => o.Type == ClaimTypes.GivenName).Value, claims.First(o => o.Type == ClaimTypes.Surname).Value);

                        if (claims.First(o => o.Type == "SystemAdminFlag") != null)
                            userExtended.SystemAdminFlag = Convert.ToInt32(claims.First(o => o.Type == "SystemAdminFlag").Value);

                        if (claims.First(o => o.Type == "SessionTimeOut") != null)
                            userExtended.SessionTimeOut = Convert.ToInt32(claims.First(o => o.Type == "SessionTimeOut").Value);



                        if (!claims.Any(x => x.Type == "ApiIdentity"))
                        {
                            // all of these variables are using only in web app.
                            if (claims.First(o => o.Type == "ThemeUrl") != null)
                                userExtended.ThemeUrl = claims.First(o => o.Type == "ThemeUrl").Value;
                            if (claims.First(o => o.Type == "ZoomCssClass") != null)
                                userExtended.ZoomCssClass = claims.First(o => o.Type == "ZoomCssClass").Value;
                            if (claims.First(o => o.Type == "AdminLoginName") != null)
                                userExtended.AdminLoginName = claims.First(o => o.Type == "AdminLoginName").Value;
                            if (claims.First(o => o.Type == "InitialLogin") != null)
                                userExtended.InitialLogin = Convert.ToInt32(claims.First(o => o.Type == "InitialLogin").Value);
                            if (claims.First(o => o.Type == "DefaultLandingPage") != null)
                                userExtended.DefaultLandingPage = claims.First(o => o.Type == "DefaultLandingPage").Value;

                            if (claims.First(o => o.Type == "PrintDocumentOn") != null)
                                userExtended.PrintDocumentOn = claims.First(o => o.Type == "PrintDocumentOn").Value == "" ? "NewWindow" : claims.First(o => o.Type == "PrintDocumentOn").Value;
                            if (claims.First(o => o.Type == "HyperlinkUnderline") != null)
                                userExtended.HyperlinkUnderline = claims.First(o => o.Type == "HyperlinkUnderline").Value.ToBoolean();

                            if (claims.First(o => o.Type == "PageLayout") != null)
                            {
                                if (claims.First(o => o.Type == "PageLayout").Value == "")
                                    userExtended.PageLayout = "Standard";
                                else
                                {
                                    userExtended.PageLayout = claims.First(o => o.Type == "PageLayout").Value;

                                }
                            }
                        }

                        var caseInfo = CaseSummaryBarInfo.CaseInfo();
                        userExtended.CaseID = caseInfo.CaseID;
                        userExtended.Client = caseInfo.Client;
                        userExtended.PDAPDNumber = caseInfo.PDAPDNumber;
                        userExtended.ApptDate = caseInfo.ApptDate;
                        userExtended.Status = caseInfo.Status;
                        userExtended.Attorney = caseInfo.Attorney;
                        userExtended.CaseJcatsNumber = caseInfo.CaseJcatsNumber;
                        userExtended.CaseNumberAgencyID = caseInfo.CaseNumberAgencyID;
                        userExtended.NextHearingDate = caseInfo.NextHearingDate;
                    }
                    catch (Exception ex)
                    {

                        if (ex.Message.Contains("Sequence contains no matching element"))
                        {
                            return userExtended;
                        }
                    }
                }

                return userExtended;

            }
        }

        public bool Authenticate(AccountLoginViewModel model, out string loginStatusMessage, out string landingPage, out NG_pd_login_spResult oLogin)
        {
            var ng_com_newlogin_spParams = new NG_com_newlogin_spParams
            {
                UserName = model.Username
            };

            var jcatsUser = UtilityService.ExecStoredProcedureWithResults<NG_com_newlogin_spResult>("NG_pd_newlogin_sp", ng_com_newlogin_spParams).FirstOrDefault();

            if (jcatsUser == null)
            {
                loginStatusMessage = "Invalid Username or Password.";
                landingPage = string.Empty;
                oLogin = null;
                return false;
            }
            if (jcatsUser != null && jcatsUser.NG_JcatsUserPassword.IsNullOrEmpty())
            {
                loginStatusMessage = "Your JCATS NG account has not been activated. Please contact your JCATS administrator.";
                landingPage = string.Empty;
                oLogin = null;
                return false;
            }
          

                var guid = Guid.NewGuid();

            var request = HttpContext.Current.Request;
            var browser = request.Browser;
            var passwordVerified = PasswordHash.ValidatePassword(model.Password, jcatsUser.NG_JcatsUserPassword);

            var nG_pd_login_spParams = new NG_pd_login_spParams
            {
                UserName = model.Username,
                Password = passwordVerified ? "success" : "fail",
                AdminFlag = 0,
                LoginKey = guid,
                Width = model.Width,
                Height = model.Height,
                AvailWidth = model.AvailWidth,
                AvailHeight = model.AvailHeight,
                ClientInfo = string.Format("{0}-{1}-{2}-{3}",
                    browser.Platform,
                    browser.Type,
                    browser.Version,
                    browser.MajorVersion),
                ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
                ClientHost = System.Net.Dns.GetHostName(),
                BatchLogJobID = Guid.NewGuid(),
                RecordStateID = 1,
            };

            var login = UtilityService.ExecStoredProcedureWithResults<NG_pd_login_spResult>("NG_pd_Login_sp", nG_pd_login_spParams).First();

            if (!passwordVerified)
            {
                loginStatusMessage = "Invalid Username or Password.";
                landingPage = string.Empty;
                oLogin = null;
                return false;
            }
            var user = UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGet_spResults>("pd_JcatsUserGet_sp", new pd_JcatsUserGet_spParams
            {
                JcatsUserID = login.UserID.Value,
                UserID = login.UserID.Value,
                BatchLogJobID = Guid.NewGuid(),
            }).FirstOrDefault();
            if (user == null)
            {
                loginStatusMessage = "pd_JcatsUserGet_sp Not returning data";
                landingPage = string.Empty;
                oLogin = null;
                return false;

            }


            switch (login.LoginSuccessful)
            {
                case LoginStatus.InvalidKey:
                    loginStatusMessage = "Another login session has begun using your user name and password and has expired your current session. If you feel this is in error, please change your password immediately.";
                    landingPage = string.Empty;
                    oLogin = null;
                    return false;
                case LoginStatus.TimedOut:
                    loginStatusMessage = " Your login has expired due to an inactive session.";
                    landingPage = string.Empty;
                    oLogin = null;
                    return false;
                case LoginStatus.AccountLocked:
                    loginStatusMessage = "The account is locked.  Please contact your JCATS administrator.";
                    landingPage = string.Empty;
                    oLogin = null;
                    return false;
                case LoginStatus.InvalidUser:
                    loginStatusMessage = "Invalid User Name and/or Password.";
                    landingPage = string.Empty;
                    oLogin = null;
                    return false;
            }
            if (!login.InitialLogin.HasValue)
            {

                loginStatusMessage = login.PasswordMessage;
                landingPage = string.Empty;
                oLogin = null;
                return false;
            }
            if (login.InitialLogin.HasValue && login.InitialLogin == 1 && login.LoginSuccessful == LoginStatus.Successful)
            {
                loginStatusMessage = login.PasswordMessage;
                landingPage = "Account/ChangePasswordRequired";
                oLogin = login;
                return true;
            }
            loginStatusMessage = String.Empty;
            landingPage = login.InitialURL;
            oLogin = login;

            return true;


        }
        public bool LoginAs(AccountLoginViewModel model, out string loginStatusMessage, out string landingPage, byte adminFlag = 0)
        {


            var ng_com_newlogin_spParams = new NG_com_newlogin_spParams
            {
                UserName = model.Username
            };

            var jcatsUser = UtilityService.ExecStoredProcedureWithResults<NG_com_newlogin_spResult>("NG_pd_newlogin_sp", ng_com_newlogin_spParams).FirstOrDefault();

            if (jcatsUser == null)
            {
                loginStatusMessage = "Incorrect username.";
                landingPage = string.Empty;

                return false;
            }

            var guid = Guid.NewGuid();

            var request = HttpContext.Current.Request;
            var browser = request.Browser;

            var nG_pd_login_spParams = new NG_pd_login_spParams
            {
                UserName = model.Username,

                AdminFlag = adminFlag,
                LoginKey = guid,
                Width = model.Width,
                Height = model.Height,
                AvailWidth = model.AvailWidth,
                AvailHeight = model.AvailHeight,
                ClientInfo = string.Format("{0}-{1}-{2}-{3}",
                    browser.Platform,
                    browser.Type,
                    browser.Version,
                    browser.MajorVersion),
                ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
                ClientHost = System.Net.Dns.GetHostName(),
                BatchLogJobID = Guid.NewGuid(),
                RecordStateID = 1,
            };

            var login = UtilityService.ExecStoredProcedureWithResults<NG_pd_login_spResult>("NG_pd_Login_sp", nG_pd_login_spParams).First();

            switch (login.LoginSuccessful)
            {
                case LoginStatus.InvalidKey:
                    loginStatusMessage = "Another login session has begun using your user name and password and has expired your current session. If you feel this is in error, please change your password immediately.";
                    landingPage = string.Empty;

                    return false;
                case LoginStatus.TimedOut:
                    loginStatusMessage = " Your login has expired due to an inactive session.";
                    landingPage = string.Empty;

                    return false;
                case LoginStatus.AccountLocked:
                    loginStatusMessage = "The account is locked.  Please contact your JCATS administrator.";
                    landingPage = string.Empty;

                    return false;
                case LoginStatus.InvalidUser:
                    loginStatusMessage = "Invalid User Name and/or Password.";
                    landingPage = string.Empty;

                    return false;
            }

            loginStatusMessage = String.Empty;
            landingPage = login.InitialURL;

            return true;
        }

        public ClaimsIdentity CreateIdentity(AccountLoginViewModel model, byte adminFlag = 0, int initialLogin = 0, NG_pd_login_spResult oLogin = null)
        {
            var identity = new ClaimsIdentity("ApplicationCookie");
            var guid = Guid.NewGuid();

            var request = HttpContext.Current.Request;
            var browser = request.Browser;
            if (oLogin == null)
            {
                var nG_pd_login_spParams = new NG_pd_login_spParams
                {
                    UserName = model.Username,
                    Password = string.Empty,
                    AdminFlag = 0,
                    LoginKey = guid,
                    Width = model.Width,
                    Height = model.Height,
                    AvailWidth = model.AvailWidth,
                    AvailHeight = model.AvailHeight,
                    ClientInfo = string.Format("{0}-{1}-{2}-{3}",
                        browser.Platform,
                        browser.Type,
                        browser.Version,
                        browser.MajorVersion),
                    ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
                    ClientHost = System.Net.Dns.GetHostName(),
                    BatchLogJobID = Guid.NewGuid(),
                    RecordStateID = 1,
                };

                oLogin = UtilityService.ExecStoredProcedureWithResults<NG_pd_login_spResult>("NG_pd_Login_sp", nG_pd_login_spParams).First();

                if (oLogin.LoginSuccessful != LoginStatus.Successful)
                {
                    throw new ArgumentException("LoginStatus is: " + oLogin.LoginSuccessful.ToString());
                }

            }

            var pd_JcatsUserGet_spParams = new pd_JcatsUserGet_spParams
            {
                JcatsUserID = oLogin.UserID.Value,
                UserID = oLogin.UserID.Value,
                BatchLogJobID = Guid.NewGuid(),
            };

            var user = UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGet_spResults>("pd_JcatsUserGet_sp", pd_JcatsUserGet_spParams).FirstOrDefault();

            var userPreferencesParams = new NG_com_JcatsUserPreferencesGet_spParams
            {
                UserId = oLogin.UserID.Value
            };

            var userPreferencesResult = UtilityService.ExecStoredProcedureWithResults<NG_com_JcatsUserPreferencesGet_spResult>("NG_com_JcatsUserPreferencesGet_sp", userPreferencesParams).FirstOrDefault();
            var themeUrl = "";
            var zoomCssClass = "";
            var printDocumentOn = "";
            var pageLayout = "";
            var hyperlinkUnderline = false;
            if (userPreferencesResult != null)
            {
                themeUrl = userPreferencesResult.ThemeCssUrl.IsNullOrEmpty() ? userPreferencesResult.CustomThemeProperties : userPreferencesResult.ThemeCssUrl;
                zoomCssClass = userPreferencesResult.ZoomCssClass.IsNullOrEmpty() ? "" : userPreferencesResult.ZoomCssClass;
                printDocumentOn = userPreferencesResult.PrintDocumentOn.IsNullOrEmpty() ? "" : userPreferencesResult.PrintDocumentOn;

                pageLayout = userPreferencesResult.PageLayout.IsNullOrEmpty() ? "" : userPreferencesResult.PageLayout;
                hyperlinkUnderline = userPreferencesResult.HyperlinkUnderline;


            }
            identity.AddClaim(new Claim("UserID", oLogin.UserID.Value.ToString()));
            identity.AddClaim(new Claim("PersonID", user.PersonID.ToString()));
            identity.AddClaim(new Claim("AgencyID", user.AgencyID.ToString()));
            identity.AddClaim(new Claim("BranchID", user.AgencyID.ToString()));
            identity.AddClaim(new Claim("JcatsGroupID", user.JcatsGroupID.ToString()));
            identity.AddClaim(new Claim("AttorneyRoleID", 0.ToString()));
            identity.AddClaim(new Claim("Guid", oLogin.LoginKey));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.PersonNameLast));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.PersonNameFirst));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.JcatsUserLoginName));
            identity.AddClaim(new Claim("ThemeUrl", themeUrl ?? string.Empty));
            identity.AddClaim(new Claim("ZoomCssClass", zoomCssClass));
            identity.AddClaim(new Claim("SystemAdminFlag", user.SystemAdminFlag.ToString()));
            identity.AddClaim(new Claim("InitialLogin", initialLogin.ToString()));
            identity.AddClaim(new Claim("DefaultLandingPage", oLogin.InitialURL ?? string.Empty));

            identity.AddClaim(adminFlag == 1
         ? new Claim("AdminLoginName", HttpContext.Current.User.Identity.Name)
         : new Claim("AdminLoginName", ""));
            identity.AddClaim(new Claim("PrintDocumentOn", printDocumentOn));





            identity.AddClaim(new Claim("CaseID", 0.ToString()));
            identity.AddClaim(new Claim("Client", String.Empty));
            identity.AddClaim(new Claim("PDAPDNumber", String.Empty));
            identity.AddClaim(new Claim("ApptDate", String.Empty));
            identity.AddClaim(new Claim("Status", String.Empty));
            identity.AddClaim(new Claim("Attorney", String.Empty));
            identity.AddClaim(new Claim("CaseJcatsNumber", String.Empty));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.SystemAdminFlag == 1
                ? Role.Admin.ToString()
                : Role.Standard.ToString()));

            identity.AddClaim(new Claim("CaseNumberAgencyID", "0"));

            identity.AddClaim(new Claim("NextHearingDate", string.Empty));


            /*   if (user.JcatsUserTimeOut.HasValue)
               {
                   identity.AddClaim(new Claim("SessionTimeOut", user.JcatsUserTimeOut.Value.ToString()));

                   HttpContext.Current.Session.Timeout = user.JcatsUserTimeOut.Value;
               }
               else
               {*/
            identity.AddClaim(new Claim("SessionTimeOut", "2400"));

            identity.AddClaim(new Claim("PageLayout", pageLayout));

            identity.AddClaim(new Claim("HyperlinkUnderline", hyperlinkUnderline ? "True" : "False"));

            HttpContext.Current.Session.Timeout = 2400;

            /*   }*/

            UtilityFunctions.DeleteFilesOlderThanXDays(user.JcatsUserID);

            return identity;
        }

        public List<NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult> LastViewTokens
        {
            get;
            set;
        }

        private string LastViewName { get; set; }

        public LoginStatus HasAccessTo(HttpContextBase httpContext, bool isPageMethod = false, int pageSecurityItemId = 0, string customSecureId = null)
        {


            var userData = this.UserExtended;
            var guid = Guid.NewGuid();

            var request = HttpContext.Current.Request;
            var browser = request.Browser;



            var isUserLoginSuccess = UtilityService.ExecStoredProcedureWithResults<NG_pd_LoginCheckSuccess_spResult>("NG_pd_LoginCheckSuccess_sp", new NG_pd_LoginCheckSuccess_spParams()
            {

                AdminFlag = !string.IsNullOrEmpty(userData.AdminLoginName) ? 1 : 0,
                LoginKey = userData.Guid.ToString(),

                ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
                ClientHost = System.Net.Dns.GetHostName(),

                BatchLogJobID = Guid.NewGuid(),
                RecordStateID = 1,
                ASPPageName = httpContext.Request.Url.AbsolutePath,
                LogoutFlag = 0,
                ReturnRecordFlag = 1,
                ServerInfo = "",
                UserID = userData.UserID
            }).FirstOrDefault();

            //var userLoginKey = UtilityService.Context.NG_pd_UserLoginCheckGetByJcatsUserID_sp(userData.UserID, Guid.NewGuid())
            //       .FirstOrDefault();
            //if (userLoginKey != null)
            //{
            //    TimeSpan span = DateTime.Now - userLoginKey.AccessDateTime;
            //    if (Guid.Parse(userLoginKey.LoginKey) != userData.Guid && !(System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] != null && System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] == "true"))
            //    {
            //        return LogonStatusCode.InvalidGuid;
            //    }
            //    else if (span.Minutes >=
            //             System.Web.Configuration.WebConfigurationManager.AppSettings["SystemTimeOutValue"].ToInt())
            //    {
            //        return LogonStatusCode.SessionTimeout;
            //    }
            //    else
            //    {

            //        UtilityService.Context.NG_pd_LoginCheckSuccess_sp(userData.Guid.ToString(), 1, userData.UserID,
            //            Guid.NewGuid(), !string.IsNullOrEmpty(userData.AdminLoginName) ? 1 : 0, Utility.GetClientIpAddress(),
            //            System.Net.Dns.GetHostName(), httpContext.Request.Url.AbsolutePath, "", 0, 1);
            //    }
            //}
            //else
            //{
            //    return LogonStatusCode.SessionTimeout;

            //}
            var isLocalMode = false;
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] != null && System.Web.Configuration.WebConfigurationManager.AppSettings["IsLocalMode"] == "true")
            {
                isLocalMode = true;
            }
            if (isUserLoginSuccess == null && !isLocalMode)
            {
                return LoginStatus.TimedOut; ;
            }
            if (isUserLoginSuccess != null && isUserLoginSuccess.StatusCode != LoginStatus.Successful && !isLocalMode)
            {
                return isUserLoginSuccess.StatusCode;
            }
            if ((string.IsNullOrWhiteSpace(LastViewName) || LastViewTokens == null))
            {
                if (customSecureId.IsNullOrEmpty())
                {
                    customSecureId = pageSecurityItemId.ToString();

                }
                LastViewTokens =
                     UtilityService.Context.NG_pd_SecurityGetBySecurityItemIDsUserID_sp(customSecureId, userData.UserID,
                         Guid.NewGuid()).ToList()
                         .Select(
                             o =>
                                 new NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult()
                                 {
                                     SecurityItemID = (SecurityToken)o.SecurityItemID,
                                     LogonStatusCodeID = LogonStatusCode.Success,
                                     MVCSecurityCode = o.SecurityItemID.ToString()
                                 })
                         .ToList();

            }

            if (LastViewTokens == null || LastViewTokens.Count <= 0) return LoginStatus.AccessDenied;
            if (pageSecurityItemId > 0)
            {
                var dbCode = LastViewTokens.Where(t => ((int)t.SecurityItemID) == pageSecurityItemId).ToList();
                if (dbCode.Any())
                    return LoginStatus.Successful;

            }
            else
            {
                if (LastViewTokens.Count > 0)
                {
                    return LoginStatus.Successful;
                }

            }


            return LoginStatus.AccessDenied;
        }
        public LogonStatusCode MobileHasAccessTo(HttpContextBase httpContext)
        {

            var userData = this.UserExtended;
            var com_SecurityGetByASPPageNameUserIDAgencyID_spParams = new NG_com_SecurityGetBySecurityItemIDUserID_spParams
            {
                ASPPageName = LastViewName,//controller,,
                LoginKey = userData.Guid.ToString(),
                UserID = userData.UserID,

                ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
                ClientHost = System.Net.Dns.GetHostName(),
                BatchLogJobID = Guid.NewGuid(),
                SecurityItemID = 786
            };

            var security = UtilityService.ExecStoredProcedureWithResults<NG_com_SecurityGetBySecurityItemIDUserID_spResult>("NG_com_SecurityGetBySecurityItemIDUserID_sp", com_SecurityGetByASPPageNameUserIDAgencyID_spParams).FirstOrDefault();
            if (security != null)
            {
                if (security.HasAccessFlag == 0)
                    return LogonStatusCode.AccessDenied;

                return security.LogonStatusCodeID;
            }
            return LogonStatusCode.AccessDenied;



        }


        public string JsonTokens()
        {
            if (LastViewTokens != null)
                return JsonConvert.SerializeObject(LastViewTokens.Where(c => !string.IsNullOrWhiteSpace(c.MVCSecurityCode)).Select(c => c.MVCSecurityCode).ToList());
            return string.Empty;
        }


        public string GeneratePasswordResetToken(AccountForgotPasswordViewModel model, DateTime expiryDate, out string errorMessage)
        {
            var request = HttpContext.Current.Request;

            var db = new DbManager();
            db.AddInParam("UserName", model.Username);
            var userid = db.ExecuteScalar("select JcatsUserID from jcatsuser where jcatsuserloginname = @UserName", System.Data.CommandType.Text);

            if (userid == null)
            {
                errorMessage = "Username and Email do not match. Please contact your JCATS administrator to reset your password";
                return string.Empty;
            }
            var pd_JcatsUserGet_spParams = new pd_JcatsUserGet_spParams
            {
                JcatsUserID = userid.ToInt(),
                UserID = userid.ToInt(),
                BatchLogJobID = Guid.NewGuid(),
            };

            var user = UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGet_spResults>("pd_JcatsUserGet_sp", pd_JcatsUserGet_spParams).First();

            if (user != null)
            {
                if (!String.Equals(user.Email, model.EmailAddress, StringComparison.CurrentCultureIgnoreCase))
                {
                    errorMessage = "No valid email is linked to this account. Please contact your JCATS administrator to reset your password";
                    return string.Empty;
                }
            }
            else
            {
                errorMessage = "the information does not match";
                return string.Empty;
            }


            var logSpParams = new NG_com_LogResetPasswordInsert_spParams
            {
                UserName = model.Username,
                EmailId = model.EmailAddress,
                ExpiryDate = expiryDate,
                IpAddress = request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
                RequestDate = DateTime.Now
            };

            var log = UtilityService.ExecStoredProcedureWithResults<NG_com_LogResetPasswordInsert_spResult>("NG_com_LogResetPasswordInsert_sp", logSpParams).FirstOrDefault();

            errorMessage = string.Empty;

            return log != null ? log.Code.ToString() : string.Empty;
        }

        public bool ValidatePasswordCode(Guid code, out NG_com_LogResetPasswordValidateCode_spResult result)
        {

            var validateSpParams = new NG_com_LogResetPasswordValidateCode_spParams
            {
                Code = code
            };

            result = UtilityService.ExecStoredProcedureWithResults<NG_com_LogResetPasswordValidateCode_spResult>(" NG_com_LogResetPasswordValidateCode_sp", validateSpParams).FirstOrDefault();


            return result != null;
        }
        public bool ResetPassword(AccountResetPasswordViewModel model, out string errorMessage)
        {
            var request = HttpContext.Current.Request;
            if (UpdatePassword(model.Username, model.Password))
            {
                var parms = new Dictionary<string, object>
                {
                    {"@IsDone", true},
                    {"@IPAddress", request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress},
                    {"@RequestCode", model.Code},
                    {"@ResetDate", DateTime.Now}
                };

                UtilityService.ExecStoredProcedureWithoutResults("NG_com_LogResetPasswordUpdate_sp", parms);
                errorMessage = string.Empty;
                return true;
            }

            errorMessage = "Faild to update password";


            return false;
        }

        public bool UpdatePassword(string userName, string newPassword, int changePasswordFlag = 0)
        {
            var hashPassowd = Jcats.CA.UI.Custom.PasswordHash.CreateHash(newPassword);

            var updatePasswordParams = new NG_com_JcatsUserUpdatePassword_spParams
            {
                UserName = userName,
                Password = hashPassowd,
                ChangePasswordFlag = changePasswordFlag
            };

            var result = UtilityService.ExecStoredProcedureWithResults<NG_com_JcatsUserUpdatePassword_spResult>("NG_pd_JcatsUserUpdatePassword_sp", updatePasswordParams).FirstOrDefault();

            return (result != null && result.IsUpdated);
        }

        public IUtilityService UtilityService1
        {
            get { return UtilityService; }
            set { UtilityService = value; }
        }
        //  <summary>
        //Update User Preferences in DB. 
        //    Update Identity Information and return ClaimsIdentity object
        //   </summary>
        public ClaimsIdentity UpdateUserPreferences(NG_com_JcatsUserPreferencesInsertOrUpdate_spParams spParams)
        {
            var parms = new Dictionary<string, object>
            {
                {"@UserId",spParams.UserId},
                {"@ThemeCssUrl",spParams.ThemeCssUrl  },
                {"@ZoomCssClass",spParams.ZoomCssClass  },
                {"@MyCalendarView",spParams.MyCalendarView  },
                {"@CustomThemeProperties",spParams.CustomThemeProperties  },
                {"@PrintDocumentOn",spParams.PrintDocumentOn  },

                {"@PageLayout",spParams.PageLayout  }
            };
            UtilityService.ExecStoredProcedureWithoutResults("NG_com_JcatsUserPreferencesInsertOrUpdate_sp", parms);


            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            identity.RemoveClaim(identity.FindFirst("ZoomCssClass"));
            identity.AddClaim(new Claim("ZoomCssClass", spParams.ZoomCssClass));

            identity.RemoveClaim(identity.FindFirst("ThemeUrl"));
            identity.AddClaim(new Claim("ThemeUrl", spParams.ThemeCssUrl.IsNullOrEmpty() ? (spParams.CustomThemeProperties ?? string.Empty) : spParams.ThemeCssUrl));
            var printDocumentOn = UserEnvironment.UserManager.UserExtended.PrintDocumentOn;
            identity.RemoveClaim(identity.FindFirst("PrintDocumentOn"));
            if (spParams.PrintDocumentOn == "-1")
            {
                identity.AddClaim(new Claim("PrintDocumentOn", printDocumentOn));
            }
            else
            {
                identity.AddClaim(new Claim("PrintDocumentOn", spParams.PrintDocumentOn));
            }

            var pageLayout = UserEnvironment.UserManager.UserExtended.PageLayout;
            identity.RemoveClaim(identity.FindFirst("PageLayout"));
            if (spParams.PageLayout == "-1")
            {
                identity.AddClaim(new Claim("PageLayout", pageLayout));
            }
            else
            {
                identity.AddClaim(new Claim("PageLayout", spParams.PageLayout));
            }
            return identity;
        }

        public void CheckSecurityPermission(string securityIds)
        {
            LastViewTokens =
                    UtilityService.Context.NG_pd_SecurityGetBySecurityItemIDsUserID_sp(securityIds, this.UserExtended.UserID,
                        Guid.NewGuid()).ToList()
                        .Select(
                            o =>
                                new NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult()
                                {
                                    SecurityItemID = (SecurityToken)o.SecurityItemID,
                                    LogonStatusCodeID = LogonStatusCode.Success,
                                    MVCSecurityCode = o.SecurityItemID.ToString()
                                })
                        .ToList();

        }
        public bool IsUserAccessToSecurity(SecurityToken security)
        {
            var id = (int)security;
            return
                     UtilityService.Context.NG_pd_SecurityGetBySecurityItemIDsUserID_sp(id.ToString(), this.UserExtended.UserID,
                         Guid.NewGuid()).ToList()
                         .Select(
                             o =>
                                 new NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult()
                                 {
                                     SecurityItemID = (SecurityToken)o.SecurityItemID,
                                     LogonStatusCodeID = LogonStatusCode.Success,
                                     MVCSecurityCode = o.SecurityItemID.ToString()
                                 })
                         .ToList().Any();

        }
        public bool IsUserAccessTo(SecurityToken security)
        {
            try
            {
                return LastViewTokens.Any(o => o.SecurityItemID == security);

            }
            catch
            {
            }
            return false;

        }
        //public LogonStatusCode MobileHasAccessTo(HttpContextBase httpContext)
        //{

        //    var userData = this.UserExtended;
        //    var com_SecurityGetByASPPageNameUserIDAgencyID_spParams = new NG_com_SecurityGetBySecurityItemIDUserID_spParams
        //    {
        //        ASPPageName = LastViewName,//controller,,
        //        LoginKey = userData.Guid.ToString(),
        //        UserID = userData.UserID,

        //        ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
        //        ClientHost = System.Net.Dns.GetHostName(),
        //        BatchLogJobID = Guid.NewGuid(),
        //        SecurityItemID = 786
        //    };

        //    var security = UtilityService.ExecStoredProcedureWithResults<NG_com_SecurityGetBySecurityItemIDUserID_spResult>("NG_com_SecurityGetBySecurityItemIDUserID_sp", com_SecurityGetByASPPageNameUserIDAgencyID_spParams).FirstOrDefault();
        //    if (security != null)
        //    {
        //        if (security.HasAccessFlag == 0)
        //            return LogonStatusCode.AccessDenied;

        //        return security.LogonStatusCodeID;
        //    }
        //    return LogonStatusCode.AccessDenied;



        //}

        /// <summary>
        ///     Updates Case Session Variables.
        ///  Update with 0 to clear all Case Session Variables.

        /// </summary>


        public bool UpdateCaseStatusBar(int caseId, bool accessDeniedRedirect = true, pd_CaseGet_sp_Result oCase = null)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var caseInfo = CaseSummaryBarInfo.CaseInfo();
            if (caseInfo.CaseID != 0 && caseInfo.CaseID != caseId && HttpContext.Current.Request.QueryString["_uniquerequest"] == null)
            {
                string guid = Guid.NewGuid().ToString("N");
                var url = "";
                var test = HttpContext.Current.Request.Url.AbsolutePath.Split('/').ToList();
                for (var i = 3; i < test.Count; i++)
                {
                    if (url != "")
                        url += "/";
                    url += test[i];
                }

                url = HttpContext.Current.Request.Url.AbsolutePath == "/" ? "Case/Search" : "/g/" + guid + "/" + url + HttpContext.Current.Request.Url.Query;

                //    HttpContext.Current.Response.Redirect("~/g/" + guid + "/Home/NewCaseSession?caseId=" + caseId.ToEncrypt() + "&redirectUrl=" + url.ToEncrypt());
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    //HttpContext.Current.Response.RedirectPermanent("~/g/" + guid + "/Home/NewCaseSession?caseId=" + caseId.ToEncrypt() + "&redirectUrl=" + url.ToEncrypt(), true);

                    HttpContext.Current.Response.RedirectPermanent(url, true);


                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }
                return false;
            }

            if (caseId != 0) //update
            {
                var summary = oCase ?? UtilityService.Context.pd_CaseGet_sp(caseId, UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();

                if (summary != null)
                {
                    if (summary.CaseAccessLevel == "ACCESS_DENIED" && accessDeniedRedirect)
                    {

                        HttpContext.Current.Session["CaseAccessLevelMessage"] = summary.CaseAccessLevelMessage;
                        HttpContext.Current.Response.Redirect("/Case/Search");
                    }
                    caseInfo.CaseID = caseId;
                    caseInfo.PDAPDNumber = summary.PetitionNumber;
                    caseInfo.ApptDate = summary.ApptDateDisplay;
                    caseInfo.Status = summary.StatusDisplay;
                    caseInfo.Client = summary.CaseName;
                    caseInfo.Attorney = summary.LeadAttorney;
                    caseInfo.CaseJcatsNumber = summary.CaseNumber;
                    caseInfo.CaseNumberAgencyID = summary.AgencyID;
                    caseInfo.NextHearingDate = summary.NextHearingDate.ToDefaultFormat() ?? string.Empty;

                    /*
                    identity.RemoveClaim(identity.FindFirst("CaseID"));
                    identity.AddClaim(new Claim("CaseID", caseId.ToString(CultureInfo.InvariantCulture)));

                    identity.RemoveClaim(identity.FindFirst("PDAPDNumber"));
                    identity.AddClaim(new Claim("PDAPDNumber", summary.PetitionNumber ?? String.Empty));

                    identity.RemoveClaim(identity.FindFirst("ApptDate"));
                    identity.AddClaim(new Claim("ApptDate", summary.CaseAppointmentDate.ToDefaultFormat() ?? String.Empty));

                    identity.RemoveClaim(identity.FindFirst("Status"));
                    identity.AddClaim(new Claim("Status", summary.CaseClosedDate.HasValue ? "Closed" : "Open"));

                    identity.RemoveClaim(identity.FindFirst("Client"));
                    identity.AddClaim(new Claim("Client", summary.CaseName));

                    identity.RemoveClaim(identity.FindFirst("Attorney"));
                    identity.AddClaim(new Claim("Attorney", summary.LeadAttorney));

                    identity.RemoveClaim(identity.FindFirst("CaseJcatsNumber"));
                    identity.AddClaim(new Claim("CaseJcatsNumber", summary.CaseNumber));


                    identity.RemoveClaim(identity.FindFirst("CaseNumberAgencyID"));
                    identity.AddClaim(new Claim("CaseNumberAgencyID", summary.AgencyID.ToString()));
                    */

                }


                /*
                HttpContext.Current.GetOwinContext().Authentication.SignOut("ApplicationCookie");
                HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                */

            }
            else //Clear Case Session
            {
                caseInfo.CaseID = caseId;
                caseInfo.PDAPDNumber = String.Empty;
                caseInfo.ApptDate = String.Empty;
                caseInfo.Status = String.Empty;
                caseInfo.Client = String.Empty;
                caseInfo.Attorney = String.Empty;
                caseInfo.CaseJcatsNumber = String.Empty;
                caseInfo.CaseNumberAgencyID = 0;
                caseInfo.NextHearingDate = string.Empty;

                /*   identity.RemoveClaim(identity.FindFirst("CaseID"));
                   identity.AddClaim(new Claim("CaseID", "0"));

                   identity.RemoveClaim(identity.FindFirst("PDAPDNumber"));
                   identity.AddClaim(new Claim("PDAPDNumber", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("Client"));
                   identity.AddClaim(new Claim("Client", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("ApptDate"));
                   identity.AddClaim(new Claim("ApptDate", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("Status"));
                   identity.AddClaim(new Claim("Status", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("Client"));
                   identity.AddClaim(new Claim("Client", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("Attorney"));
                   identity.AddClaim(new Claim("Attorney", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("CaseJcatsNumber"));
                   identity.AddClaim(new Claim("CaseJcatsNumber", String.Empty));

                   identity.RemoveClaim(identity.FindFirst("CaseNumberAgencyID"));
                   identity.AddClaim(new Claim("CaseNumberAgencyID", "0"));



                   HttpContext.Current.GetOwinContext().Authentication.SignOut("ApplicationCookie");
                   HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                   */


            }

            CaseSummaryBarInfo.UpdateCaseInfo(caseInfo);
            return true;
        }

        public void ClearCaseStatusBar(int caseId)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

            identity.RemoveClaim(identity.FindFirst("CaseID"));
            identity.AddClaim(new Claim("CaseID", "0"));

            identity.RemoveClaim(identity.FindFirst("PDAPDNumber"));
            identity.AddClaim(new Claim("PDAPDNumber", String.Empty));

            identity.RemoveClaim(identity.FindFirst("Client"));
            identity.AddClaim(new Claim("Client", String.Empty));

            identity.RemoveClaim(identity.FindFirst("ApptDate"));
            identity.AddClaim(new Claim("ApptDate", String.Empty));

            identity.RemoveClaim(identity.FindFirst("Status"));
            identity.AddClaim(new Claim("Status", String.Empty));

            identity.RemoveClaim(identity.FindFirst("Client"));
            identity.AddClaim(new Claim("Client", String.Empty));

            identity.RemoveClaim(identity.FindFirst("Attorney"));
            identity.AddClaim(new Claim("Attorney", String.Empty));

            identity.RemoveClaim(identity.FindFirst("CaseJcatsNumber"));
            identity.AddClaim(new Claim("CaseJcatsNumber", String.Empty));

            identity.RemoveClaim(identity.FindFirst("CaseNumberAgencyID"));
            identity.AddClaim(new Claim("CaseNumberAgencyID", "0"));

            identity.RemoveClaim(identity.FindFirst("NextHearingDate"));
            identity.AddClaim(new Claim("NextHearingDate", String.Empty));

            HttpContext.Current.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);


        }
    }
}