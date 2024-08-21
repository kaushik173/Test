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
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.AddEditCountyCounsel;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AKAsPages, PageSecurityItemID = SecurityToken.ViewAKA)]
        public virtual ActionResult AKAs()
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
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AKAsPages, PageSecurityItemID = SecurityToken.ViewAKA)]
        public virtual JsonResult PersonNameGetAKA(string id)
        {
            var personID = id.ToDecrypt().ToInt();
            var pd_PersonNameGetAKAByPersonID_spParams = new pd_PersonNameGetAKAByPersonID_spParams()
            {
                PersonID = personID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var personNameAKAList = UtilityService.ExecStoredProcedureWithResults<pd_PersonNameGetAKAByPersonID_spResult>("pd_PersonNameGetAKAByPersonID_sp", pd_PersonNameGetAKAByPersonID_spParams)
                .Select(x => new
                {
                    x.PreferredFlag,
                    x.PersonNameLast,
                    x.PersonNameFirst,
                    x.PersonNameID,
                    x.PersonID,
                    EncryptedPersonNameID = x.PersonNameID.ToEncrypt(),
                    EncryptedPersonID = x.PersonID.ToEncrypt()
                }).ToList();
            var total = personNameAKAList.Count();

            return Json(new DataTablesResponse(0, personNameAKAList, total, total));
        }


        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AKAsPages, PageSecurityItemID = SecurityToken.ViewAKA)]
        public virtual ActionResult UpdatedPreferredName(int personId, int personNameId)
        {

            UtilityService.ExecStoredProcedureWithoutResults("pd_PersonNamePreferredUpdate_sp", new pd_PersonNamePreferredUpdate_spParams
            {
                PersonID = personId,
                PersonNameID = personNameId,
                UserID = UserManager.UserExtended.UserID,
            });

            return Json(new { isSuccess = true });
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AKAsPages, PageSecurityItemID = SecurityToken.DeleteAKA)]
        public virtual JsonResult PersonNameGetAKADelete(string id)
        {
            var pd_PersonNameDelete_spParams = new pd_PersonNameDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "PersonName",
                RecordStateID = 10,
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonNameDelete_sp", pd_PersonNameDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AKAsPages)]
        public virtual ActionResult AKAsAddEdit(string id, string nameID, string name)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.AddAKA) && !UserManager.IsUserAccessTo(SecurityToken.EditAKA))
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            var viewModel = new AKAsAddEditViewModel();

            var personId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                personId = id.ToDecrypt().ToInt();
            }
            viewModel.PersonID = personId;
            if (!nameID.IsNullOrEmpty())
            {
                //Edit
                ViewBag.DisplayTitle = "Edit AKA" + (string.IsNullOrEmpty(name) ? string.Empty : " for <b>" + name + "</b>");
                var personNmaeInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonNameGet_spResult>("pd_PersonNameGet_sp", new pd_PersonNameGet_spParams()
                {
                    PersonNameID = nameID.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();
                if (personNmaeInfo != null)
                {
                    viewModel.PersonID = personNmaeInfo.PersonID.Value;
                    viewModel.PersonNameID = personNmaeInfo.PersonNameID.Value;
                    viewModel.PersonNameFirst = personNmaeInfo.PersonNameFirst;
                    viewModel.PersonNameLast = personNmaeInfo.PersonNameLast;
                    viewModel.PersonNameTypeCodeID = personNmaeInfo.PersonNameTypeCodeID.Value;
                    viewModel.PersonNameMiddle = personNmaeInfo.PersonNameMiddle;
                    viewModel.RecordStateID = personNmaeInfo.RecordStateID.Value;
                    viewModel.AgencyID = personNmaeInfo.AgencyID.Value;
                }
            }
            else
            {
                //Add
                ViewBag.DisplayTitle = "Add AKA" + (string.IsNullOrEmpty(name) ? string.Empty : " for <b>" + name + "</b>"); ;
            }

            return View(viewModel);
        }

        public virtual JsonResult AKAsAddEditSave(AKAsAddEditViewModel viewModel)
        {
            if (viewModel.PersonNameID.HasValue)
            {
                //Edit
                var pd_PersonNameUpdate_spParams = new pd_PersonNameUpdate_spParams()
                {
                    PersonNameID = viewModel.PersonNameID.Value,
                    AgencyID = viewModel.AgencyID,
                    PersonID = viewModel.PersonID,
                    PersonNameFirst = viewModel.PersonNameFirst,
                    PersonNameLast = viewModel.PersonNameLast,
                    PersonNameTypeCodeID = viewModel.PersonNameTypeCodeID,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonNameUpdate_sp", pd_PersonNameUpdate_spParams).FirstOrDefault();
            }
            else
            {
                var personNameId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp", new pd_PersonNameInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,
                    PersonID = viewModel.PersonID,
                    PersonNameFirst = viewModel.PersonNameFirst,
                    PersonNameLast = viewModel.PersonNameLast,
                    PersonNameTypeCodeID = 3201,
                }).FirstOrDefault();
            }
            return Json(new { isSuccess = true });
        }
    }
}