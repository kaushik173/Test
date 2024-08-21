using LALoDep.Domain.JudgeDeptTrans;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using System;
using System.Linq;
using System.Web.Mvc;


namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController : Controller
    {



        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.JudgeDeptTransferPage, PageSecurityItemID = SecurityToken.ViewJudgeDeptTransferPage)]
        public virtual ActionResult JudgeDeptTransfer(string id)
        {

            var viewModel = new JudgeDeptTransferViewModel();

            viewModel.AgencyGroupID = id.ToDecrypt().ToIntNullable();

            viewModel.AgencyGroupList = UtilityService.ExecStoredProcedureWithResults<JudgeDeptTrans_GetAgencyGroup_spResult>("JudgeDeptTrans_GetAgencyGroup_sp", new JudgeDeptTrans_GetAgencyGroup_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyGroupID = viewModel.AgencyGroupID
            }).Select(o => new SelectListItem { Text = o.AgencyGroupDisplay, Value = o.AgencyGroupID.Value.ToString().ToEncrypt(), Selected = o.Selected.Value == 1 }).ToList();

            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<JudgeDeptTrans_GetAgency_spResult>("JudgeDeptTrans_GetAgency_sp", new JudgeDeptTrans_GetAgency_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyGroupID = viewModel.AgencyGroupID
            }).Select(o => new SelectListItem { Text = o.AgencyDisplay, Value = o.AgencyID.ToString() }).ToList();

            viewModel.AttorneyPersonList = UtilityService.ExecStoredProcedureWithResults<JudgeDeptTrans_GetAttorney_spResult>("JudgeDeptTrans_GetAttorney_sp", new JudgeDeptTrans_GetAttorney_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyGroupID = viewModel.AgencyGroupID
            }).Select(o => new SelectListItem { Text = o.AttorneyDisplay, Value = o.AttorneyPersonID.ToString() }).ToList();

            viewModel.UpdateJudgePersonList = viewModel.JudgePersonList = UtilityService.ExecStoredProcedureWithResults<JudgeDeptTrans_GetJudge_spResult>("JudgeDeptTrans_GetJudge_sp", new JudgeDeptTrans_GetJudge_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyGroupID = viewModel.AgencyGroupID
            }).Select(o => new SelectListItem { Text = o.JudgeDisplay, Value = o.JudgePersonID.ToString() }).ToList();
            viewModel.UpdateDeptCodeList = viewModel.DeptCodeList = UtilityService.ExecStoredProcedureWithResults<JudgeDeptTrans_GetDept_spResult>("JudgeDeptTrans_GetDept_sp", new JudgeDeptTrans_GetDept_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyGroupID = viewModel.AgencyGroupID
            }).Select(o => new SelectListItem { Text = o.DeptDisplay, Value = o.DeptCodeID.ToString() }).ToList();


            if (viewModel.AgencyList.Count() == 1)
                viewModel.AgencyID = viewModel.AgencyList.FirstOrDefault().Value.ToInt();

            if (viewModel.JudgePersonList.Count() == 1)
                viewModel.JudgePersonID = viewModel.JudgePersonList.FirstOrDefault().Value.ToInt();

            if (viewModel.DeptCodeList.Count() == 1)
                viewModel.DeptCodeID = viewModel.DeptCodeList.FirstOrDefault().Value.ToInt();

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult JudgeDeptTransfer(JudgeDeptTransferViewModel model)
        {
            model.AgencyGroupID = Request.Form["AgencyGroupID"].ToDecrypt().ToIntNullable();
            var searchParams = new JudgeDeptTrans_Search_spParams
            {
                AgencyID = model.AgencyID,
                AgencyGroupID = model.AgencyGroupID,
                AttorneyPersonID = model.AttorneyPersonID,
                DeptCodeID = model.DeptCodeID,
                HearingEndDate = model.HearingEndDate.IsNullOrEmpty() ? DateTime.Parse("12/31/2999") : model.HearingEndDate.ToDateTime(),
                HearingStartDate = model.HearingStartDate.ToDateTime(),
                JudgePersonID = model.JudgePersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            model.JudgeDeptTransSearchResult = UtilityService.ExecStoredProcedureWithResults<JudgeDeptTrans_Search_spResult>(
                                                    "JudgeDeptTrans_Search_sp", searchParams).ToList();
            return PartialView("_JudgeDeptTransferSearchResult", model);
        }
        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.JudgeDeptTransferPage, PageSecurityItemID = SecurityToken.ViewJudgeDeptTransferPage)]

        public virtual ActionResult JudgeDeptTransferSubmit(JudgeDeptTransferViewModel model)
        {
          
            UtilityService.ExecStoredProcedureWithoutResultADO("JudgeDeptTrans_TransferProcess_sp", new JudgeDeptTrans_TransferProcess_spParams
            {
                TransferGUID = model.TransferGUID,
                UpdateToDeptCodeID = model.UpdateDeptCodeID,
                UpdateToJudgePersonID = model.UpdateJudgePersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            });
            return Json(new { isSuccess = true });
        }


    }
}