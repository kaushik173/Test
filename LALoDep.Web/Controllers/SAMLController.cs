using ComponentSpace.SAML2;
using GAFulDep.Core.NG_sp.com_Login;
using GAFulDep.Custom;
using GAFulDep.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GAFulDep.Controllers
{
    public class SAMLController : BaseController
    {
        private readonly UserManager _userManager;
        public SAMLController(UserManager userManager)
        {
            _userManager = userManager;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            var partnerName = ConfigurationManager.AppSettings["PartnerName"];
            SAMLServiceProvider.InitiateSSO(Response, returnUrl, partnerName);
            return new EmptyResult();
        }

        public virtual ActionResult Callback(string ReturnUrl)
        {
            // Receive and process the SAML assertion contained in the SAML response.
            // The SAML response is received either as part of IdP-initiated or SP-initiated SSO.
            bool isInResponseTo;
            string partnerName;
            string authnContext;
            string userName;
            IDictionary<string, string> attributes;
            string relayState;

            SAMLServiceProvider.ReceiveSSO(
                Request,
                out isInResponseTo,
                out partnerName,
                out authnContext,
                out userName,
                out attributes,
                out relayState);

            AuthenticationManager.SignOut("ApplicationCookie");
            var errorMessage = String.Empty;
            var landingPage = String.Empty;
            AccountLoginViewModel model = new AccountLoginViewModel();
            if (!userName.StartsWith(@"cosd\"))
                userName = @"cosd\" + userName;

            model.Username = userName;

            //var ngLoginInfo = new NG_com_login_spResult(); 

            if (!string.IsNullOrEmpty(userName))
            {
                Session["ForceSAMLJCATSLogin"] = 1;
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ErrorMessage = errorMessage;
            if (!string.IsNullOrEmpty(landingPage))
            {
                return RedirectToLocal(ReturnUrl, landingPage);
            }

            return View("/Views/Home/AccessDenied.cshtml");
        }

        public virtual ActionResult Logout()
        {
            // Receive the single logout request or response.
            // If a request is received then single logout is being initiated by the identity provider.
            // If a response is received then this is in response to single logout having been initiated by the service provider.
            bool isRequest;
            string logoutReason;
            string partnerName;
            string relayState;

            SAMLServiceProvider.ReceiveSLO(
                Request,
                out isRequest,
                out logoutReason,
                out partnerName,
                out relayState);

            if (isRequest)
            {
                // Respond to the IdP-initiated SLO request indicating successful logout.
                SAMLServiceProvider.SendSLO(Response, null);
            }
            var returnUrl = LogOffHelper(0);
            return Redirect("~/" + returnUrl);
        }
        public virtual ActionResult LogOff()
        {
            var returnUrl = LogOffHelper(0);
            if (SAMLServiceProvider.CanSLO())
            {
                // Request logout at the identity provider.
                SAMLServiceProvider.InitiateSLO(Response, null, null);
                return new EmptyResult();
            }
            return Redirect("~/" + returnUrl);
        }
    }
}