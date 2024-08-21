//using LALoDep.Domain.NG_com;
//using LALoDep.Domain.PD_JcatsUser;
//using LALoDep.Domain.Services;
//using Jcats.CA.UI.Custom;
//using LALoDep.Core.Custom.Extensions;
//using LALoDep.Core.Custom.Utility;
//using LALoDep.Core.Enums;
//using LALoDep.Core.NG_pd_login_spParams;
//using LALoDep.Core.NG_pd_login_spResult;
//using LALoDep.Core.NG_sp.com_Login;
//using LALoDep.Custom;
//using LALoDep.Models;
//using LALoDep.Models.Api;
//using Microsoft.Owin.Security;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Web;
//using System.Web.Http;

//namespace LALoDep.Controllers.Api
//{
//    [Authorize]
//    public class AccountController : ApiController
//    {
//        private IUtilityService UtilityService;
//        private readonly UserManager UserManager;
//        public AccountController(UserManager _userManager, IUtilityService _utilityService)
//        {
//            UserManager = _userManager;
//            UtilityService = _utilityService;
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        public LoginResult Login(LoginModel model)
//        {
//            LoginResult result = new LoginResult() { IsSuccess = true };


//            var jcatsUser = UtilityService.ExecStoredProcedureWithResults<NG_com_newlogin_spResult>("NG_pd_newlogin_sp", new NG_com_newlogin_spParams { UserName = model.Username }).FirstOrDefault();
//            if (jcatsUser == null)
//            {
//                result.Message = "Invalid Username or Password.";
//                result.IsSuccess = false;
//            }
//            else if (jcatsUser != null && jcatsUser.NG_JcatsUserPassword.IsNullOrEmpty())
//            {
//                result.Message = "Your JCATS NG account has not been activated. Please contact your JCATS administrator.";
//                result.IsSuccess = false;
//            }
//            else if (!PasswordHash.ValidatePassword(model.Password, jcatsUser.NG_JcatsUserPassword))
//            {
//                result.Message = "Invalid Username or Password.";
//                result.IsSuccess = false;
//            }
//            else
//            {
//                var guid = Guid.NewGuid();
//                var browser = HttpContext.Current.Request.Browser;

//                var nG_pd_login_spParams = new NG_pd_login_spParams
//                {
//                    UserName = model.Username,
//                    Password = "success",
//                    AdminFlag = 0,
//                    LoginKey = guid,
//                    ClientInfo = string.Format("{0}-{1}-{2}-{3}", browser?.Platform, browser?.Type, browser?.Version, browser?.MajorVersion),
//                    ClientAddress = Utility.GetClientIpAddress(),//request.ServerVariables["REMOTE_ADDR"] ?? request.UserHostAddress,
//                    ClientHost = Dns.GetHostName(),
//                    BatchLogJobID = Guid.NewGuid(),
//                    RecordStateID = 1,
//                };

//                var login = UtilityService.ExecStoredProcedureWithResults<NG_pd_login_spResult>("NG_pd_Login_sp", nG_pd_login_spParams).FirstOrDefault();
//                switch (login.LoginSuccessful)
//                {
//                    case LoginStatus.InvalidKey:
//                        result.Message = "Another login session has begun using your user name and password and has expired your current session. If you feel this is in error, please change your password immediately.";
//                        result.IsSuccess = false;
//                        break;
//                    case LoginStatus.TimedOut:
//                        result.Message = "Your login has expired due to an inactive session.";
//                        result.IsSuccess = false;
//                        break;
//                    case LoginStatus.AccountLocked:
//                        result.Message = "The account is locked.  Please contact your JCATS administrator.";
//                        result.IsSuccess = false;
//                        break;
//                    case LoginStatus.InvalidUser:
//                        result.Message = "Invalid User Name and/or Password.";
//                        result.IsSuccess = false;
//                        break;
//                }


//                if (result.IsSuccess)
//                {


