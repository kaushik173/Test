using LALoDep.Domain.Agency;
using LALoDep.Domain.IVeActivity;
using LALoDep.Domain.TitleIVe;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController
    {

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AgencyGroup, PageSecurityItemID = SecurityToken.AgencyGroup)]
        public virtual ActionResult AgencyGroup()
        {
            var viewModel = new AgencyGroupViewModel
            {
                AgencyGroupList = UtilityService.ExecStoredProcedureWithResults<AgencyGetGroupByUserID_spResult>("AgencyGetGroupByUserID_sp",
                                    new AgencyGetGroupByUserID_sppParams
                                    {
                                        SortOption = "AgencyGroup",
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid(),
                                    }).Select(x => new SelectListItem { Text = x.AgencyGroup, Value = x.AgencyGroupID.ToString() }).ToList()
            };
            viewModel.CountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeCountyDropDown_spResult>("TitleIVeCountyDropDown_sp", new TitleIVeCountyDropDown_spParams { UserID = UserManager.UserExtended.UserID })
                                     .Select(x => new SelectListItem() { Text = x.AgencyCounty, Value = x.AgencyCountyID.ToString() });

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult GetAgencyGroup(int? id, int? agencyCountyId)
        {
            var viewModel = new AgencyGroupViewModel();

            if (id > 0)
            {
                viewModel.CountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeCountyDropDown_spResult>("TitleIVeCountyDropDown_sp", new TitleIVeCountyDropDown_spParams { UserID = UserManager.UserExtended.UserID })
                                           .Select(x => new SelectListItem() { Text = x.AgencyCounty, Value = x.AgencyCountyID.ToString() });

                viewModel.AgencyGroupID = id;
                viewModel.AgencyGroupCountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeAgencyGroupGet_spResult>("TitleIVeAgencyGroupGet_sp",
                                   new TitleIVeAgencyGroupGet_spParams
                                   {
                                       AgencyGroupID = id,
                                       UserID = UserManager.UserExtended.UserID,
                                       AgencyCountyID = agencyCountyId
                                   }).ToList();

                var agencyGroup = viewModel.AgencyGroupCountyList.FirstOrDefault();
                if (agencyGroup != null)
                {
                    viewModel.InjectFrom(agencyGroup);
                }
            }

            return PartialView("_AgencyGroupPartial", viewModel);
        }


        [HttpPost]
        public virtual ActionResult SaveAgencyGroup(AgencyGroupViewModel viewModel)
        {
            UtilityService.ExecStoredProcedureWithoutResults("TitleIVeAgencyGroupInsertUpdate_sp",
                                   new TitleIVeAgencyGroupInsertUpdate_spParams
                                   {
                                       AgencyCountyID = viewModel.AgencyCountyID,
                                       UserID = UserManager.UserExtended.UserID,
                                       AgencyGroupID = viewModel.AgencyGroupID,
                                       AgencyGroup = viewModel.AgencyGroup,
                                       AgencyGroupAbbreviation = viewModel.AgencyGroupAbbreviation,
                                       AgencyGroupAddressName = viewModel.AgencyGroupAddressName,
                                       AgencyGroupAddressStreet = viewModel.AgencyGroupAddressStreet,
                                       AgencyGroupAddressCSZ = viewModel.AgencyGroupAddressCSZ,
                                       AgencyGroupAddressPhone = viewModel.AgencyGroupAddressPhone,
                                       AgencyGroupSCCInvoiceIdentifier = viewModel.AgencyGroupSCCInvoiceIdentifier,
                                       AgencyGroupContractNumber = viewModel.AgencyGroupContractNumber,
                                       TitleIVeStartDate = viewModel.TitleIVeStartDate.ToDateTimeValue(),
                                       TitleIVeEndDate = viewModel.TitleIVeEndDate.ToDateTimeValue(),
                                       StandardAgreementNbr = viewModel.StandardAgreementNbr,
                                       DirectlyContractWithCourt = viewModel.DirectlyContractWithCourt.ToBoolean(),
                                       FederalIDNumber = viewModel.FederalIDNumber,
                                       PersonWhoWillPrepareInvoice = viewModel.PersonWhoWillPrepareInvoice,
                                       PrepareInvoicePersonTitle = viewModel.PrepareInvoicePersonTitle,
                                       PrepareInvoicePersonPhone = viewModel.PrepareInvoicePersonPhone,
                                       PrepareInvoicePersonEmail = viewModel.PrepareInvoicePersonEmail,
                                       NumberOfSocialWorkers = viewModel.NumberOfSocialWorkers,
                                       NumberOfInvestigators = viewModel.NumberOfInvestigators,
                                       NumberOfParalegals = viewModel.NumberOfParalegals,
                                       NumberofAdminAssistants = viewModel.NumberofAdminAssistants,
                                       UseWorkHoursForActivityLog = viewModel.IsUseWorkHoursForActivityLog ? 1 : 0,
                                       TitleIVeAgencyGroupID = viewModel.TitleIVeAgencyGroupID,
                                       IsPanelAgency = viewModel.IsPanelAgency,
                                       FullReviewMonth1 = viewModel.FullReviewMonth1,
                                       FullReviewMonth2 = viewModel.FullReviewMonth2,
                                       IsStreamliner = viewModel.IsStreamliner
                                   });

            return Json(new { isSuccess = true });
        }


        public virtual ActionResult AgencyGroupAllocation(string id)
        {
            var viewModel = new AgencyGroupAllocationViewModel();
            viewModel.AgencyGroupID = id.ToDecrypt().ToInt();
            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeAgencyGroupCountyAllocationGet_sp", new TitleIVeAgencyGroupCountyAllocationGet_spParams
            {
                AgencyGroupID = viewModel.AgencyGroupID,
                UserID = UserManager.UserExtended.UserID
            });
            if (jsonData != "")
                viewModel.TitleIVeAgencyGroupCountyAllocationList = JsonConvert.DeserializeObject<List<TitleIVeAgencyGroupCountyAllocationGet_spResult>>(jsonData);

            viewModel.TitleIVeAgencyGroupCountyAllocationList.Add(new TitleIVeAgencyGroupCountyAllocationGet_spResult { TitleIVeAgencyGroupCountyAllocationID = 0, AgencyGroupID = viewModel.AgencyGroupID, RecordStateID = 1 });
            viewModel.TitleIVeAgencyGroupCountyAllocationList.Add(new TitleIVeAgencyGroupCountyAllocationGet_spResult { TitleIVeAgencyGroupCountyAllocationID = 0, AgencyGroupID = viewModel.AgencyGroupID, RecordStateID = 1 });
            viewModel.TitleIVeAgencyGroupCountyAllocationList.Add(new TitleIVeAgencyGroupCountyAllocationGet_spResult { TitleIVeAgencyGroupCountyAllocationID = 0, AgencyGroupID = viewModel.AgencyGroupID, RecordStateID = 1 });
            viewModel.TitleIVeAgencyGroupCountyAllocationList.Add(new TitleIVeAgencyGroupCountyAllocationGet_spResult { TitleIVeAgencyGroupCountyAllocationID = 0, AgencyGroupID = viewModel.AgencyGroupID, RecordStateID = 1 });

            return View(viewModel);
        }
        [HttpPost]
        public virtual ActionResult AgencyGroupAllocation(List<TitleIVeAgencyGroupCountyAllocationGet_spResult> viewModel)
        {
            if (viewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(viewModel);
                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeAgencyGroupCountyAllocationInsertUpdate_sp", new TitleIVeAgencyGroupCountyAllocationInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    AgencyGroupID = viewModel[0].AgencyGroupID,
                    InputData = jsonData
                });

                return Json(new { isSuccess = true });

            }

            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }


    }
}