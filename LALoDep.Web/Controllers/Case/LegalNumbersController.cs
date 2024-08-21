using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using LALoDep.Domain.pd_Role;
using DataTables.Mvc;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Code;
using LALoDep.Core.Enums;
using LALoDep.Models;
using LALoDep.Custom;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.LegalNumberPage, PageSecurityItemID = SecurityToken.ViewLegalNumber)]
        public virtual ActionResult LegalNumber()
        {
            if (UserManager.UserExtended.CaseID > 0)
            {
                var roleList =
                    UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                        "pd_RoleGetByCaseIDChildRespondent_sp", new pd_RoleGetByCaseIDChildRespondent_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        }).ToList();
                return View(roleList);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.LegalNumberPage, PageSecurityItemID = SecurityToken.ViewLegalNumber)]
        public virtual JsonResult LegalNumberForPerson(string id)
        {
            var personID = id.ToDecrypt().ToInt();
            var pd_LegalNumberGetByPersonID_spParams = new pd_LegalNumberGetByPersonID_spParams()
            {
                PersonID = personID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var personLegalNumberList = UtilityService.ExecStoredProcedureWithResults<pd_LegalNumberGetByPersonID_spResult>("pd_LegalNumberGetByPersonID_sp", pd_LegalNumberGetByPersonID_spParams).Select(x => new
            {
                Type = x.LegalNumberTypeCodeValue,
                Number = x.LegalNumberEntry,
                EncryptedID = x.LegalNumberID.ToEncrypt(),
                EncryptedPersonID = x.PersonID.ToEncrypt(),
                x.LegalNumberComment
            }).ToList();
            var total = personLegalNumberList.Count();

            return Json(new DataTablesResponse(0, personLegalNumberList, total, total));
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.LegalNumberPage, PageSecurityItemID = SecurityToken.DeleteLegalNumber)]
        public virtual JsonResult LegalNumberDeleteForPerson(string id)
        {
            var legalNumberID = id.ToDecrypt().ToInt();
            var pd_LegalNumberDelete_spParams = new pd_LegalNumberDelete_spParams()
            {
                ID = legalNumberID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "LegalNumber",
                RecordStateID = 10,
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_LegalNumberDelete_sp", pd_LegalNumberDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.LegalNumberPage, PageSecurityItemID = SecurityToken.AddLegalNumber)]
        public virtual ActionResult LegalNumberAdd(string id, string pageID, string name)
        {
            ViewBag.PageID = string.Empty;
            if (!string.IsNullOrEmpty(pageID))
                ViewBag.PageID = pageID.ToDecrypt().ToInt();
            var model = GetLegalNumberAddEditViewModel(0);
            model.PersonID = id.ToDecrypt().ToInt();

            ViewBag.PersonName = name;
            return View("LegalNumberAddEdit", model);
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.LegalNumberPage, PageSecurityItemID = SecurityToken.EditLegalNumber)]
        public virtual ActionResult LegalNumberEdit(string id, string pageID, string name)
        {
            ViewBag.PageID = string.Empty;
            if (!string.IsNullOrEmpty(pageID))
                ViewBag.PageID = pageID.ToDecrypt().ToInt();
            var legalNumberID = id.ToDecrypt().ToInt();
            var model = GetLegalNumberAddEditViewModel(legalNumberID);

            ViewBag.PersonName = name;
            return View("LegalNumberAddEdit", model);
        }

        public virtual JsonResult LegalNumberAddEdit(LegalNumberAddEditViewModel viewModel)
        {
             


                var result = UtilityService.ExecStoredProcedureWithResults<pd_LegalNumberValidateRelatedNumber_spResult>("pd_LegalNumberValidateRelatedNumber_sp", new pd_LegalNumberValidateRelatedNumber_spParams()
                {

                    CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,

                    LegalNumberTypeCodeID = viewModel.LegalNumberTypeCodeID.Value,
                    LegalNumberEntry = viewModel.LegalNumberEntry,

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID,
                }).FirstOrDefault();
                if (result != null)
                {
                    if (result.ErrorCode > 0)
                    {
                        return Json(new { isSuccess = false, ErrorMessage = result.ErrorMessage });
                    }
                }
            


            if (viewModel.LegalNumberID > 0)
            {
                //update 
                var pd_LegalNumberUpdate_spParams = new pd_LegalNumberUpdate_spParams()
                {
                    LegalNumberID = viewModel.LegalNumberID,
                    AgencyID = null,
                    PersonID = viewModel.PersonID,
                    LegalNumberTypeCodeID = viewModel.LegalNumberTypeCodeID.Value,
                    LegalNumberEntry = viewModel.LegalNumberEntry,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                if (!viewModel.LegalNumberComment.IsNullOrEmpty())
                    pd_LegalNumberUpdate_spParams.LegalNumberComment = viewModel.LegalNumberComment;
                else
                    pd_LegalNumberUpdate_spParams.LegalNumberComment = "";
                UtilityService.ExecStoredProcedureWithResults<object>("pd_LegalNumberUpdate_sp", pd_LegalNumberUpdate_spParams).FirstOrDefault();
            }
            else
            {
                var pd_LegalNumberInsert_spParams = new pd_LegalNumberInsert_spParams()
                {
                    PersonID = viewModel.PersonID,
                    LegalNumberTypeCodeID = viewModel.LegalNumberTypeCodeID.Value,
                    LegalNumberEntry = viewModel.LegalNumberEntry,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LegalNumberComment = viewModel.LegalNumberComment
                };
                var insertedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_LegalNumberInsert_sp", pd_LegalNumberInsert_spParams).ToList();
            }
            return Json(new { isSuccess = true });
        }
        private LegalNumberAddEditViewModel GetLegalNumberAddEditViewModel(int legalNumberID)
        {
            var viewModel = new LegalNumberAddEditViewModel();

            if (legalNumberID > 0)
            {
                //edit Mode
                var pd_LegalNumberGet_spParams = new pd_LegalNumberGet_spParams()
                {
                    LegalNumberID = legalNumberID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var legalNumberInfo = UtilityService.ExecStoredProcedureWithResults<pd_LegalNumberGet_spResult>("pd_LegalNumberGet_sp", pd_LegalNumberGet_spParams).FirstOrDefault();
                if (legalNumberInfo != null)
                {
                    viewModel.PersonID = legalNumberInfo.PersonID;
                    viewModel.AgencyID = legalNumberInfo.AgencyID;
                    viewModel.LegalNumberEntry = legalNumberInfo.LegalNumberEntry;
                    viewModel.LegalNumberID = legalNumberInfo.LegalNumberID;
                    viewModel.RecordStateID = legalNumberInfo.RecordStateID;
                    viewModel.LegalNumberTypeCodeID = legalNumberInfo.LegalNumberTypeCodeID;
                    viewModel.LegalNumberComment = legalNumberInfo.LegalNumberComment;
                }
            }


            viewModel.LegalNumberTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(4, includeCodeId: viewModel.LegalNumberTypeCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            return viewModel;
        }
    }
}