//                    var identity = new ClaimsIdentity(Startup.OAuthOptions.AuthenticationType);
//                    identity.AddClaim(new Claim("ApiIdentity", "1"));
//                    identity.AddClaim(new Claim("UserID", login.UserID.Value.ToString()));
//                    identity.AddClaim(new Claim("Guid", login.LoginKey));
//                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, login.UserID.Value.ToString()));


//                    var user = UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGet_spResults>("pd_JcatsUserGet_sp", new pd_JcatsUserGet_spParams
//                    {
//                        JcatsUserID = login.UserID.Value,
//                        UserID = login.UserID.Value,
//                        BatchLogJobID = Guid.NewGuid(),
//                    }).FirstOrDefault();

//                    identity.AddClaim(new Claim(ClaimTypes.Surname, user.PersonNameLast));
//                    identity.AddClaim(new Claim(ClaimTypes.GivenName, user.PersonNameFirst));
//                    identity.AddClaim(new Claim(ClaimTypes.Name, user.JcatsUserLoginName));
//                    identity.AddClaim(new Claim("PersonID", user.PersonID.ToString()));
//                    identity.AddClaim(new Claim("AgencyID", user.AgencyID.ToString()));
//                    identity.AddClaim(new Claim("BranchID", user.AgencyID.ToString()));
//                    identity.AddClaim(new Claim("JcatsGroupID", user.JcatsGroupID.ToString()));
//                    identity.AddClaim(new Claim("SystemAdminFlag", user.SystemAdminFlag.ToString()));
//                    identity.AddClaim(new Claim(ClaimTypes.Role, user.SystemAdminFlag == 1 ? Role.Admin.ToString() : Role.Standard.ToString()));

//                    identity.AddClaim(new Claim("AttorneyRoleID", 0.ToString()));
//                    identity.AddClaim(new Claim("SessionTimeOut", "2400"));


//                    AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
//                    var currentUtc = DateTime.UtcNow;
//                    ticket.Properties.IssuedUtc = currentUtc;
//                    //ticket.Properties.ExpiresUtc = currentUtc.Add(TimeSpan.FromMinutes(30));                    
//                    result.UserName = user.PersonNameFirst + " " + user.PersonNameLast;
//                    result.AccessToken = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);

//                    //HttpClient client = new HttpClient();
//                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strAccessToken);
//                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Startup.OAuthOptions.AuthenticationType, strAccessToken);
//                }

//            }
//            return result;
//        }
               

//        [HttpPost]
//        public ApiResult<NG_DeviceTokenIUD_spResult> AddDeviceToken(UpdateDeviceTokenModel model)
//        {
//            var result = new ApiResult<NG_DeviceTokenIUD_spResult> { IsSuccess = true };
//            try
//            {
//                if (model.DeleteDeviceTokenID > 0)
//                {
//                    var deleted = UtilityService.ExecStoredProcedureWithResults<NG_DeviceTokenIUD_spResult>("NG_DeviceTokenIUD_sp", new NG_DeviceTokenIUD_spParams
//                    {
//                        IUD = "DELETE",
//                        DeviceTokenID = model.DeleteDeviceTokenID,
//                        BatchLogJobID = Guid.NewGuid(),
//                        UserID = UserManager.UserExtended.UserID,
//                    }).FirstOrDefault();
//                }

//                result.Result = UtilityService.ExecStoredProcedureWithResults<NG_DeviceTokenIUD_spResult>("NG_DeviceTokenIUD_sp", new NG_DeviceTokenIUD_spParams
//                {
//                    IUD = "INSERT",
//                    BatchLogJobID = Guid.NewGuid(),
//                    UserID = UserManager.UserExtended.UserID,
//                    DeviceType = model.DeviceType,
//                    RecordStateID = 1,
//                    Token = model.NewToken
//                }).FirstOrDefault();
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//                result.IsSuccess = false;
//            }

//            return result;
//        }
//    }
//}
