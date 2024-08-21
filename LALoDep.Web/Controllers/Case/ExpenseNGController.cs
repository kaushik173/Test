using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using LALoDep.Models.Case;
using LALoDep.Domain.Expense;
using Omu.ValueInjecter;
using LALoDep.Domain;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseExpenseNG, IsCasePage = true)]

        public virtual ActionResult ExpenseNG(string id)
        {

            var expenseId = id.ToDecrypt().ToInt();
            var model = new ExpenseNGViewModel
            {
                EligibleList = UtilityService.ExecStoredProcedureWithResults<Expense_GetCodes_spResult>(
                        "Expense_GetCodes_sp",
                        new Expense_GetCodes_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            CodeTypeCodeID = 2000,
                            ExpenseID = expenseId,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID
                        }).Select(o => new SelectListItem { Text = o.CodeDisplay, Value = o.CodeID.ToString(), Selected = o.Selected.ToInt() == 1 }).ToList(),
                StatusList = UtilityService.ExecStoredProcedureWithResults<Expense_GetCodes_spResult>(
                        "Expense_GetCodes_sp",
                        new Expense_GetCodes_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            CodeTypeCodeID = 1605,
                            ExpenseID = expenseId,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID
                        }).Select(o => new SelectListItem { Text = o.CodeDisplay, Value = o.CodeID.ToString(), Selected = o.Selected.ToInt() == 1 }).ToList(),
                TypeList = UtilityService.ExecStoredProcedureWithResults<Expense_GetCodes_spResult>(
                        "Expense_GetCodes_sp",
                        new Expense_GetCodes_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            CodeTypeCodeID = 1600,
                            ExpenseID = expenseId,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID
                        }).Select(o => new SelectListItem { Text = o.CodeDisplay, Value = o.CodeID.ToString(), Selected = o.Selected.ToInt() == 1 }).ToList(),

                ExpenseList = UtilityService.ExecStoredProcedureWithResults<Expense_GetByCaseID_spResult>(
                        "Expense_GetByCaseID_sp",
                        new Expense_GetByCaseID_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,

                        }).ToList(),
                StaffMemberList = UtilityService.ExecStoredProcedureWithResults<Expense_GetStaffMembers_spResult>(
                        "Expense_GetStaffMembers_sp",
                        new Expense_GetStaffMembers_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,

                            ExpenseID = expenseId,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID
                        }).Select(o => new SelectListItem { Text = o.PersonDisplay, Value = o.PersonID.ToString(), Selected = o.Selected.ToInt() == 1 }).ToList(),

            };
            model.ExpenseDate = DateTime.Now.ToString("d");
            if (expenseId > 0)
            {
                var expenseGet = UtilityService.ExecStoredProcedureWithResults<Expense_Get_spResult>(
                        "Expense_Get_sp",
                        new Expense_Get_spParams()
                        {
                            UserID = UserManager.UserExtended.UserID,
                            ExpenseID = expenseId,

                        }).FirstOrDefault();
                if (expenseGet != null)
                    model.InjectFrom(expenseGet);
            }

            model.IsAdmin = UserManager.UserExtended.SystemAdminFlag == 1;
            return View(model);
        }

        [HttpPost]
        public virtual JsonResult ExpenseNGSave(ExpenseNGViewModel model)
        {


            if (model.ExpenseID > 0)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("ExpenseIUD_sp", new ExpenseIUD_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    ExpenseID = model.ExpenseID,
                    IUD = "UPDATE",
                    ExpenseAmount = model.Amount,
                    ExpenseIVeEligibleCodeID = model.EligibleCodeID,
                    ExpenseNote = model.Note,
                    ExpenseVendorName = model.VendorName,
                    ExpenseDate = model.ExpenseDate.ToDateTimeNullableValue(),
                    ExpenseTypeCodeID = model.ExpenseTypeCodeID,
                    ExpenseRequestedByPersonID = model.StaffPersonID,
                    ExpenseStatusCodeID = model.CurrentStatusCodeID,
                    ExpenseNoteAdmin = model.AdminNote,
                    CaseID = model.CaseID,
                    AgencyID = model.AgencyID,
                });
            }
            else
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("ExpenseIUD_sp", new ExpenseIUD_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    IUD = "ADD",
                    ExpenseAmount = model.Amount,
                    ExpenseIVeEligibleCodeID = model.EligibleCodeID,
                    ExpenseNote = model.Note,
                    ExpenseVendorName = model.VendorName,
                    ExpenseDate = model.ExpenseDate.ToDateTimeNullableValue(),
                    ExpenseTypeCodeID = model.ExpenseTypeCodeID,
                    ExpenseRequestedByPersonID = model.StaffPersonID,
                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,

                });
            }



            return Json(new { Status = "Done" });
        }

        [HttpPost]

        public virtual ActionResult ExpenseNGDelete(int id)
        {



            UtilityService.ExecStoredProcedureWithoutResultADO("ExpenseIUD_sp", new ExpenseIUD_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                ExpenseID = id,
                IUD = "DELETE"

            });



            return Json(new { Status = "Done" });
        }

        public virtual ActionResult ExpenseNGStatusHistory(int id)
        {


            var data = UtilityService.ExecStoredProcedureWithResults<Expense_GetStatusHistory_spResult>(
                         "Expense_GetStatusHistory_sp",
                         new Expense_GetStatusHistory_spParams()
                         {
                             ExpenseID = id,
                             UserID = UserManager.UserExtended.UserID,

                         }).ToList();
         
            return View(data);
        }
        [ChildActionOnly]
        public virtual ActionResult ExpenseNGHeader(int? id)
        {

            var model = UtilityService.ExecStoredProcedureWithResults<Expense_GetHeader_spResult>(
                            "Expense_GetHeader_sp",
                            new Expense_GetHeader_spParams()
                            {
                                ExpenseID = id,
                                UserID = UserManager.UserExtended.UserID,

                            }).FirstOrDefault();
            if (model != null)
                model.ExpenseID = id.ToInt();
            return View(model);
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
           PageSecurityItemID = SecurityToken.AttachFileView)]
        public virtual ActionResult ExpenseNGFiles(string id)
        {
            if (id.ToDecrypt().ToInt() == 0) 
                return RedirectToAction("ExpenseNG");

          var expenseId=  ViewBag.ExpenseID = id.ToDecrypt().ToInt();

            var model = new AttachPathViewModel
            {
                CaseFileGetPathByCaseList =
                     UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                         "CaseFileGetPathByCaseID_sp",
                         new CaseFileGetPathByCaseID_spParams
                         {
                             BatchLogJobID = Guid.NewGuid(),
                             CaseID = UserManager.UserExtended.CaseID,
                             UserID = UserManager.UserExtended.UserID,


                         }).ToList(),
                RoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                     "pd_RoleGetByCaseIDChildRespondent_sp",
                     new pd_RoleGetByCaseIDChildRespondent_spParams
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,


                     }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.RoleID.ToString() }),
                CategoryList =
                     UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(900)
                         .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() })
                         .ToList(),

                FileDate = DateTime.Now.ToShortDateString()
            };
            model.UseGoogleDriveUpload = model.CaseFileGetPathByCaseList.Any(o => o.UseGoogleDocsFlag.HasValue && o.UseGoogleDocsFlag.Value == 1) ? 1 : 0;
            var caseFileGetPath = model.CaseFileGetPathByCaseList.FirstOrDefault();
            if (caseFileGetPath != null)
            {
                if (caseFileGetPath.UseGoogleDocsFlag.HasValue && caseFileGetPath.UseGoogleDocsFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
                    var parentFolderId = "";
                    if (environment == "Test")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_PROD;

                    }
                    if (!parentFolderId.IsNullOrEmpty())
                        model.UseGoogleDriveUpload = caseFileGetPath.UseGoogleDocsFlag.Value;

                }

                if (caseFileGetPath.SharePoint_UseFlag.HasValue && caseFileGetPath.SharePoint_UseFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];

                    if (environment == "Test" || environment == "Dev")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_PROD;

                    }

                    model.SharePoint_UseFlag = caseFileGetPath.SharePoint_UseFlag.Value;
                }
            }

            var settings = UtilityFunctions.JcatsNGConfigGetAll(UserManager.UserExtended.CaseID);

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes))
                ViewBag.AttachFileTypes = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes).JcatsNGConfigValue;
            else
                ViewBag.AttachFileTypes = "gif|jpe?g|png|pdf|txt|docx|doc|xls|xlsx|csv|ppt|pptx";

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize))
                ViewBag.AttachFileMaxSize = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize).JcatsNGConfigValue;
            else
                ViewBag.AttachFileMaxSize = "15000000";
            
            return View(model);

        }

    }
}