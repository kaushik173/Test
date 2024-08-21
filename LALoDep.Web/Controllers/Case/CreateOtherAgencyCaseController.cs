using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using LALoDep.Domain.pd_Role;
using DataTables.Mvc;
using LALoDep.Domain.pd_Code;
using LALoDep.Core.Enums;
using LALoDep.Models;
using LALoDep.Custom;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.pd_wt;
using LALoDep.Domain.pd_Note;
using Omu.ValueInjecter;
using LALoDep.Domain.pd_Conflict;
using LALoDep.Domain.rpt_Print;
using Jcats.SD.Domain.COAC;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {


        #region Create Other Agency Case

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CreateOtherAgencyCase, PageSecurityItemID = SecurityToken.CreateOtherAgencyCase)]
        public virtual ActionResult CreateOtherAgencyCase()
        {
            if (UserManager.UserExtended.CaseID > 0)
            {


                var viewModel = new CreateOtherAgencyCaseViewModel();


                var coac_CaseRoles_spParams = new coac_CaseRoles_spParams()
                {

                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    UserID = UserManager.UserExtended.UserID,

                };
                viewModel.CaseRolesList = UtilityService.ExecStoredProcedureWithResults<coac_CaseRoles_spResult>("coac_CaseRoles_sp", coac_CaseRoles_spParams).ToList();

                viewModel.ConflictResultsList = UtilityService.ExecStoredProcedureWithResults<coac_ConflictResults_spResult>("coac_ConflictResults_sp", coac_CaseRoles_spParams).ToList();
                viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<coac_AttorneyList_spResult>("coac_AttorneyList_sp", coac_CaseRoles_spParams).Select(o => new SelectListItem { Text = o.AttorneyDisplay, Value = o.AttorneyPersonID.Value.ToString() + "|" + o.AttorneyAgencyID.Value.ToString() }).ToList();


                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }


        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CreateOtherAgencyCase, PageSecurityItemID = SecurityToken.CreateOtherAgencyCase)]
        public virtual JsonResult CreateOtherAgencyCase(CreateOtherAgencyCaseViewModel viewModel)

        {


            UtilityService.ExecStoredProcedureWithResults<object>("pd_CopyCaseProcess_sp", new pd_CopyCaseProcess_spParams
            {

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                FromCaseID = UserManager.UserExtended.CaseID,
                IncludeRoleID = 0,
                CopyAllFlag = 1,
                TransferDate = viewModel.ApptDate.ToDateTime(),
                ToAttorneyPersonID = viewModel.AttorneyAndAgencyID.Split('|')[0].ToInt(),
                ToAgencyID = viewModel.AttorneyAndAgencyID.Split('|')[1].ToInt(),
                CopyMode= "CreateOtherAgencyCase",
                DoNotRunSummaryFlag=0,
                IncludeClientPersonIDList=viewModel.SelectedPersonIDs
                
            }).FirstOrDefault();


            return Json(new { isSuccess = true });
        }

        #endregion 

    }
}