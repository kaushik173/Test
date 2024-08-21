using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Motions;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Appeals;
using LALoDep.Domain.Advisement;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AdvisementsView)]
        public virtual ActionResult Advisements(string id, string caseId)
        {
            if (!id.IsNullOrEmpty() && !caseId.IsNullOrEmpty())
            {
                ViewBag.HearingID = id.ToDecrypt();
                int hearingId = id.ToDecrypt().ToInt();
                if (caseId.ToDecrypt().ToInt() > 0)
                    UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());


            }
            else if (!caseId.IsNullOrEmpty())
            {
               
                if (caseId.ToDecrypt().ToInt() > 0)
                    UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());


            }
            var model = new AdvisementViewModel();
            model.CaseID = UserManager.UserExtended.CaseID;
            model.AttorneyPersonID = UserManager.UserExtended.PersonID;
            model.AttorneyList = UtilityService.ExecStoredProcedureWithResults<Advisement_GetAttorneyList_spResult>(
                "Advisement_GetAttorneyList_sp", new Advisement_GetAttorneyList_spParams()
                {
                    CaseID = model.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).Select(o => new SelectListItem() { Text = o.AttorneyDisplay, Value = o.AttorneyPersonID.ToString(), Selected = o.DefaultFlag.Value == 1 }).ToList();
            model.StatusList = UtilityService.ExecStoredProcedureWithResults<Advisement_GetStatusList_spResult>(
              "Advisement_GetStatusList_sp", new Advisement_GetStatusList_spParams()
              {
                  CaseID = model.CaseID,
                  UserID = UserManager.UserExtended.UserID,
                  BatchLogJobID = Guid.NewGuid()
              }).Select(o => new SelectListItem() { Text = o.StatusDisplay, Value = o.StatusCodeID.ToString() }).ToList();


            model.Advisements = UtilityService.ExecStoredProcedureWithResults<Advisement_GetByCaseID_spResult>(
            "Advisement_GetByCaseID_sp", new Advisement_GetByCaseID_spParams()
            {
                CaseID = model.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();

            return View(model);
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AdvisementsView)]
        [HttpPost]
        public virtual JsonResult AdvisementsSave(List<AdvisementIUD_spParams> spParams)
        {
            foreach (var paramList in spParams)
            {
                paramList.BatchLogJobID = Guid.NewGuid();
                paramList.UserID = UserManager.UserExtended.UserID;
                if (!paramList.CaseID.HasValue)
                    paramList.CaseID = UserManager.UserExtended.CaseID;
                UtilityService.ExecStoredProcedureWithoutResultADO("AdvisementIUD_sp", paramList);
            }


            return Json(new { isSuccess = true, Model = spParams });
        }


    }
}