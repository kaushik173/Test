using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain;
using LALoDep.Domain.Agency;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using LALoDep.Models.Case;
using LALoDep.Domain.Advisement;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing)]
        public virtual ActionResult Hearing(string id)
        {

            if (Request.QueryString["CaseID"] != null)
            {
                Custom.UserEnvironment.UserManager.UpdateCaseStatusBar(Request.QueryString["CaseID"].ToDecrypt().ToInt());
            }

            var roleGetForHearingAttendingAttorney =
                UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForHearingAttendingAttorney_spResult>(
                    "pd_RoleGetForHearingAttendingAttorney_sp", new pd_RoleGetForHearingAttendingAttorney_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,
                        HearingID = id.ToDecrypt().ToInt(),
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID
                    });
            var model = new HearingModel
            {
                HearingOfficerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp", new pd_HearingOfficerGet_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID
                }).Select(o => new SelectListItem() { Value = o.PersonID.ToString(), Text = o.PersonNameLast + ", " + o.PersonNameFirst }).ToList(),
                AppearingAttorneyList = roleGetForHearingAttendingAttorney.Select(o => new SelectListItem() { Value = o.PersonID.ToString() + "|" + o.RoleID.ToString() + "|" + o.HearingAttendanceID.ToString(), Text = o.PersonNameDisplay, Selected = o.HearingAttendanceID > 0 }).ToList(),

                //GlobalResultList = UtilityFunctions.CodeGetByTypeIdAndUserId(11).OrderBy(x => x.Text),
                CaseStatusList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetClientStatus_spResult>(
                    "pd_CodeGetClientStatus_sp",
                    new pd_CodeGetClientStatus_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        HearingID = id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList(),


                HearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseID_spResults>("pd_HearingGetByCaseID_sp", new LALoDep.Domain.pd_Hearing.pd_HearingGetByCaseID_spParams
                {
                    UserID = UserManager.UserExtended.UserID,

                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID
                }).ToList(),




                HoursLabel = "Hours (use tenths for partial hrs)"
            };

            model.HearingContinuanceReasonGetForHearingList = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<HearingContinuanceReasonGetForHearing_spResult>(
               "HearingContinuanceReasonGetForHearing_sp", new HearingContinuanceReasonGetForHearing_spParams
               {
                   CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                   CaseID = UserManager.UserExtended.CaseID,
                   HearingID = id.ToDecrypt().ToInt() > 0 ? id.ToDecrypt().ToInt() : (int?)null,
                   UserID = UserEnvironment.UserManager.UserExtended.UserID,
               }).ToList();

            model.HearingContinuanceRequestedByGetForHearingList = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<HearingContinuanceRequestedByGetForHearing_spResult>(
            "HearingContinuanceRequestedByGetForHearing_sp", new HearingContinuanceRequestedByGetForHearing_spParams
            {
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = id.ToDecrypt().ToInt() > 0 ? id.ToDecrypt().ToInt() : (int?)null,
                UserID = UserEnvironment.UserManager.UserExtended.UserID,
            }).ToList();

            var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).FirstOrDefault();

            if (!string.IsNullOrEmpty(defaultPhase?.HoursLabel))
                model.HoursLabel = defaultPhase?.HoursLabel;
            if (defaultPhase.WorkHoursRequiredFlag.HasValue)
                model.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.Value;

            if (defaultPhase.HideWorkHoursOnHearingPages.HasValue)
                model.HideWorkHoursOnHearingPages = defaultPhase.HideWorkHoursOnHearingPages.Value;

            var hearingResults = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults_ForHearingResult>(
               "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
               {
                   CodeTypeID = 11,
                   SortOption = "IncludeInActive",
                   BatchLogJobID = Guid.NewGuid(),
                   UserID = UserEnvironment.UserManager.UserExtended.UserID,
                   AgencyID = UserManager.UserExtended.CaseNumberAgencyID
               }).ToList();

            model.GlobalResultList = hearingResults.Where(x => x.ActiveFlag == 1).Select(x => new SelectListItem() { Text = x.CodeDisplay, Value = x.CodeID.ToString() });

            var caseDefault =
                UtilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
                    new pd_CaseGetDefaults_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,

                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID
                    }).FirstOrDefault();
            if (caseDefault != null)
            {
                model.DefaultHearingTime = caseDefault.DefaultHearingTime;
                model.DefaultHearingTimeContested = caseDefault.DefaultHearingTimeContested;
                model.HearingTime = caseDefault.DefaultHearingTime;
                model.HoursNotRequiredBeforeHearingDate = caseDefault.HoursNotRequiredBeforeHearingDate;
                if (caseDefault.DefaultHearingCourtDepartmentCodeID != null)
                {
                    model.DepartmentID = caseDefault.DefaultHearingCourtDepartmentCodeID.Value;

                }
                if (caseDefault.DataValidation_RequireJudgeFlag != null)
                {

                    model.DataValidation_RequireJudgeFlag = caseDefault.DataValidation_RequireJudgeFlag.Value;
                }
                if (caseDefault.DefaultHearingOfficerPersonID != null)
                    model.HearingOfficerID = caseDefault.DefaultHearingOfficerPersonID.Value;
                if (caseDefault.DataValidation_RequireHearingHoursFlag != null)
                    model.DataValidation_RequireHearingHoursFlag = caseDefault.DataValidation_RequireHearingHoursFlag.Value;
            }


            var summary = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (summary != null)
            {
                model.CaseDepartmentID = summary.DepartmentID ?? 0;

            }
            model.HearingAttendance = UtilityService.ExecStoredProcedureWithResults<pd_HearingAttendanceGetByHearingID_spResult>("pd_HearingAttendanceGetByHearingID_sp", new pd_HearingAttendanceGetByHearingID_spParams()
            {
                HearingID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "HearingEdit"
            }).Select(x => new HearingAttendanceListViewModel()
            {
                AttendedFlag = x.AttendedFlag,
                PersonNameDisplay = x.PersonNameDisplay,
                PersonNameLast = x.PersonNameLast,
                PersonNameFirst = x.PersonNameFirst,
                RoleID = x.RoleID,
                RoleTypeID = x.RoleTypeID,
                RoleType = x.RoleType,
                RoleClient = x.RoleClient,
                AttendanceID = x.AttendanceID,
                IsSelected = x.AttendedFlag == 1,
                IsEditable = x.Editable == 1 ? true : false,
                CS_CodeDisplay = x.CS_CodeDisplay,
                CS_CodeID = x.CS_CodeID,
                CS_ID = x.CS_ID,
                CS_PersonID = x.CS_PersonID,
                CS_StartDate = x.CS_StartDate,


            }).ToList();
            model.IsCaseStatusExists = model.CaseStatusList.Any();
            if (!string.IsNullOrEmpty(id))
            {
                var hearingGet =
                    UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                        new pd_HearingGet_spParams
                        {
                            UserID = UserManager.UserExtended.UserID,

                            BatchLogJobID = Guid.NewGuid(),
                            HearingID = id.ToDecrypt().ToInt()
                        }).FirstOrDefault();
                if (hearingGet != null)
                {
                    if (hearingGet.WorkHasInvoiceFlag.HasValue && hearingGet.WorkHasInvoiceFlag.Value > 0)
                    {
                        model.WorkHasInvoiceFlag = hearingGet.WorkHasInvoiceFlag.Value;

                    }
                    if (hearingGet.Resulted.HasValue && hearingGet.Resulted.Value > 0)
                    {
                        model.OriginalHearingResulted = hearingGet.Resulted.Value;

                    }
                    if (hearingGet.HearingTypeCodeID != null) model.HearingTypeID = hearingGet.HearingTypeCodeID.Value;
                    if (hearingGet.HearingDateTime != null)
                    {
                        model.HearingDate = hearingGet.HearingDateTime.Value.ToString("d");
                        model.HearingTime = hearingGet.HearingDateTime.Value.ToString("t");
                    }
                    if (hearingGet.HearingCourtDepartmentCodeID != null)
                        model.DepartmentID = hearingGet.HearingCourtDepartmentCodeID.Value;
                    if (hearingGet.HearingOfficerPersonID != null)
                        model.HearingOfficerID = hearingGet.HearingOfficerPersonID.Value;
                    model.Note = hearingGet.NoteEntry;
                    model.NoteID = hearingGet.NoteID;
                    if (hearingGet.HearingID != null)
                        model.HearingID = hearingGet.HearingID.Value;

                    if (hearingGet.WorkDescriptionCodeID != null)
                        model.HourTypeID = hearingGet.WorkDescriptionCodeID.Value;
                    if (hearingGet.WorkPhaseCodeID != null) model.PhaseID = hearingGet.WorkPhaseCodeID.Value;
                    if (hearingGet.WorkHours != null) model.Hours = hearingGet.WorkHours.Value;
                    if (hearingGet.HearingMediaPresentFlag != null)
                        model.MediaPresent = hearingGet.HearingMediaPresentFlag.Value == 1;
                    if (hearingGet.WorkID != null)
                        model.WorkID = hearingGet.WorkID.Value;
                    if (hearingGet.HearingRequestedByCodeID != null)
                        model.ContinuanceRequestedByID = hearingGet.HearingRequestedByCodeID.Value;


                    var attonory = roleGetForHearingAttendingAttorney.Where(o => o.HearingAttendanceID > 0).FirstOrDefault();
                    if (attonory != null)
                    {
                        model.AppearingAttorneyID = attonory.PersonID + "|" + attonory.RoleID.ToString() + "|" + attonory.HearingAttendanceID.ToString();
                        model.HearingAttendanceID = attonory.HearingAttendanceID;
                    }
                    model.WorkList =
                   UtilityService.ExecStoredProcedureWithResults<pd_WorkGetByCaseID_spResult>("pd_WorkGetByCaseID_sp",
                       new pd_WorkGetByCaseID_spParams
                       {
                           UserID = UserManager.UserExtended.UserID,

                           BatchLogJobID = Guid.NewGuid(),
                           CaseID = UserManager.UserExtended.CaseID
                       }).Take(5).ToList();

                    model.PetitionByHearingList =
                      UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetAllByHearingID_spResult>(
                          "pd_PetitionGetAllByHearingID_sp", new pd_PetitionGetAllByHearingID_spParams
                          {
                              UserID = UserManager.UserExtended.UserID,

                              BatchLogJobID = Guid.NewGuid(),
                              HearingID = hearingGet.HearingID.Value
                          }).ToList();

                }

                ViewBag.AllHearingResults = hearingResults;

                model.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10, includeCodeId: model.HearingTypeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, includeCodeId: model.DepartmentID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HourTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(222, includeCodeId: model.HourTypeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: model.PhaseID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.PetitionType602List = UtilityFunctions.CodeGetBySystemValueTypeId(179, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingType602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(239, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                model.HearingResult602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(240, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                model.CourtDepartment602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(241, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeContestedList = UtilityFunctions.CodeGetBySystemValueTypeId(46, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingResultContinuanceList = UtilityFunctions.CodeGetBySystemValueTypeId(47, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeNonHearingEventList = UtilityFunctions.CodeGetBySystemValueTypeId(214, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingResultFutureHearingsList = UtilityFunctions.CodeGetBySystemValueTypeId(207, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                if (model.ContinuanceRequestedByID.HasValue)
                    model.ContinuanceRequestedByList = UtilityFunctions.CodeGetByTypeIdAndUserId(28, includeCodeId: model.ContinuanceRequestedByID.Value, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                else
                    model.ContinuanceRequestedByList = UtilityFunctions.CodeGetByTypeIdAndUserId(28, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.ControlType = UtilityFunctions.GetNoteControlType("CaseOpening/Hearing", noteId: model.NoteID);
                model.HearingResultListForHoursNotRequiredValidation = UtilityFunctions.CodeGetBySystemValueTypeId(262, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeHoursNotRequired = UtilityFunctions.CodeGetBySystemValueTypeId(265, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HourTypeParentList =
          UtilityService.ExecStoredProcedureWithResults<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults>(
              "CodeHierarchyGetByCodeRelationshipIDAgencyID_sp", new CodeHierarchyGetByCodeRelationshipIDAgencyID_spParams
              {
                  UserID = UserManager.UserExtended.UserID,

                  BatchLogJobID = Guid.NewGuid(),
                  CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                  CodeRelationshipID = 4,
                  IncludeParentCodeID = model.HourTypeID,
                  IncludeChildCodeID = model.HearingTypeID

              }).ToList();
                model.PhaseParentList =
             UtilityService.ExecStoredProcedureWithResults<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults>(
                 "CodeHierarchyGetByCodeRelationshipIDAgencyID_sp", new CodeHierarchyGetByCodeRelationshipIDAgencyID_spParams
                 {
                     UserID = UserManager.UserExtended.UserID,

                     BatchLogJobID = Guid.NewGuid(),
                     CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                     CodeRelationshipID = 5,
                     IncludeParentCodeID = model.PhaseID,
                     IncludeChildCodeID = model.HearingTypeID
                 }).ToList();
                return View("~/Views/CaseOpening/HearingEdit.cshtml", model);
            }
            else
            {

                model.PetitionList =
                    UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetByCaseID_spResult>(
                        "pd_PetitionGetByCaseID_sp", new pd_PetitionGetByCaseID_spParams
                        {
                            UserID = UserManager.UserExtended.UserID,

                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = UserManager.UserExtended.CaseID
                        }).ToList();

                model.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HourTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(222, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                model.PetitionType602List = UtilityFunctions.CodeGetBySystemValueTypeId(179, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingType602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(239, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                model.HearingResult602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(240, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                model.CourtDepartment602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(241, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeContestedList = UtilityFunctions.CodeGetBySystemValueTypeId(46, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingResultContinuanceList = UtilityFunctions.CodeGetBySystemValueTypeId(47, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeNonHearingEventList = UtilityFunctions.CodeGetBySystemValueTypeId(214, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingResultFutureHearingsList = UtilityFunctions.CodeGetBySystemValueTypeId(207, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingResultListForHoursNotRequiredValidation = UtilityFunctions.CodeGetBySystemValueTypeId(262, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeHoursNotRequired = UtilityFunctions.CodeGetBySystemValueTypeId(265, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HourTypeParentList =
             UtilityService.ExecStoredProcedureWithResults<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults>(
                 "CodeHierarchyGetByCodeRelationshipIDAgencyID_sp", new CodeHierarchyGetByCodeRelationshipIDAgencyID_spParams
                 {
                     UserID = UserManager.UserExtended.UserID,

                     BatchLogJobID = Guid.NewGuid(),
                     CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                     CodeRelationshipID = 4,


                 }).ToList();
                model.PhaseParentList =
            UtilityService.ExecStoredProcedureWithResults<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults>(
                "CodeHierarchyGetByCodeRelationshipIDAgencyID_sp", new CodeHierarchyGetByCodeRelationshipIDAgencyID_spParams
                {
                    UserID = UserManager.UserExtended.UserID,

                    BatchLogJobID = Guid.NewGuid(),
                    CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    CodeRelationshipID = 5,

                }).ToList();
            }
            if (model.ContinuanceRequestedByID.HasValue)
                model.ContinuanceRequestedByList = UtilityFunctions.CodeGetByTypeIdAndUserId(28, includeCodeId: model.ContinuanceRequestedByID.Value, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            else
                model.ContinuanceRequestedByList = UtilityFunctions.CodeGetByTypeIdAndUserId(28, agencyId: UserManager.UserExtended.CaseNumberAgencyID);


            model.ControlType = UtilityFunctions.GetNoteControlType("CaseOpening/Hearing", noteId: model.NoteID);


            return View(model);
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing, IsCasePage = true)]
        public virtual JsonResult HearingSave(HearingModel model)
        {

            if (!UserManager.IsUserAccessTo(SecurityToken.AddHearing))
            {
                return Json(new { Status = "Fail", URL = "/Home/AccessDenied" });
            }

            if (model.HearingID > 0)
            {
                return Json(new { Status = "Done" });
            }

            var hearingId = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingInsert_sp", new pd_HearingInsert_spParams
            {
                CaseID = UserManager.UserExtended.CaseID,
                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                BatchLogJobID = Guid.NewGuid(),
                HearingCourtDepartmentCodeID = model.DepartmentID,
                HearingDateTime = DateTime.Parse(model.HearingDate + " " + model.HearingTime),
                HearingMediaPresentFlag = model.MediaPresent ? 1 : 0,
                HearingOfficerPersonID = model.HearingOfficerID,
                HearingTypeCodeID = model.HearingTypeID,
                UserID = UserManager.UserExtended.UserID,
                RecordStateID = 1,
                HearingRequestedByCodeID = model.ContinuanceRequestedByID,
            }).FirstOrDefault();
            model.HearingID = (int)hearingId;
            if (!string.IsNullOrEmpty(model.Note))
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_NoteInsert_sp", new pd_NoteInsert_spParams()
                {
                    NoteEntitySystemValueTypeID = 113,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = (int)hearingId,
                    NoteTypeCodeID = 0,
                    NoteEntry = model.Note,
                    CaseID = UserManager.UserExtended.CaseID,

                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
            }


            if (!string.IsNullOrEmpty(model.PetitionsSelectedIds))
            {
                var petitionIds = model.PetitionsSelectedIds.Split(',');
                foreach (var petitionId in petitionIds)
                {
                    var id = petitionId.Split('|')[0].ToInt();

                    int? resultId = petitionId.Split('|')[1].ToInt();
                    if (resultId == 0 && model.GlobalResultID > 0)
                    {
                        resultId = model.GlobalResultID;
                    }

                    if (resultId == 0) resultId = null;

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        HearingID = (int)hearingId,
                        PetitionID = id,
                        HearingPersonResultCodeID = resultId,
                        AppearanceRequiredFlag = petitionId.Split('|')[2].ToInt()

                    }).FirstOrDefault();

                }
            }
            var AppearingAttorneyID = 0;
            if (!string.IsNullOrEmpty(model.AppearingAttorneyID))
            {
                var aapersonId = AppearingAttorneyID = model.AppearingAttorneyID.Split('|')[0].ToInt();

                var aaroleId = model.AppearingAttorneyID.Split('|')[1].ToInt();

                var aahearingAttorneyAttendanceId = model.AppearingAttorneyID.Split('|')[2];

                if (aahearingAttorneyAttendanceId != model.HearingAttendanceID.ToString() || model.HearingAttendanceID == 0)
                {

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = (int)hearingId,
                        NewAttendingAttorneyPersonID = aaroleId > 0 ? (int?)null : aapersonId,
                        RoleID = aaroleId > 0 ? aaroleId : 0,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,

                    }).FirstOrDefault();
                }

            }

            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionAutoUpdate_sp", new pd_HearingPetitionAutoUpdate_spParams()
            {
                HearingID = (int)hearingId,

                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            });
            UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);
            if (model.HourTypeID > 0 || model.PhaseID > 0 || (model.Hours > 0))
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_WorkInsertByHearingID_sp", new pd_WorkInsertByHearingID_spParams()
                {
                    HearingID = (int)hearingId,
                    AttorneyPersonID = AppearingAttorneyID,
                    WorkDescriptionCodeID = model.HourTypeID,
                    WorkHours = model.Hours ?? 0,
                    WorkPhaseCodeID = model.PhaseID,

                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();

            }


            if (model.UpdateDepartment)
            {
                var caseGet = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
                if (caseGet != null)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_CaseUpdate_sp", new pd_CaseUpdate_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        AgencyID = caseGet.AgencyID,
                        CaseAppointmentDate = caseGet.CaseAppointmentDate,
                        CaseClosedDate = caseGet.CaseClosedDate,
                        CaseID = caseGet.CaseID,
                        CaseNumber = caseGet.CaseNumber,
                        CasePanelCase = caseGet.CasePanelCase,
                        DepartmentID = model.DepartmentID
                    });
                }

            }
            UtilityFunctions.HearingContinuanceReasonAndRequestDataInsert(model);

            return Json(new { Status = "Done", Data = model });
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing, IsCasePage = true)]
        [HttpPost]

        public virtual ActionResult HearingDelete(int id)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.DeleteHearing))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }



            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingDelete_sp", new pd_HearingDelete_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                ID = id,

                RecordTimeStamp = null
            });



            return Json(new { Status = "Done" });
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing, IsCasePage = true)]
        public virtual JsonResult HearingUpdate(HearingModel model)
        {

            if (!UserManager.IsUserAccessTo(SecurityToken.EditHearing))
            {
                return Json(new { Status = "Fail", URL = "/Home/AccessDenied" });
            }
            var updateAutoUpdateSp = false;

            // Update Case Deparment, if user selects YES from prompt:
            if (model.UpdateDepartment)
            {
                var caseGet = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
                if (caseGet != null)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_CaseUpdate_sp", new pd_CaseUpdate_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        AgencyID = caseGet.AgencyID,
                        CaseAppointmentDate = caseGet.CaseAppointmentDate,
                        CaseClosedDate = caseGet.CaseClosedDate,
                        CaseID = caseGet.CaseID,
                        CaseNumber = caseGet.CaseNumber,
                        CasePanelCase = caseGet.CasePanelCase,
                        DepartmentID = model.DepartmentID
                    });
                }

            }

            // 2. Hearing Update - (Type, Date/Time, Officer, Department, Continuance Requested By, Media Flag is updated)
            var hearingGet =
                  UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                      new pd_HearingGet_spParams
                      {
                          UserID = UserManager.UserExtended.UserID,

                          BatchLogJobID = Guid.NewGuid(),
                          HearingID = model.HearingID
                      }).FirstOrDefault();
            if (hearingGet != null)
            {
                if (hearingGet.HearingCourtDepartmentCodeID != model.DepartmentID ||
                    hearingGet.HearingDateTime != DateTime.Parse(model.HearingDate + " " + model.HearingTime) ||
                    hearingGet.HearingMediaPresentFlag != (model.MediaPresent ? 1 : 0) ||
                    hearingGet.HearingOfficerPersonID != model.HearingOfficerID ||
                    hearingGet.HearingTypeCodeID != model.HearingTypeID ||
                    hearingGet.HearingRequestedByCodeID != model.ContinuanceRequestedByID)
                {
                    updateAutoUpdateSp = true;
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingUpdate_sp", new pd_HearingInsert_spParams
                    {
                        HearingID = model.HearingID,
                        CaseID = UserManager.UserExtended.CaseID,
                        AgencyID = hearingGet.AgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        HearingCourtDepartmentCodeID = model.DepartmentID,
                        HearingDateTime = DateTime.Parse(model.HearingDate + " " + model.HearingTime),
                        HearingMediaPresentFlag = model.MediaPresent ? 1 : 0,
                        HearingOfficerPersonID = model.HearingOfficerID,
                        HearingTypeCodeID = model.HearingTypeID,
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1,
                        HearingRequestedByCodeID = model.ContinuanceRequestedByID,
                        HearingFollowedRecommendations = hearingGet.HearingFollowedRecommendations,
                        HearingInvoiceAmount = hearingGet.HearingInvoiceAmount,
                        HearingResultCodeID = hearingGet.HearingResultCodeID

                    });
                }
            }
            // Hearing Note
            if (!string.IsNullOrEmpty(model.Note))
            {
                if ((model.NoteID.HasValue && model.NoteID.Value == 0) || model.NoteID == null)
                {
                    UtilityFunctions.NoteInsert(113, 123, model.HearingID, 0, "", model.Note, hearingId: model.HearingID);

                }
                else if (model.NoteID.HasValue && model.NoteID > 0)
                {
                    var note = UtilityFunctions.NoteGet(model.NoteID.Value);
                    if (note != null)
                    {
                        UtilityFunctions.NoteUpdate(note.NoteID.Value, note.NoteEntityCodeID.Value, note.NoteEntityTypeCodeID.Value, model.HearingID, note.NoteTypeCodeID.Value, "", model.Note, hearingId: model.HearingID, petitionId: note.PetitionID, agencyId: note.AgencyID);

                    }

                }
            }
            else if (model.NoteID.HasValue && model.NoteID > 0)
            {
                UtilityFunctions.NoteDelete(model.NoteID.Value);
            }

            // Update / Add Petition Result
            if (!string.IsNullOrEmpty(model.PetitionsSelectedIds))
            {
                var petitionIds = model.PetitionsSelectedIds.Split(',');
                foreach (var petitionId in petitionIds)
                {
                    var id = petitionId.Split('|')[0].ToInt();
                    var hearingpetitionkey = petitionId.Split('|')[3].ToInt();
                    var isChecked = petitionId.Split('|')[4].ToInt() == 1;
                    var isResultChange = petitionId.Split('|')[5].ToInt() == 1;
                    var selected = petitionId.Split('|')[6].ToInt();
                    var closedate = petitionId.Split('|')[2];
                    int? resultId = petitionId.Split('|')[1].ToInt();
                    var orderedToAppear = petitionId.Split('|')[7].ToInt();
                    if (resultId == 0 && model.GlobalResultID > 0)
                    {
                        isResultChange = true;
                        resultId = model.GlobalResultID;
                    }

                    if (resultId == 0) resultId = null;

                    if (hearingpetitionkey > 0 && isChecked && selected == 1)//update
                    {
                        if (isResultChange)
                        {
                            updateAutoUpdateSp = true;
                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", new pd_HearingPetitionUpdate_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                HearingID = model.HearingID,
                                PetitionID = id,
                                HearingPetitionResultCodeID = resultId.HasValue ? resultId.Value : 0,


                            });
                        }
                    }
                    else if (hearingpetitionkey > 0 && isChecked == false && selected == 1)
                    {
                        if (closedate.IsNullOrEmpty())//delete
                        {
                            updateAutoUpdateSp = true;
                            //issuw with  10117830 case and url is http://localhost:61548/CaseOpening/Hearing/739A021F6E4F4D563E2EF174F0B0A4AF
                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionDelete_sp", new pd_HearingPetitionDelete_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                ID = hearingpetitionkey,
                                ID2 = model.HearingID
                            });
                        }
                        else//update
                        {
                            updateAutoUpdateSp = true;
                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", new pd_HearingPetitionUpdate_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                HearingID = model.HearingID,
                                PetitionID = id,
                                HearingPetitionResultCodeID = resultId.Value,


                            });
                        }

                    }
                    else if (isChecked && selected == 0)
                    {
                        updateAutoUpdateSp = true;
                        UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            HearingID = model.HearingID,
                            PetitionID = id,
                            HearingPersonResultCodeID = resultId,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID
                            ,
                            AppearanceRequiredFlag = orderedToAppear
                        }).FirstOrDefault();
                    }




                }
            }

            //Attorney Update
            var AppearingAttorneyID = 0;
            if (!string.IsNullOrEmpty(model.AppearingAttorneyID))
            {

                var aapersonId = AppearingAttorneyID = model.AppearingAttorneyID.Split('|')[0].ToInt();

                var aaroleId = model.AppearingAttorneyID.Split('|')[1].ToInt();

                var aahearingAttorneyAttendanceId = model.AppearingAttorneyID.Split('|')[2];

                if (aahearingAttorneyAttendanceId != model.HearingAttendanceID.ToString() || model.HearingAttendanceID == 0)
                {
                    if (model.HearingAttendanceID > 0)
                    {
                        UtilityFunctions.DeleteRecord("pd_HearingAttendanceDelete_sp", model.HearingAttendanceID);
                    }

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = model.HearingID,
                        NewAttendingAttorneyPersonID = aaroleId > 0 ? aapersonId : aapersonId,
                        RoleID = aaroleId > 0 ? aaroleId : 0,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,

                    }).FirstOrDefault();
                }

            }
            else if (model.HearingAttendanceID > 0)
            {
                UtilityFunctions.DeleteRecord("pd_HearingAttendanceDelete_sp", model.HearingAttendanceID);

            }


            //Hours
            if ((model.HourTypeID > 0 || model.PhaseID > 0 || model.Hours > 0) && model.WorkID == 0)
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_WorkInsertByHearingID_sp", new pd_WorkInsertByHearingID_spParams()
                {
                    HearingID = model.HearingID,
                    AttorneyPersonID = AppearingAttorneyID,
                    WorkDescriptionCodeID = model.HourTypeID,
                    WorkHours = model.Hours ?? 0,
                    WorkPhaseCodeID = model.PhaseID,

                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();

            }
            else if (model.WorkID > 0)
            {
                if (model.HourTypeID == 0 && !model.Hours.HasValue && model.PhaseID == 0)
                {
                    UtilityFunctions.DeleteRecord("pd_WorkDelete_sp", model.WorkID);
                }
                else if (model.HourTypeID > 0)
                {
                    var workGet = UtilityService.ExecStoredProcedureWithResults<pd_WorkGet_spResult>("pd_WorkGet_sp", new pd_WorkGet_spParams()
                    {
                        WorkID = model.WorkID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                    if (workGet != null)
                    {

                        if (workGet.WorkHours != model.Hours || workGet.WorkPhaseCodeID != model.PhaseID || workGet.WorkDescriptionCodeID != model.HourTypeID || workGet.PersonID != AppearingAttorneyID)
                        {
                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_WorkUpdate1_sp", new pd_WorkUpdate1_spParams()
                            {
                                WorkID = model.WorkID,
                                AgencyID = workGet.AgencyID,
                                CaseID = UserManager.UserExtended.CaseID,
                                HearingID = model.HearingID <= 0 ? -1 : model.HearingID,
                                PersonID = AppearingAttorneyID,
                                WorkDescriptionCodeID = model.HourTypeID,
                                WorkHours = model.Hours,
                                WorkPhaseCodeID = model.PhaseID,
                                RecordStateID = workGet.RecordStateID,
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                WorkEndDate = workGet.WorkEndDate,
                                WorkHoursOverTime = workGet.WorkHoursOverTime,
                                WorkMileage = workGet.WorkMileage,
                                WorkStartDate = workGet.WorkStartDate,
                                WorkIVeEligibleCodeID = -1,
                            });
                        }
                    }
                }

            }
            foreach (var item in model.OrderedToAppearModelList)
            {

                UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_HearingAttendanceIUD_sp",
                  new qcal_AS_HearingAttendanceIUD_spParams()
                  {
                      HearingID = model.HearingID,
                      AttendedFlag = item.HA_ID > 0 ? item.HA_AttendedFlag : -1,
                      CounselPersonID = item.HA_CounselPersonID,
                      FillinCounselPersonID = item.HA_FillinCounselPersonID,

                      Placement = item.HA_Placement,
                      RoleID = item.HA_RoleID,

                      UserID = UserManager.UserExtended.UserID,
                      BatchLogJobID = Guid.NewGuid(),
                      AppearanceRequiredFlag = item.HA_AppearanceRequiredFlag,
                      IUD = item.HA_ID > 0 ? "UPDATE" : "INSERT",
                      HearingAttendanceID = item.HA_ID
                  });
            }

            //Attendance
            if (model.HearingAttendance.Any())
            {
                int roleId = 0;
                int? personId = null;
                int attandanceId = 0;
                if (!string.IsNullOrEmpty(model.AppearingAttorneyID))
                {
                    personId = AppearingAttorneyID = model.AppearingAttorneyID.Split('|')[0].ToInt();

                    roleId = model.AppearingAttorneyID.Split('|')[1].ToInt();

                    attandanceId = model.AppearingAttorneyID.Split('|')[2].ToInt();
                }
                foreach (var item in model.HearingAttendance)
                {
                    //insert
                    var pd_HearingAttendanceInsert_spParams = new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingAttendanceID = item.AttendanceID.Value,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        HearingID = model.HearingID,
                        RoleID = item.RoleID.Value,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        AttendedFlag = item.IsSelected ? 1 : 0
                    };

                    if (roleId == 0)
                        pd_HearingAttendanceInsert_spParams.NewAttendingAttorneyPersonID = personId;

                    var hearingAttendanceID = UtilityService.ExecStoredProcedureWithResults<int>("pd_HearingAttendanceInsert_sp", pd_HearingAttendanceInsert_spParams).FirstOrDefault();
                    //if (item.AttendanceID != 0 && !item.IsSelected)
                    //{
                    //    //delete
                    //    var pd_HearingAttendanceDelete_spParams = new pd_HearingAttendanceDelete_spParams()
                    //    {
                    //        ID = item.AttendanceID.Value,
                    //        RecordStateID = 10,
                    //        LoadOption = "HearingAttendance",
                    //        UserID = UserManager.UserExtended.UserID,
                    //        BatchLogJobID = Guid.NewGuid()

                    //    };
                    //    var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingAttendanceDelete_sp", pd_HearingAttendanceDelete_spParams).ToList();
                    //}
                }
            }
            if (model.ClientStatusList.Any())
            {
                foreach (var item in model.ClientStatusList)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("ClientStatusIUD_sp",
                            new LALoDep.Domain.qcal.ClientStatusIUD_spParams()
                            {
                                HearingID = model.HearingID,

                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                CS_CodeID = item.CS_CodeID,
                                CS_ID = item.CS_ID,
                                CS_PersonID = item.CS_PersonID,
                                CS_StartDate = item.CS_StartDate.ToDateTimeNullableValue(),
                                HearingDateTime = hearingGet.HearingDateTime,
                                IUD = "Auto"
                            });
                }
            }

            if (updateAutoUpdateSp)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionAutoUpdate_sp", new pd_HearingPetitionAutoUpdate_spParams()
                {
                    HearingID = model.HearingID,

                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                });

            }
            var alertMessage = UtilityService.ExecStoredProcedureWithResults<Advisement_GetAlert_spResult>("Advisement_GetAlert_sp", new Advisement_GetAlert_spParams()
            {
                HearingID = model.HearingID,
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault().AdvisementAlert;

            UtilityFunctions.HearingContinuanceReasonAndRequestDataInsert(model);


            UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);
            return Json(new { Status = "Done", Data = model, AlertMessage = alertMessage });
        }




    }
}