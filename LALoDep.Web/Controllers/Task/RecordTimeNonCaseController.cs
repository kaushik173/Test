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
using LALoDep.Models.Task;
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
using LALoDep.Domain.RTNC;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        #region RecordTime List

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimeNonCasePage, PageSecurityItemID = SecurityToken.RecordTimeNonCaseView)]
        public virtual ActionResult RecordTimeNonCase()
        {


            var viewModel = new RecordTimeViewModel();
            var RTNC_WorkDescription_spParams = new RTNC_WorkDescription_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var RTNC_AgencyGroup_spParams = new RTNC_AgencyGroup_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            viewModel.StartDate = DateTime.Now.AddDays(-7).ToString("d");
            viewModel.EndDate = DateTime.Now.ToString("d");
            viewModel.WorkDescriptionList = UtilityService.ExecStoredProcedureWithResults<RTNC_WorkDescription_spResult>("RTNC_WorkDescription_sp", RTNC_WorkDescription_spParams).ToList();
            viewModel.StaffMemberList = UtilityService.ExecStoredProcedureWithResults<RTNC_StaffMember_spResult>("RTNC_StaffMember_sp", RTNC_WorkDescription_spParams).ToList();
            viewModel.AgencyGroupList = UtilityService.ExecStoredProcedureWithResults<RTNC_AgencyGroup_spResult>("RTNC_AgencyGroup_sp", RTNC_AgencyGroup_spParams).ToList();
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<RTNC_Agency_spResult>("RTNC_Agency_sp", RTNC_AgencyGroup_spParams).ToList();
            viewModel.SupervisorList = UtilityService.ExecStoredProcedureWithResults<RTNC_Supervisor_spResult>("RTNC_Supervisor_sp", RTNC_AgencyGroup_spParams).ToList();

            if (viewModel.WorkDescriptionList.Any(o => o.Selected == 1))
                viewModel.WorkDescriptionID = viewModel.WorkDescriptionList.FirstOrDefault(o => o.Selected == 1).CodeID.Value;

            if (viewModel.StaffMemberList.Any(o => o.Selected == 1))
                viewModel.StaffMemberID = viewModel.StaffMemberList.FirstOrDefault(o => o.Selected == 1).PersonID.Value;


            if (viewModel.AgencyGroupList.Any(o => o.Selected == 1))
                viewModel.AgencyGroupID = viewModel.AgencyGroupList.FirstOrDefault(o => o.Selected == 1).AgencyGroupID.Value;

            if (viewModel.AgencyList.Any(o => o.Selected == 1))
                viewModel.AgencyID = viewModel.AgencyList.FirstOrDefault(o => o.Selected == 1).AgencyID.Value;

            if (viewModel.SupervisorList.Any(o => o.Selected == 1))
                viewModel.SupervisorID = viewModel.SupervisorList.FirstOrDefault(o => o.Selected == 1).PersonID.Value;

            return View(viewModel);


        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimeNonCasePage, PageSecurityItemID = SecurityToken.RecordTimeNonCaseView)]
        [HttpPost]
        public virtual PartialViewResult RecordTimeNonCaseList(RecordTimeViewModel viewModel)
        {
            var RTNC_Search_spParams = new RTNC_Search_spParams()
            {

                UserID = UserManager.UserExtended.UserID,

                BatchLogJobID = Guid.NewGuid(),
                AgencyGroupID = viewModel.AgencyGroupID,
                AgencyID = viewModel.AgencyID,
                SupervisorPersonID = viewModel.SupervisorID,
                StaffPersonID = viewModel.StaffMemberID,
                WorkDescriptionCodeID = viewModel.WorkDescriptionID,
                StartDate = viewModel.StartDate.ToDateTime(),
                EndDate = viewModel.EndDate.ToDateTime(),
            };
            var data = UtilityService.ExecStoredProcedureWithResults<RTNC_Search_spResult>("RTNC_Search_sp", RTNC_Search_spParams).ToList();


            return PartialView("_partialRecordTimeNonCaseList", data);
        }



        #endregion RecordTime List

        #region RecordTime Add/Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimeNonCasePage, PageSecurityItemID = SecurityToken.RecordTimeAdd)]
        public virtual ActionResult RecordTimeNonCaseAdd()
        {

            var viewModel = GetRecordTimeNonCaseAddEditModel("");
            return View("~/Views/Task/RecordTimeNonCaseAddEdit.cshtml", viewModel);

        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimeNonCasePage, PageSecurityItemID = SecurityToken.RecordTimeEdit)]
        public virtual ActionResult RecordTimeNonCaseEdit(string id)
        {

            var viewModel = GetRecordTimeNonCaseAddEditModel(id);
            return View("~/Views/Task/RecordTimeNonCaseAddEdit.cshtml", viewModel);

        }
        public RecordTimeNonCaseAddEditViewModel GetRecordTimeNonCaseAddEditModel(string id)
        {
            int workId = id.ToDecrypt().ToInt();


            //If User Coming RFD Wizard  AR Record Time then we need to maintain the wizard steps and its information 


            var viewModel = new RecordTimeNonCaseAddEditViewModel();

            var RTNC_WorkDescription_spParams = new RTNC_WorkDescription_spParams()
            {
                WorkID = workId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.WorkDescriptionList = UtilityService.ExecStoredProcedureWithResults<RTNC_WorkDescription_spResult>("RTNC_WorkDescription_sp", RTNC_WorkDescription_spParams).ToList();
            viewModel.WorkerList = UtilityService.ExecStoredProcedureWithResults<RTNC_StaffMember_spResult>("RTNC_StaffMember_sp", RTNC_WorkDescription_spParams).ToList();
            var countyList = UtilityService.ExecStoredProcedureWithResults<RTNC_County_spResult>("RTNC_County_sp", RTNC_WorkDescription_spParams).ToList();
            var selectCounty = countyList.FirstOrDefault(o => o.Selected == 1);
            if (selectCounty != null)
            {
                viewModel.CountyID = selectCounty.AgencyCountyID;
            }
            viewModel.CountyList = countyList.Select(o => new SelectListItem() { Text = o.AgencyCounty, Value = o.AgencyCountyID.ToString(), Selected = o.Selected.ToBoolean() });
            if (viewModel.WorkDescriptionList.Any(o => o.Selected == 1))
                viewModel.WorkDescriptionCodeID = viewModel.WorkDescriptionList.FirstOrDefault(o => o.Selected == 1).CodeID.Value;

            if (viewModel.WorkerList.Any(o => o.Selected == 1))
                viewModel.PersonID = viewModel.WorkerList.FirstOrDefault(o => o.Selected == 1).PersonID.Value;
            if (workId > 0)
            {
                var pd_WorkGet_spParams = new pd_WorkGet_spParams
                {
                    WorkID = workId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                var work = UtilityService.ExecStoredProcedureWithResults<pd_WorkGet_spResult>("pd_WorkGet_sp", pd_WorkGet_spParams).FirstOrDefault();
                if (work != null)
                {
                    viewModel.InjectFrom(work);
                    viewModel.WorkStartDate = work.WorkStartDate.HasValue ? work.WorkStartDate.ToShortDateString() : string.Empty;
                    viewModel.WorkEndDate = work.WorkEndDate.HasValue ? work.WorkEndDate.ToShortDateString() : string.Empty;

                    var workNoteParams = new pd_NoteGetByEntity_spParams
                    {
                        EntityPrimaryKeyID = workId,
                        EntityCodeSystemValueTypeID = 152,
                        EntityCodeTypeSystemValueTypeID = 123,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };

                    // Note
                    var workNoteResult = UtilityService.ExecStoredProcedureWithResults<pd_NoteGetByEntity_spResult>("pd_NoteGetByEntity_sp", workNoteParams).FirstOrDefault();
                    if (workNoteResult != null)
                    {
                        viewModel.NoteEntry = workNoteResult.NoteEntry;
                        viewModel.NoteID = workNoteResult.NoteID ?? 0;
                        viewModel.NoteAgencyID = workNoteResult.AgencyID;
                        viewModel.NoteEntityCodeID = workNoteResult.NoteEntityCodeID;
                        viewModel.NoteEntityTypeCodeID = workNoteResult.NoteEntityTypeCodeID;
                        viewModel.NoteEntityPrimaryKeyID = workNoteResult.EntityPrimaryKeyID;
                        viewModel.NoteTypeCodeID = workNoteResult.NoteTypeCodeID;
                        viewModel.NoteSubject = workNoteResult.NoteSubject;
                        viewModel.NoteCaseID = workNoteResult.CaseID;
                        viewModel.NotePetitionID = workNoteResult.PetitionID;
                        viewModel.NoteHearingID = workNoteResult.HearingID;
                        viewModel.NoteRecordStateID = workNoteResult.RecordStateID;
                    }
                    viewModel.ControlType = UtilityFunctions.GetNoteControlType("Task/RecordTimeNonCaseAddEdit", noteId: viewModel.NoteID, caseId: 0);


                    viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(workId: work.WorkID, workIVeEligibleCodeId: work.WorkIVeEligibleCodeID, agencyId: work.AgencyID.Value);

                    //var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
                    //{

                    //    CaseID = UserManager.UserExtended.CaseID,

                    //    UserID = UserManager.UserExtended.UserID,
                    //    BatchLogJobID = Guid.NewGuid()

                    //}).FirstOrDefault();
                    //if (defaultPhase != null && defaultPhase.WorkPhaseCodeID.HasValue)
                    //{
                    //    viewModel.RecordTimeNoteSubjectFlag = defaultPhase.RecordTimeNoteSubjectFlag.HasValue ? defaultPhase.RecordTimeNoteSubjectFlag.Value : 0;

                    //    viewModel.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.HasValue ? defaultPhase.WorkHoursRequiredFlag.Value : 0;
                    //}
                    //if (viewModel.RecordTimeNoteSubjectFlag == 0)
                    //{
                    //    if (viewModel.NoteSubject.IsNullOrEmpty())
                    //        viewModel.NoteSubject = "Record Time Note";
                    //}

                }
                else
                {
                    viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(agencyId: UserManager.UserExtended.AgencyID);
                }
            }
            else
            {
                viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(agencyId: UserManager.UserExtended.AgencyID);
            }
            return viewModel;
        }



        [HttpPost]
        public virtual JsonResult RecordTimeNonCaseAddEditSave(RecordTimeNonCaseAddEditViewModel viewModel, RecordTimeNonCaseAddEditViewModel oldViewModel)
        {

            if (viewModel.WorkID > 0)
            {


                if (viewModel.PersonID != oldViewModel.PersonID || viewModel.WorkHours != oldViewModel.WorkHours
                    || viewModel.WorkHoursOverTime != oldViewModel.WorkHoursOverTime
                    || viewModel.WorkDescriptionCodeID != oldViewModel.WorkDescriptionCodeID
                    || viewModel.WorkStartDate != oldViewModel.WorkStartDate || viewModel.WorkIVeEligibleCodeID != oldViewModel.WorkIVeEligibleCodeID || viewModel.CountyID != oldViewModel.CountyID)
                {
                    // Update Work time
                    var pd_WorkUpdate1_spParams = new pd_WorkUpdate1_spParams
                    {
                        WorkID = viewModel.WorkID,
                        AgencyID = viewModel.AgencyID,
                        CaseID = -1,
                        PersonID = viewModel.PersonID,
                        WorkHours = viewModel.WorkHours,
                        WorkHoursOverTime = viewModel.WorkHoursOverTime,
                        WorkMileage = viewModel.WorkMileage,
                        WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID,
                        RecordStateID = viewModel.RecordStateID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        WorkPhaseCodeID = viewModel.WorkPhaseCodeID,
                        HearingID = viewModel.HearingID.ToInt() <= 0 ? -1 : viewModel.HearingID.ToInt(),
                        WorkIVeEligibleCodeID = viewModel.WorkIVeEligibleCodeID,
                        AgencyCountyID=viewModel.CountyID
                    };

                    if (!string.IsNullOrEmpty(viewModel.WorkStartDate))
                        pd_WorkUpdate1_spParams.WorkStartDate = Convert.ToDateTime(viewModel.WorkStartDate);


                    UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkUpdate1_sp", pd_WorkUpdate1_spParams).FirstOrDefault();
                }
            }
            else
            {
                var pd_WorkInsert1_spParams = new pd_WorkInsert1_spParams()
                {
                    CaseID = -1,
                    PersonID = viewModel.PersonID.Value,
                    WorkHours = viewModel.WorkHours,
                    WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID.Value,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    AgencyID = UserManager.UserExtended.AgencyID,
                    WorkIVeEligibleCodeID = viewModel.WorkIVeEligibleCodeID
                      ,AgencyCountyID = viewModel.CountyID
                };
                if (!string.IsNullOrEmpty(viewModel.WorkStartDate))
                    pd_WorkInsert1_spParams.WorkEndDate = pd_WorkInsert1_spParams.WorkStartDate = viewModel.WorkStartDate.ToDateTime();
                else
                    pd_WorkInsert1_spParams.WorkEndDate = pd_WorkInsert1_spParams.WorkStartDate = DateTime.Now;


                viewModel.WorkID = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_WorkInsert1_sp", pd_WorkInsert1_spParams).FirstOrDefault().ToInt();
            }
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var noteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams
                {
                    NoteEntitySystemValueTypeID = 152,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = viewModel.WorkID,
                    NoteTypeCodeID = 16,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = -1,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();

            }
            else if (viewModel.NoteID.HasValue && ((!string.IsNullOrEmpty(viewModel.NoteEntry) && viewModel.NoteEntry != oldViewModel.NoteEntry) || viewModel.NoteSubject != oldViewModel.NoteSubject))
            {

                // Update Note
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteUpdate_sp", new pd_NoteUpdate_spParams
                {
                    NoteID = viewModel.NoteID,
                    AgencyID = viewModel.NoteAgencyID,
                    NoteEntityCodeID = viewModel.NoteEntityCodeID,
                    NoteEntityTypeCodeID = viewModel.NoteEntityTypeCodeID,
                    EntityPrimaryKeyID = viewModel.NoteEntityPrimaryKeyID,
                    NoteTypeCodeID = viewModel.NoteTypeCodeID,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = -1,
                    PetitionID = viewModel.NotePetitionID,
                    HearingID = viewModel.NoteHearingID,
                    RecordStateID = viewModel.NoteRecordStateID,
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                {
                    RecordStateID = 10,
                    ID = viewModel.NoteID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note"
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }




            string URL = "/Task/RecordTimeNonCase";
            return Json(new { isSuccess = true, URL = URL });
        }
        #endregion RecordTime  

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeDelete)]
        public virtual JsonResult RecordTimeNonCaseDelete(string id)
        {
            var pd_WorkDelete_spParams = new pd_WorkDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Work",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkDelete_sp", pd_WorkDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }
    }
}