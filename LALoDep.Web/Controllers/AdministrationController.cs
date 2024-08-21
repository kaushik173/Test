using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.PD_JcatsUser;
using LALoDep.Domain.Services;
using LALoDep.Core.Enums;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Models.Inquiry;
using LALoDep.Core.Custom.Extensions;
using Omu.ValueInjecter;
using LALoDep.Domain;
using LALoDep.Core.NG_sp.com_Login;
using LALoDep.Core.PasswordHasher;
using LALoDep.Domain.com_Jcats;
using System.IO;

namespace LALoDep.Controllers.Administration
{
    [AuthenticationAuthorize]
    public partial class AdministrationController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public AdministrationController(IUtilityService utilityService,UserManager userManager)
        {
            UtilityService = utilityService;
            UserManager = userManager;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.ChangePasswordPage, PageSecurityItemID = SecurityToken.ViewChangePassword)]
        public virtual ActionResult ChangePassword()
        {
            var viewModel = new AdminChangePasswordViewModel();
            var pd_JcatsUserGet_spParams = new pd_JcatsUserGet_spParams
            {
                JcatsUserID = UserManager.UserExtended.UserID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };

            var userInfo= UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGet_spResults>("pd_JcatsUserGet_sp", pd_JcatsUserGet_spParams).First();
            if (userInfo!=null)
            {
                viewModel.JcatsUserLoginName = userInfo.JcatsUserLoginName;
            }
            return View(MVC.Administration.Views.ChangePassword, viewModel);
        }

        [HttpPost]
        public virtual JsonResult ChangePassword(AdminChangePasswordViewModel changePasswordViewModel)
        {
            string errorMessage = string.Empty;
            string succesMessage = string.Empty;
            var ng_com_newlogin_spParams = new NG_com_newlogin_spParams()
            {
                UserName = changePasswordViewModel.JcatsUserLoginName
            };
            var jcatsUser = UtilityService.ExecStoredProcedureWithResults<NG_com_newlogin_spResult>("NG_pd_newlogin_sp", ng_com_newlogin_spParams).FirstOrDefault();
            if (jcatsUser != null)
            {
                #region for Change Password
                if (!string.IsNullOrEmpty(changePasswordViewModel.CurrentPassword))
                {
                    var oldPasswordVerified = PasswordHash.ValidatePassword(changePasswordViewModel.CurrentPassword, jcatsUser.NG_JcatsUserPassword);
                    if (oldPasswordVerified)
                    {
                        var changePassword = UtilityService.ExecStoredProcedureWithResults<NG_pd_ChangePassword_spResult>("NG_pd_ChangePassword_sp", new NG_pd_ChangePassword_spParams()
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            ChangeUserID = UserManager.UserExtended.UserID,
                            CurrentPassword = changePasswordViewModel.CurrentPassword,
                            NewPassword = changePasswordViewModel.NewPassword,
                            ResetFlag = 0,
                            UserID = UserManager.UserExtended.UserID

                        }).FirstOrDefault();
                        if (changePassword != null)
                        {
                            if (changePassword.Status == "SUCCESS")
                            {
                                if (!UserManager.UpdatePassword(changePasswordViewModel.JcatsUserLoginName,
                                changePasswordViewModel.NewPassword))
                                {
                                    errorMessage = "Change password faild";
                                }
                                else
                                {
                                    succesMessage = "Password changed successfully!";
                                }
                            }
                            else
                            {
                                errorMessage = changePassword.StatusMessage;
                            }
                        }

                    }
                    else
                    {
                        errorMessage = "Incorrect Password";
                    }
                }
                #endregion for Change Password
            }
            else
            {
                errorMessage = "Incorrect Password";
            }
            return Json(new { URL = MVC.Administration.Name + "/" + MVC.Administration.ActionNames.ChangePassword, errorMessage, succesMessage }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult Download(string file)
        {
            string fullPath = UtilityFunctions.GetDocumentDownloadFolderPath() + file;


            if (UserManager.UserExtended.PrintDocumentOn == "NewWindow" && Path.GetExtension(fullPath).Contains("pdf"))
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                return File(fullPath, "application/pdf");
            }
            if (System.IO.File.Exists(fullPath))
            {

                return File(fullPath, "application/force-download", file);
            }
            else
            {
                return Content("File not found");
            }

        }
    }
}