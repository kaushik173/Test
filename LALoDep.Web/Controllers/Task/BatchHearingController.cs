using DataTables.Mvc;
using LALoDep.Domain.BatchHearing;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.sup_Case;


namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        //
        // GET: /BatchHearing/
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.BatchHearingPage, PageSecurityItemID = SecurityToken.ViewBatchHearing)]
        public virtual ActionResult BatchHearing()
        {
            var viewModel = new BatchHearingViewModel();
            viewModel.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10);
            viewModel.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30);

            viewModel.OfficcerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp", new pd_HearingOfficerGet_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).Select(x => new SelectListItem()
            {
                Text = x.PersonNameLast + " " + x.PersonNameFirst,
                Value = x.PersonID.ToString(),
            }).ToList();

            return View(viewModel);
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.BatchHearingPage, PageSecurityItemID = SecurityToken.ViewBatchHearing)]
        [HttpPost]
        public virtual JsonResult BatchHearingSearch(BatchHearingViewModel viewModel)
        {
            var pd_BatchHearingSearch_spParams = new pd_BatchHearingSearch_spParams()
            {
                DocketNumber = viewModel.Case1,
                DocketNumber2 = viewModel.Case2,
                DocketNumber3 = viewModel.Case3,
                DocketNumber4 = viewModel.Case4,
                DocketNumber5 = viewModel.Case5,
                DocketNumber6 = viewModel.Case6,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var searchList = UtilityService.ExecStoredProcedureWithResults<pd_BatchHearingSearch_spResult>("pd_BatchHearingSearch_sp", pd_BatchHearingSearch_spParams).Select(x => new
            {
                Agency = x.Agency,
                JcatsNumber = x.CaseNumber,
                CaseNumber = x.PetitionDocketNumber,
                PetitionType = x.PetitionType,
                Department = x.Department,
                Client = x.Client,
                ChildName = x.ChildName,
                NextHearing = x.NextHearing,
                EncryptedCaseID = x.CaseID.ToEncrypt(),
                EncryptedPetitionID = x.PetitionID.ToEncrypt(),
                PetitionID = x.PetitionID,
                CaseID = x.CaseID,
            }).ToList();
            var total = searchList.Count;
            return Json(new DataTablesResponse(0, searchList, total, total));
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.BatchHearingPage, PageSecurityItemID = SecurityToken.ViewBatchHearing)]
        [HttpPost]
        public virtual JsonResult BatchHearingSave(BatchHearingViewModel viewModel)
        {
            foreach (var item in viewModel.BatchHearingList)
            {
                var pd_HearingInsert_spParams = new pd_HearingInsert_spParams()
                {
                    CaseID = item.CaseID.Value,
                    HearingTypeCodeID = viewModel.HearingTypeID,
                    HearingDateTime = Convert.ToDateTime(viewModel.HearingDate + " " + viewModel.HearingTime),
                    HearingOfficerPersonID = viewModel.HearingOfficerID,
                    HearingCourtDepartmentCodeID = viewModel.DepartmentID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                int insertedHearingID = (int)UtilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingInsert_sp", pd_HearingInsert_spParams).FirstOrDefault();
                foreach (var person in viewModel.BatchHearingList)
                {
                    var pd_HearingPersonInsertByPetitionID_spParams = new pd_HearingPersonInsertByPetitionID_spParams()
                    {
                        HearingID = insertedHearingID,
                        PetitionID = person.PetitionID.Value,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    UtilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingPersonInsertByPetitionID_sp", pd_HearingPersonInsertByPetitionID_spParams).FirstOrDefault();
                }

                var sup_CaseNextHearingSet_spParams = new sup_CaseNextHearingSet_spParams()
                {
                    CaseID = item.CaseID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("sup_CaseNextHearingSet_sp", sup_CaseNextHearingSet_spParams).ToList();
            }
            return Json(new { isSuccess = true });

        }

    }
}