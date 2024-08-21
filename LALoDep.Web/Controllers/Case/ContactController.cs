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
using LALoDep.Custom;
using LALoDep.Domain.pd_Address;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewContactInformation, PageSecurityItemID = SecurityToken.ViewContactInformation)]
        public virtual ActionResult ContactInfo()
        {
            var viewModel = new ContactInformationViewModel();

            if (UserManager.UserExtended.CaseID > 0)
            {
                viewModel.CanAddAccess = UserManager.IsUserAccessTo(SecurityToken.AddContactInformation);
                viewModel.CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.EditContactInformation);


                var pd_PersonSpParams = new pd_PersonContactGetByCaseID_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                viewModel.ContactInfoList = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetByCaseID_spResults>("pd_PersonContactGetByCaseID_sp", pd_PersonSpParams).ToList();

                viewModel.PersonList = UtilityService.ExecStoredProcedureWithResults<pd_PersonGetByCaseIDContact_spResult>("pd_PersonGetByCaseIDContact_sp", pd_PersonSpParams).Select(x => new PersonViewModel()
                {
                    PersonID = x.PersonID.Value,
                    PersonNameDisplay = x.PersonNameLast + ", " + x.PersonNameFirst
                }).ToList();

                viewModel.ContactTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(40, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                //defult display two row
                for (int i = 0; i < 2; i++)
                {
                    viewModel.ContactInfoAddList.Add(new pd_PersonContactUpdate_spParams());
                }
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.ViewContactInformation, PageSecurityItemID = SecurityToken.ViewContactInformation)]
        public virtual JsonResult ContactInfoSave(ContactInformationViewModel viewModel)
        {
            if (UserManager.IsUserAccessTo(SecurityToken.EditContactInformation))
            {
                foreach (var item in viewModel.ContactInfoList)
                {
                    var pd_PersonContactUpdate_spParams = new pd_PersonContactUpdate_spParams()
                    {
                        PersonContactID = item.PersonContactID,
                        AgencyID = item.AgencyID,
                        PersonID = item.PersonID,
                        PersonContactTypeCodeID = item.PersonContactTypeCodeID,
                        PersonContactInfo = item.PersonContactInfo,
                        RecordStateID = (int)item.RecordStateID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonContactUpdate_sp", pd_PersonContactUpdate_spParams).FirstOrDefault();
                }
            }

            if (UserManager.IsUserAccessTo(SecurityToken.AddContactInformation))
            {
                foreach (var item in viewModel.ContactInfoAddList)
                {
                    var pd_PersonContactInsert_spParams = new pd_PersonContactInsert_spParams()
                    {
                        AgencyID = null,
                        PersonID = (int)item.PersonID,
                        PersonContactTypeCodeID = (int)item.PersonContactTypeCodeID,
                        PersonContactInfo = item.PersonContactInfo,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var inseredId = UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_PersonContactInsert_sp", pd_PersonContactInsert_spParams).FirstOrDefault();
                }
            }

            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.ViewContactInformation, PageSecurityItemID = SecurityToken.DeleteContactInformation)]
        public virtual JsonResult ContactInfoDelete(string id)
        {
            var pd_PersonContactDelete_spParams = new pd_PersonContactDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "PersonContact",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonContactDelete_sp", pd_PersonContactDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }
    }
}