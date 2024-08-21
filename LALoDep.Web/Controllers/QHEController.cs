using LALoDep.Domain.pd_Conflict;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Models.CaseOpening;
using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Models;
using LALoDep.Domain.pd_Allegation;
using Omu.ValueInjecter;
using LALoDep.Domain.pd_Users;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.Advisement;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_wt;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class QHEController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public QHEController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        #region Step 1. QHE Hearing
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing)]
        public virtual ActionResult QHEHearing(string id, string page = "")
        {
            TempData["SourcePage"] = page;
            int hearingId = id.ToDecrypt().ToInt();
            var hearingGet =
                   UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                       new pd_HearingGet_spParams
                       {
                           UserID = UserManager.UserExtended.UserID,

                           BatchLogJobID = Guid.NewGuid(),
                           HearingID = hearingId
                       }).FirstOrDefault();
            if (hearingGet != null)
            {
                UserManager.UpdateCaseStatusBar(hearingGet.CaseID ?? 0);
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
                ContinuanceRequestedByList = UtilityFunctions.CodeGetByTypeIdAndUserId(28, agencyId: UserManager.UserExtended.CaseNumberAgencyID),

                HearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseID_spResults>("pd_HearingGetByCaseID_sp", new LALoDep.Domain.pd_Hearing.pd_HearingGetByCaseID_spParams
                {
                    UserID = UserManager.UserExtended.UserID,

                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID
                }).ToList(),


                HoursLabel = "Hours (use tenths for partial hrs)"
            };

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


            var hearingResults = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults_ForHearingResult>(
                  "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                  {
                      CodeTypeID = 11,
                      SortOption = "IncludeInActive",
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserManager.UserExtended.UserID,
                      AgencyID = UserManager.UserExtended.CaseNumberAgencyID

                  }).ToList();

            model.GlobalResultList = hearingResults.Where(x => x.ActiveFlag == 1).Select(x => new SelectListItem() { Text = x.CodeDisplay, Value = x.CodeID.ToString() });


            var caseDefault = UtilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
                    new pd_CaseGetDefaults_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,

                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID
                    }).FirstOrDefault();

            if (caseDefault != null)
            {
                model.HoursNotRequiredBeforeHearingDate = caseDefault.HoursNotRequiredBeforeHearingDate;
                model.HearingTime = caseDefault.DefaultHearingTime;
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
            model.PetitionType602List = UtilityFunctions.CodeGetBySystemValueTypeId(179, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.HearingType602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(239, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            model.HearingResult602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(240, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            model.CourtDepartment602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(241, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.HearingResultContinuanceList = UtilityFunctions.CodeGetBySystemValueTypeId(47, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.HearingTypeNonHearingEventList = UtilityFunctions.CodeGetBySystemValueTypeId(214, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.HearingResultFutureHearingsList = UtilityFunctions.CodeGetBySystemValueTypeId(207, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.HearingResultListForHoursNotRequiredValidation = UtilityFunctions.CodeGetBySystemValueTypeId(262, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.HearingTypeHoursNotRequired = UtilityFunctions.CodeGetBySystemValueTypeId(265, agencyId: UserManager.UserExtended.CaseNumberAgencyID);


            var summary = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (summary != null)
            {
                model.CaseDepartmentID = summary.DepartmentID ?? 0;

            }
            if (!string.IsNullOrEmpty(id))
            {

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


                    model.HourTypeParentList = UtilityService.ExecStoredProcedureWithResults<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults>(
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
                }
                model.ControlType = UtilityFunctions.GetNoteControlType("QHE/QHEHearing", noteId: model.NoteID);
                ViewBag.AllHearingResults = hearingResults;

                model.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10, includeCodeId: model.HearingTypeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, includeCodeId: model.DepartmentID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HourTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(222, includeCodeId: model.HourTypeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: model.PhaseID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            }



            return View(model);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing, IsCasePage = true)]
        public virtual JsonResult QHEHearingUpdate(HearingModel model)
        {

            if (!UserManager.IsUserAccessTo(SecurityToken.EditHearing))
            {
                return Json(new { isSuccess = false, URL = "/Home/AccessDenied" });
            }


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
                    var orderedToAppear = petitionId.Split('|')[7].ToInt();
                    var closedate = petitionId.Split('|')[2];
                    int? resultId = petitionId.Split('|')[1].ToInt();
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
                        UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            HearingID = model.HearingID,
                            PetitionID = id,
                            HearingPersonResultCodeID = resultId,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
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

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = model.HearingID,
                        NewAttendingAttorneyPersonID = aaroleId > 0 ? aapersonId : aapersonId,
                        RoleID = aaroleId > 0 ? aaroleId : 0,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                }

            }
            else if (model.HearingAttendanceID > 0)
            {
                UtilityFunctions.DeleteRecord("pd_HearingAttendanceDelete_sp", model.HearingAttendanceID);

            }


            //Hours
            if ((model.Hours > 0 || model.HourTypeID > 0 || model.PhaseID > 0) && model.WorkID == 0)
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
                else if (model.Hours > 0 || model.HourTypeID > 0 || model.PhaseID > 0)
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
                                WorkHours = model.Hours ?? 0,
                                WorkPhaseCodeID = model.PhaseID,
                                RecordStateID = workGet.RecordStateID,
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                WorkEndDate = workGet.WorkEndDate,
                                WorkHoursOverTime = workGet.WorkHoursOverTime,
                                WorkMileage = workGet.WorkMileage,
                                WorkStartDate = workGet.WorkStartDate,
                                WorkIVeEligibleCodeID = -1
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


            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionAutoUpdate_sp", new pd_HearingPetitionAutoUpdate_spParams()
            {
                HearingID = model.HearingID,

                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            });

            UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);


            if (model.buttonId == 4)
            {
                return NextQHECase(model.HearingID);
            }
            var alertMessage = UtilityService.ExecStoredProcedureWithResults<Advisement_GetAlert_spResult>("Advisement_GetAlert_sp", new Advisement_GetAlert_spParams()
            {
                HearingID = model.HearingID,
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault().AdvisementAlert;



            return Json(new
            {
                isSuccess = true,
                Data = model,
                AlertMessage = alertMessage
            });
        }
        #endregion

        #region Step 2. QHE Attendance
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttendancePages, PageSecurityItemID = SecurityToken.ViewAttendance)]
        public virtual ActionResult QHEAttendance(string id)
        {
            var viewModel = new AttendanceViewModel();

            var hearingId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                hearingId = id.ToDecrypt().ToInt();
                var pd_HearingGet_spParams = new pd_HearingGet_spParams()
                {
                    HearingID = hearingId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", pd_HearingGet_spParams).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.AgencyID = hearingInfo.AgencyID;

                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                    viewModel.HearingOfficer = hearingInfo.HearingJudge;
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.HearingType = hearingInfo.HearingTypeCodeValue;
                }
            }

            var noteInfo = UtilityFunctions.NoteGetByEntity(hearingId, 113, 125).FirstOrDefault();
            if (noteInfo != null)
            {
                viewModel.NoteID = noteInfo.NoteID;
                viewModel.NoteEntityCodeID = noteInfo.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteInfo.NoteEntityTypeCodeID;
                viewModel.EntityPrimaryKeyID = noteInfo.EntityPrimaryKeyID;
                viewModel.NoteTypeCodeID = noteInfo.NoteTypeCodeID;
                viewModel.NoteSubject = noteInfo.NoteSubject;
                viewModel.NoteEntry = noteInfo.NoteEntry;
                viewModel.NoteCaseID = noteInfo.CaseID;
                viewModel.NoteRecordStateID = noteInfo.RecordStateID;
            }

            var pd_RoleGetForHearingAttendingAttorney_spParams = new pd_RoleGetForHearingAttendingAttorney_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = hearingId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var appearingAttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForHearingAttendingAttorney_spResult>("pd_RoleGetForHearingAttendingAttorney_sp", pd_RoleGetForHearingAttendingAttorney_spParams).ToList();
            viewModel.AppearingAttorney = appearingAttorneyList.Select(x => new SelectListItem
            {
                Value = x.RoleID + "_" + x.PersonID + "_" + x.HearingAttendanceID,
                Text = x.PersonNameDisplay
            }).ToList();

            var appearingAttorney = appearingAttorneyList.FirstOrDefault(x => x.AttendingHearingFlag == 1);
            if (appearingAttorney != null)
            {
                viewModel.AppearingAttorneyID = appearingAttorney.RoleID + "_" + appearingAttorney.PersonID + "_" + appearingAttorney.HearingAttendanceID;

                viewModel.OldAppearingAttorneyID = appearingAttorney.PersonID;
                viewModel.HearingAttendanceID = appearingAttorney.HearingAttendanceID;
                viewModel.HearingAttandanceRoleID = appearingAttorney.RoleID;
            }

            var pd_HearingAttendanceGetByHearingID_spParams = new pd_HearingAttendanceGetByHearingID_spParams()
            {
                HearingID = hearingId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.HearingAttendance = UtilityService.ExecStoredProcedureWithResults<pd_HearingAttendanceGetByHearingID_spResult>("pd_HearingAttendanceGetByHearingID_sp", pd_HearingAttendanceGetByHearingID_spParams)
                                                    .Select(x => new HearingAttendanceListViewModel()
                                                    {
                                                        PersonNameLast = x.PersonNameLast,
                                                        PersonNameFirst = x.PersonNameFirst,
                                                        RoleID = x.RoleID,
                                                        RoleTypeID = x.RoleTypeID,
                                                        RoleType = x.RoleType,
                                                        RoleClient = x.RoleClient,
                                                        AttendanceID = x.AttendanceID,
                                                        IsSelected = x.AttendedFlag == 1,
                                                        IsEditable = x.Editable == 1 ? true : false
                                                    }).ToList();

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttendancePages, PageSecurityItemID = SecurityToken.EditAttendance)]
        [HttpPost]
        public virtual JsonResult QHEAttendanceSave(AttendanceViewModel viewModel)
        {
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(113, 125, viewModel.HearingID.Value, 16, null, viewModel.NoteEntry, viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.EntityPrimaryKeyID.Value,
                                                            viewModel.NoteTypeCodeID.Value, null, viewModel.NoteEntry, viewModel.HearingID);

            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new LALoDep.Domain.pd_Conflict.pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }

            int roleId = 0;
            int? personId = null;
            int attandanceId = 0;
            if (!string.IsNullOrEmpty(viewModel.AppearingAttorneyID))
            {
                var hearingPerson = viewModel.AppearingAttorneyID.Split('_');
                roleId = hearingPerson[0].ToInt();
                personId = hearingPerson[1].ToInt();
                attandanceId = hearingPerson[2].ToInt();
            }


            //Attendance
            if (viewModel.HearingAttendance.Any())
            {
                foreach (var item in viewModel.HearingAttendance)
                {
                    if (item.AttendanceID == 0 && item.IsSelected)
                    {
                        //insert
                        var pd_HearingAttendanceInsert_spParams = new pd_HearingAttendanceInsert_spParams()
                        {
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            HearingID = viewModel.HearingID.Value,
                            RoleID = item.RoleID.Value,
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };

                        if (roleId == 0)
                            pd_HearingAttendanceInsert_spParams.NewAttendingAttorneyPersonID = personId;

                        var hearingAttendanceID = UtilityService.ExecStoredProcedureWithResults<int>("pd_HearingAttendanceInsert_sp", pd_HearingAttendanceInsert_spParams).FirstOrDefault();
                    }
                    else if (item.AttendanceID != 0 && !item.IsSelected)
                    {
                        //delete
                        var pd_HearingAttendanceDelete_spParams = new pd_HearingAttendanceDelete_spParams()
                        {
                            ID = item.AttendanceID.Value,
                            RecordStateID = 10,
                            LoadOption = "HearingAttendance",
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };
                        var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingAttendanceDelete_sp", pd_HearingAttendanceDelete_spParams).ToList();
                    }
                }
            }

            if (viewModel.OldAppearingAttorneyID != personId)
            {

                if (viewModel.HearingAttendanceID != 0)
                {
                    //delete
                    var pd_HearingAttendanceDelete_spParams = new pd_HearingAttendanceDelete_spParams()
                    {
                        ID = viewModel.HearingAttendanceID,
                        RecordStateID = 10,
                        LoadOption = "HearingAttendance",
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingAttendanceDelete_sp", pd_HearingAttendanceDelete_spParams).ToList();
                }

                if (personId.HasValue && personId != 0)
                {
                    var pd_HearingAttendanceInsert_spParams = new pd_HearingAttendanceInsert_spParams()
                    {
                        AgencyID = viewModel.AgencyID ?? UserManager.UserExtended.CaseNumberAgencyID,
                        HearingID = viewModel.HearingID.Value,
                        RoleID = roleId,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };

                    if (roleId == 0)
                        pd_HearingAttendanceInsert_spParams.NewAttendingAttorneyPersonID = personId;

                    var hearingAttendanceID = UtilityService.ExecStoredProcedureWithResults<int>("pd_HearingAttendanceInsert_sp", pd_HearingAttendanceInsert_spParams).FirstOrDefault();
                }
            }


            if (viewModel.buttonID == 3 && viewModel.HearingID.HasValue)
            {
                return NextQHECase(viewModel.HearingID ?? 0);
            }
            return Json(new { isSuccess = true, nextHearingId = string.Empty });
        }

        #endregion

        #region Step 3. QHE Plea.

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MainPleaPages, PageSecurityItemID = SecurityToken.ViewPlea)]
        public virtual ActionResult QHEPlea(string id)
        {
            var viewModel = new PleaViewModel();

            var hearingId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                hearingId = id.ToDecrypt().ToInt();
                var pd_HearingGet_spParams = new pd_HearingGet_spParams()
                {
                    HearingID = hearingId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", pd_HearingGet_spParams).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                    viewModel.HearingJudge = hearingInfo.HearingJudge;
                    viewModel.HearingType = hearingInfo.HearingTypeCodeValue;
                    viewModel.HearingID = hearingId;
                    viewModel.AgencyID = hearingInfo.AgencyID;
                    viewModel.CaseID = hearingInfo.CaseID;
                }

                viewModel.GloabalPlea = UtilityFunctions.CodeGetByTypeIdAndUserId(48, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                viewModel.HearingRespondentList = UtilityService.ExecStoredProcedureWithResults<pd_HearingPleaGetByHearingIDRespondent_spResult>("pd_HearingPleaGetByHearingIDRespondent_sp", pd_HearingGet_spParams).ToList();
                viewModel.HearingChildernList = UtilityService.ExecStoredProcedureWithResults<pd_HearingPleaGetByHearingIDChildren_spResult>("pd_HearingPleaGetByHearingIDChildren_sp", pd_HearingGet_spParams).ToList();

            }


            return View(viewModel);
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MainPleaPages, PageSecurityItemID = SecurityToken.EditPlea)]
        [HttpPost]
        public virtual ActionResult QHEPlea(PleaViewModel viewModel)
        {
            foreach (var plea in viewModel.HearingRespondentList)
            {
                //use plea dropdown value
                int intPleaTypeID = plea.PleaTypeID ?? 0;

                // check for Respondent plea type is selected
                if (intPleaTypeID == 0 && viewModel.HearingRespondentGlobalList != null && viewModel.HearingRespondentGlobalList.Any())
                {
                    var globalPlea = viewModel.HearingRespondentGlobalList.FirstOrDefault(x => x.PersonID == plea.PersonID);
                    if (globalPlea != null)
                        intPleaTypeID = globalPlea.PleaTypeID ?? 0;
                }

                //check if global plea is selected
                if (intPleaTypeID == 0 && viewModel.GloabalPleaTypeCodeID.HasValue)
                    intPleaTypeID = viewModel.GloabalPleaTypeCodeID ?? 0;

                // if plea is selected any where then update it
                if (intPleaTypeID != 0)
                {
                    if (plea.PleaID.HasValue && plea.PleaID > 0) // Update plea
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingPleaUpdate_sp",
                                            new pd_HearingPleaUpdate_spParams
                                            {
                                                HearingPleaID = plea.PleaID,
                                                HearingID = viewModel.HearingID,
                                                PetitionRoleID = plea.PetitionRoleID,

                                                HearingPleaCodeID = intPleaTypeID,

                                                RecordStateID = 1,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            }).FirstOrDefault();
                    }
                    else // Add new plea
                    {
                        plea.PleaID = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingPleaInsert_sp",
                                            new pd_HearingPleaInsert_spParams
                                            {
                                                HearingID = viewModel.HearingID,
                                                PetitionRoleID = plea.PetitionRoleID,

                                                HearingPleaCodeID = intPleaTypeID,

                                                RecordStateID = 1,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            }).FirstOrDefault().ToInt();
                    }
                }

            }
            if (viewModel.buttonID == 3)
            {
                return NextQHECase(viewModel.HearingID ?? 0);
            }
            return Json(new { isSuccess = true });
        }
        #endregion Main Page->Plea

        #region Step 4. QHE Court Position


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.QHEPosPages, PageSecurityItemID = SecurityToken.ViewCourtPosition)]
        public virtual ActionResult QHEPos(string id)
        {
            var viewModel = new PosViewModel();

            var hearingId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                hearingId = id.ToDecrypt().ToInt();
                var hearingSP_Params = new pd_HearingGet_spParams()
                {
                    HearingID = hearingId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", hearingSP_Params).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                    viewModel.HearingJudge = hearingInfo.HearingJudge;
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.HearingType = hearingInfo.HearingTypeCodeValue;
                }
                viewModel.PeopleOnHearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOpinionGetByHearingID_spResult>("pd_HearingOpinionGetByHearingID_sp", hearingSP_Params).ToList();
            }

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.QHEPosPages, PageSecurityItemID = SecurityToken.AddPos)]
        public virtual ActionResult HearingOpinion(string id, string opinionid, string enhearingID, string ennoteID, string enopinion)
        {
            var viewModel = new HearingOpinionViewModel();
            if (!string.IsNullOrEmpty(id))
                viewModel.RoleID = id.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(opinionid))
                viewModel.HearingOpinionID = opinionid.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(enhearingID))
                viewModel.HearingID = enhearingID.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(ennoteID))
                viewModel.NoteID = ennoteID.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(enopinion))
                viewModel.OpinionNote = enopinion.ToDecrypt();


            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult HearingOpinionSave(HearingOpinionViewModel viewModel)
        {
            if (viewModel.HearingOpinionID == 0 || viewModel.HearingOpinionID == null)
            {
                var pd_HearingOpinionInsert_spParams = new pd_HearingOpinionInsert_spParams()
                {
                    HearingID = viewModel.HearingID,
                    RoleID = viewModel.RoleID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var insertedID = (int)UtilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingOpinionInsert_sp", pd_HearingOpinionInsert_spParams).FirstOrDefault();
                UtilityFunctions.NoteInsert(118, 123, insertedID, 16, null, viewModel.OpinionNote, viewModel.HearingID);

            }
            else
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, 3284, 3288, viewModel.HearingOpinionID.Value, 16, null, viewModel.OpinionNote, viewModel.HearingID);

                var pd_HearingOpinionUpdate_spParams = new pd_HearingOpinionUpdate_spParams()
                {
                    HearingOpinionID = viewModel.HearingOpinionID.Value,
                    HearingID = viewModel.HearingID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingOpinionUpdate_sp", pd_HearingOpinionUpdate_spParams).FirstOrDefault();
            }
            return Json(new { isSuccess = true });
        }

        #endregion Main Page->Pos

        #region Step 5. QHE Findings And Orders
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.ViewFindingsAndOrders)]
        public virtual ActionResult QHEFindingsAndOrders(string id)
        {

            int hearingId = id.ToDecrypt().ToInt();
            var findings = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderGetByHearingID_spResult>("pd_HearingFindingOrderGetByHearingID_sp",
                            new pd_HearingFindingOrderGetByHearingID_spParams { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            if (!findings.Any() && UserManager.IsUserAccessTo(SecurityToken.AddFindingsAndOrders))
                return RedirectToAction("AddFindingsAndOrders", "QHE", new { id = id, main = true });

            var viewModel = new FindingsAndOrdersListViewModel();

            var person = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderPersonGetByHearingIDHearing_spResult>("pd_HearingFindingOrderPersonGetByHearingIDHearing_sp",
                            new pd_HearingFindingOrderPersonGetByHearingIDHearing_spParams { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            var notices = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderNoticeGetByHearingID_spResult>("pd_HearingFindingOrderNoticeGetByHearingID_sp",
                            new pd_HearingFindingOrderNoticeGetByHearingID_spParams { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            viewModel.FindingAndOrderList = findings.Select(x => new FindingAndOrderListModel
            {
                HearingFindingOrderID = x.HearingFindingOrderID.ToEncrypt(),
                HearingFindingOrderCodeValue = x.HearingFindingOrderCodeValue,
                Person = string.Join(", ", person.Where(p => p.HearingFindingOrderID == x.HearingFindingOrderID).Select(p => p.PersonNameFirst + " " + p.PersonNameLast)),
                Notices = string.Join(", ", notices.Where(n => n.HearingFindingOrderID == x.HearingFindingOrderID).Select(n => n.Notice))
            }).ToList();

            var noteEntry = UtilityFunctions.NoteGetByEntity(hearingId, 113, 124).FirstOrDefault();
            if (noteEntry != null)
                viewModel.NoteEntry = noteEntry.NoteEntry;

            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                            new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;
            }

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.DeleteFindingsAndOrders)]
        public virtual ActionResult DeleteFindingsAndOrders(string id)
        {
            var findingID = id.ToDecrypt().ToInt();
            UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderDelete_sp",
                            new pd_HearingFindingOrderDelete_spParams() { ID = findingID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.AddFindingsAndOrders)]
        public virtual ActionResult AddFindingsAndOrders(string id)
        {
            int hearingId = id.ToDecrypt().ToInt();

            var viewModel = new FindingsAndOrdersAddViewModel();
            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                            new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;

                viewModel.FindingOrderPersonList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderPersonGetByHearingFindingOrderID_sp",
                                    new pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new FindingOrderPerson
                                    {
                                        PersonID = x.PersonID,
                                        PersonDisplayName = x.PersonNameLast + ", " + x.PersonNameFirst,
                                        HearingFindingOrderPersonID = x.HearingFindingOrderPersonID,
                                        RoleID = x.RoleID,
                                        RoleTypeCodeValue = x.RoleTypeCodeValue,
                                        Selected = x.Selected == 1,
                                        IsRoleClient = x.RoleClient == 1
                                    }).ToList();

                viewModel.FindingOrderNoticeList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_sp",
                                    new pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new FindingOrderNotice
                                    {
                                        HearingFindingOrderNoticeID = x.HearingFindingOrderNoticeID,
                                        NoticeID = x.NoticeID,
                                        Notice = x.Notice,
                                        Selected = x.Selected == 1
                                    }).ToList();

                viewModel.FindingOrderTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spResult>("pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_sp",
                                    new pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spParams() { HearingTypeCodeID = hearingInfo.HearingTypeCodeID.Value, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new SelectListItem
                                    {
                                        Text = x.HearingFindingOrderCodeValue,
                                        Value = x.HearingFindingOrderCodeID.ToString()
                                    }).ToList();
            }


            var noteEntry = UtilityFunctions.NoteGetByEntity(hearingId, 113, 124).FirstOrDefault();
            if (noteEntry != null)
            {
                viewModel.NoteID = noteEntry.NoteID;
                viewModel.NoteEntry = noteEntry.NoteEntry;
                viewModel.NoteSubject = noteEntry.NoteSubject;
                viewModel.NoteEntityCodeID = noteEntry.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteEntry.NoteEntityTypeCodeID;

            }

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.AddFindingsAndOrders)]
        [HttpPost]
        public virtual ActionResult AddFindingsAndOrders(FindingsAndOrdersAddViewModel viewModel)
        {
            #region Manage Note
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(113, 124, viewModel.HearingID.Value, 0, null, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID ?? 0, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.HearingID.Value, 0, viewModel.NoteSubject, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }
            #endregion

            if (viewModel.FindingsAndOrders != null && viewModel.FindingsAndOrders.Count > 0)
            {

                foreach (var item in viewModel.FindingsAndOrders)
                {
                    var resultId = UtilityService.ExecStoredProcedureScalar("pd_HearingFindingOrderInsert_sp",
                                           new pd_HearingFindingOrderInsert_spParams()
                                           {
                                               HearingID = viewModel.HearingID,
                                               HearingFindingOrderCodeID = item.CodeHearingFindingOrderID,
                                               RecordStateID = 1,
                                               UserID = UserManager.UserExtended.UserID,
                                               BatchLogJobID = Guid.NewGuid()
                                           });

                    var findingOrderId = resultId.ToInt();

                    if (findingOrderId > 0)
                    {
                        if (item.FindingOrderPersonList != null && item.FindingOrderPersonList.Count > 0)
                        {
                            foreach (var person in item.FindingOrderPersonList)
                            {
                                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderPersonInsertByRoleID_sp",
                                               new pd_HearingFindingOrderPersonInsertByRoleID_spParams()
                                               {
                                                   RoleID = person.RoleID,
                                                   HearingFindingOrderID = findingOrderId,
                                                   RecordStateID = 1,
                                                   UserID = UserManager.UserExtended.UserID,
                                                   BatchLogJobID = Guid.NewGuid()
                                               }).FirstOrDefault();

                            }
                        }

                        if (item.FindingOrderNoticeList != null && item.FindingOrderNoticeList.Count > 0)
                        {
                            foreach (var notice in item.FindingOrderNoticeList)
                            {
                                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderNoticeInsert_sp",
                                               new pd_HearingFindingOrderNoticeInsert_spParams()
                                               {
                                                   HearingFindingOrderID = findingOrderId,
                                                   HearingFindingOrderNoticeCodeID = notice.NoticeID,
                                                   RecordStateID = 1,
                                                   UserID = UserManager.UserExtended.UserID,
                                                   BatchLogJobID = Guid.NewGuid()
                                               }).FirstOrDefault();

                            }
                        }
                    }
                }
            }
            if (viewModel.buttonID == 5)
            {
                return NextQHECase(viewModel.HearingID ?? 0);
            }
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.EditFindingsAndOrders)]
        public virtual ActionResult EditFindingsAndOrders(string id, string fId)
        {
            int hearingId = id.ToDecrypt().ToInt();
            int findingsId = fId.ToDecrypt().ToInt();

            var viewModel = new FindingsAndOrdersEditViewModel();
            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                            new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;


                var finding = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderGet_spResult>("pd_HearingFindingOrderGet_sp",
                            new pd_HearingFindingOrderGet_spParams() { HearingFindingOrderID = findingsId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

                if (finding != null)
                {
                    viewModel.HearingFindingOrderID = finding.HearingFindingOrderID;
                    viewModel.HearingFindingOrderCodeID = finding.HearingFindingOrderCodeID;
                    viewModel.RecordStateID = finding.RecordStateID;
                    viewModel.AgencyID = finding.AgencyID;
                }


                viewModel.FindingOrderPersonList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderPersonGetByHearingFindingOrderID_sp",
                            new pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spParams() { HearingFindingOrderID = findingsId, HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                            .Select(x => new FindingOrderPerson
                            {
                                PersonID = x.PersonID,
                                PersonDisplayName = x.PersonNameLast + ", " + x.PersonNameFirst,
                                HearingFindingOrderPersonID = x.HearingFindingOrderPersonID,
                                RoleID = x.RoleID,
                                RoleTypeCodeValue = x.RoleTypeCodeValue,
                                Selected = x.Selected == 1,
                                IsRoleClient = x.RoleClient == 1
                            }).ToList();


                viewModel.FindingOrderNoticeList = UtilityService.ExecStoredProcedureWithResults<pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spResult>("pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_sp",
                                    new pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spParams() { HearingFindingOrderID = findingsId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new FindingOrderNotice
                                    {
                                        HearingFindingOrderNoticeID = x.HearingFindingOrderNoticeID,
                                        NoticeID = x.NoticeID,
                                        Notice = x.Notice,
                                        Selected = x.Selected == 1
                                    }).ToList();


                viewModel.FindingOrderTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spResult>("pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_sp",
                                    new pd_CodeHearingFindingOrderTypeGetByHearingTypeCodeID_spParams() { HearingTypeCodeID = findingsId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new SelectListItem
                                    {
                                        Text = x.HearingFindingOrderCodeValue,
                                        Value = x.HearingFindingOrderCodeID.ToString()
                                    }).ToList();
            }


            var noteEntry = UtilityFunctions.NoteGetByEntity(hearingId, 113, 124).FirstOrDefault();
            if (noteEntry != null)
            {
                viewModel.NoteID = noteEntry.NoteID;
                viewModel.NoteEntry = noteEntry.NoteEntry;
                viewModel.NoteSubject = noteEntry.NoteSubject;
                viewModel.NoteEntityCodeID = noteEntry.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteEntry.NoteEntityTypeCodeID;

            }

            return View(viewModel);
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.FindingsAndOrders, PageSecurityItemID = SecurityToken.EditFindingsAndOrders)]
        [HttpPost]
        public virtual ActionResult EditFindingsAndOrders(FindingsAndOrdersEditViewModel viewModel)
        {
            #region Manage Note
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(113, 124, viewModel.HearingID.Value, 0, null, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID ?? 0, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.HearingID.Value, 0, viewModel.NoteSubject, viewModel.NoteEntry, hearingId: viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }
            #endregion

            if (viewModel.IsFindingUpdate)
            {
                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderUpdate_sp",
                                                new pd_HearingFindingOrderUpdate_spParams()
                                                {
                                                    HearingFindingOrderID = viewModel.HearingFindingOrderID,
                                                    AgencyID = viewModel.AgencyID,
                                                    HearingID = viewModel.HearingID,
                                                    HearingFindingOrderCodeID = viewModel.HearingFindingOrderCodeID,
                                                    RecordStateID = viewModel.RecordStateID,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).FirstOrDefault();
            }


            if (viewModel.FindingOrderPersonList != null && viewModel.FindingOrderPersonList.Count > 0)
            {
                foreach (var person in viewModel.FindingOrderPersonList)
                {
                    if (person.Selected && !person.HearingFindingOrderPersonID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderPersonInsertByRoleID_sp",
                                           new pd_HearingFindingOrderPersonInsertByRoleID_spParams()
                                           {
                                               RoleID = person.RoleID,
                                               HearingFindingOrderID = viewModel.HearingFindingOrderID,
                                               RecordStateID = 1,
                                               UserID = UserManager.UserExtended.UserID,
                                               BatchLogJobID = Guid.NewGuid()
                                           }).FirstOrDefault();
                    }
                    else if (!person.Selected && person.HearingFindingOrderPersonID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderPersonDelete_sp",
                                                new pd_HearingFindingOrderPersonDelete_spParams()
                                                {
                                                    ID = person.RoleID,
                                                    ID2 = viewModel.HearingFindingOrderID,
                                                    RecordStateID = 10,
                                                    LoadOption = "HearingFindingOrderPerson",
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).FirstOrDefault();
                    }

                }
            }

            if (viewModel.FindingOrderNoticeList != null && viewModel.FindingOrderNoticeList.Count > 0)
            {
                foreach (var notice in viewModel.FindingOrderNoticeList)
                {
                    if (notice.Selected && !notice.HearingFindingOrderNoticeID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderNoticeInsert_sp",
                                          new pd_HearingFindingOrderNoticeInsert_spParams()
                                          {
                                              HearingFindingOrderID = viewModel.HearingFindingOrderID,
                                              HearingFindingOrderNoticeCodeID = notice.NoticeID,
                                              RecordStateID = 1,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid()
                                          }).FirstOrDefault();
                    }
                    else if (!notice.Selected && notice.HearingFindingOrderNoticeID.HasValue)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingFindingOrderNoticeDelete_sp",
                                                new pd_HearingFindingOrderNoticeDelete_spParams()
                                                {
                                                    ID = notice.HearingFindingOrderNoticeID.Value,
                                                    RecordStateID = 10,
                                                    LoadOption = "HearingFindingOrderNotice",
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).FirstOrDefault();
                    }

                }
            }

            return Json(new { isSuccess = true });
        }
        #endregion

        #region Step 6. QHE Allegations
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditAllegation)]
        public virtual ActionResult QHEAllegations(string id)
        {
            var hearingId = id.ToDecrypt().ToInt();
            var viewModel = new QHEAllegationViewModel();

            var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                                    new pd_HearingGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingInfo != null)
            {
                viewModel.HearingID = hearingInfo.HearingID;
                viewModel.HearingType = hearingInfo.HearingType;
                viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToDefaultFormat();
                viewModel.HearingJudge = hearingInfo.HearingJudge;
                viewModel.HearingDept = hearingInfo.HearingDept;

                UserManager.UpdateCaseStatusBar(hearingInfo.CaseID.Value);
            }

            viewModel.Findings = UtilityFunctions.CodeGetByTypeIdAndUserId(68, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            viewModel.Allegations = UtilityService.ExecStoredProcedureWithResults<pd_AllegationGetByHearingID_spResult>("pd_AllegationGetByHearingID_sp",
                                    new pd_AllegationGetByHearingID_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                    .Select(x => new AllegationViewModel
                                    {
                                        PetitionID = x.PetitionID,
                                        PetitionNumber = x.PetitionNumber,
                                        PetitionFileDate = x.PetitionFileDate.ToDefaultFormat(),
                                        ChildFirstName = x.ChildFirstName,
                                        ChildLastName = x.ChildLastName,
                                        AllegationID = x.AllegationID,
                                        AllegationTypeCodeValue = x.AllegationTypeCodeValue,
                                        AllegationTypeCodeID = x.AllegationTypeCodeID,
                                        AllegationFindingCodeID = x.AllegationFindingCodeID,
                                        RecordStateID = x.RecordStateID
                                    }).ToList();

            return View(viewModel);
        }
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditAllegation)]
        [HttpPost]
        public virtual ActionResult QHEAllegations(QHEAllegationViewModel viewModel)
        {
            if (viewModel.Allegations != null && viewModel.Allegations.Count > 0)
            {
                foreach (var item in viewModel.Allegations)
                {
                    int? findingCode = viewModel.GlobalFindingCodeId;

                    if (item.PetitionGlobalFindingCodeId.HasValue)
                        findingCode = item.PetitionGlobalFindingCodeId ?? 0;

                    if (item.IsChanged)
                        findingCode = item.AllegationFindingCodeID;

                    UtilityService.ExecStoredProcedureWithResults<object>("pd_AllegationUpdate_sp", new pd_AllegationUpdate_spParams()
                    {
                        AllegationID = item.AllegationID ?? 0,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        PetitionID = item.PetitionID,
                        AllegationTypeCodeID = item.AllegationTypeCodeID,
                        AllegationFindingCodeID = findingCode,
                        RecordStateID = item.RecordStateID ?? 1,

                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                }

            }

            if (viewModel.buttonID == 3)
            {
                return NextQHECase(viewModel.HearingID ?? 0);
            }

            return Json(new { isSuccess = true });
        }
        #endregion

        #region Step 7. QHE Next Hearing
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddHearing, IsCasePage = true)]
        public virtual ActionResult QHENextHearing(string id)
        {
            var hearingId = id.ToDecrypt().ToInt();
            var viewModel = new QHEHearingViewModel();
            viewModel.HearingID = hearingId;

            var caseDefault = UtilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
                                new pd_CaseGetDefaults_spParams { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid(), CaseID = UserManager.UserExtended.CaseID }).FirstOrDefault();

            if (caseDefault != null)
            {
                viewModel.HearingTime = caseDefault.DefaultHearingTime;
                viewModel.DepartmentID = caseDefault.DefaultHearingCourtDepartmentCodeID ?? 0;
                viewModel.HearingOfficerID = caseDefault.DefaultHearingOfficerPersonID ?? 0;
            }

            viewModel.AppearingAttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForHearingAttendingAttorney_spResult>("pd_RoleGetForHearingAttendingAttorney_sp",
                                                new pd_RoleGetForHearingAttendingAttorney_spParams
                                                {
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    CaseID = UserManager.UserExtended.CaseID
                                                })
                                                .Select(o => new SelectListItem() { Value = o.PersonID.ToString() + "|" + o.RoleID.ToString() + "|" + o.HearingAttendanceID.ToString(), Text = o.PersonNameDisplay }).ToList();


            viewModel.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            viewModel.HearingOfficerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp",
                                            new pd_HearingOfficerGet_spParams
                                            {
                                                UserID = UserManager.UserExtended.UserID,
                                                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                                BatchLogJobID = Guid.NewGuid(),
                                                CaseID = UserManager.UserExtended.CaseID
                                            }).Select(o => new SelectListItem() { Value = o.PersonID.ToString(), Text = o.PersonNameLast + ", " + o.PersonNameFirst }).ToList();
            viewModel.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            viewModel.PetitionList = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetByCaseID_spResult>("pd_PetitionGetByCaseID_sp",
                                                new pd_PetitionGetByCaseID_spParams
                                                {
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    CaseID = UserManager.UserExtended.CaseID
                                                }).ToList();

            viewModel.HearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseID_spResults>("pd_HearingGetByCaseID_sp",
                                                                            new LALoDep.Domain.pd_Hearing.pd_HearingGetByCaseID_spParams
                                                                            {
                                                                                CaseID = UserManager.UserExtended.CaseID,
                                                                                UserID = UserManager.UserExtended.UserID,
                                                                                BatchLogJobID = Guid.NewGuid()
                                                                            }).ToList();

            return View(viewModel);
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddHearing, IsCasePage = true)]
        [HttpPost]
        public virtual ActionResult QHENextHearing(QHENextHearingSaveViewModel viewModel)
        {
            foreach (var hearing in viewModel.Hearings)
            {
                var hearingId = UtilityService.ExecStoredProcedureScalar("pd_HearingInsert_sp", new pd_HearingInsert_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    HearingCourtDepartmentCodeID = hearing.DepartmentID,
                    HearingDateTime = DateTime.Parse(hearing.HearingDate + " " + hearing.HearingTime),
                    HearingOfficerPersonID = hearing.HearingOfficerID,
                    HearingTypeCodeID = hearing.HearingTypeID,
                    HearingMediaPresentFlag = 0,
                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                }).ToInt();

                if (!string.IsNullOrEmpty(hearing.AppearingAttorneyID))
                {
                    var aapersonId = hearing.AppearingAttorneyID.Split('|')[0].ToInt();

                    var aaroleId = hearing.AppearingAttorneyID.Split('|')[1].ToInt();

                    var aahearingAttorneyAttendanceId = hearing.AppearingAttorneyID.Split('|')[2];



                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = (int)hearingId,
                        NewAttendingAttorneyPersonID = aaroleId > 0 ? (int?)null : aapersonId,
                        RoleID = aaroleId > 0 ? aaroleId : 0,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();

                }

                foreach (var item in viewModel.PetitionList)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                    {
                        HearingID = hearingId,
                        PetitionID = item.PetitionID,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    }).FirstOrDefault();
                }
                if (hearing.UpdateCaseDepartment)
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
                            DepartmentID = hearing.DepartmentID
                        });
                    }
                }
            }
            if (viewModel.buttonID == 3)
            {
                return NextQHECase(viewModel.HearingID ?? 0);
            }
            return Json(new { isSuccess = true });
        }
        #endregion

        #region Step 8. QHE Record Time
        #region Record Time List

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeview)]
        public virtual ActionResult QHERecordTime(string id)
        {
            var viewModel = new RecordTimeViewModel();
            viewModel.HearingID = id.ToDecrypt().ToInt();

            var pd_RoleGetByCaseIDClientCriteria_spParams = new pd_RoleGetByCaseIDClientCriteria_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            viewModel.Workers = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDWorkerCriteria_spResult>("pd_RoleGetByCaseIDWorkerCriteria_sp", pd_RoleGetByCaseIDClientCriteria_spParams).ToList();
            viewModel.Clients = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDClientCriteria_spResult>("pd_RoleGetByCaseIDClientCriteria_sp", pd_RoleGetByCaseIDClientCriteria_spParams).ToList();

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeview)]
        [HttpPost]
        public virtual PartialViewResult QHERecordTimeList(RecordTimeViewModel viewModel)
        {
            var pd_WorkGetByCaseID_spParams = new pd_WorkGetByCaseID_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                WorkerPersonID = viewModel.WorkerPersonID,
                ClientPersonID = viewModel.ClientPersonID,
                BatchLogJobID = Guid.NewGuid(),
            };
            ViewBag.HearingID = viewModel.HearingID.ToEncrypt();
            var data = UtilityService.ExecStoredProcedureWithResults<pd_WorkGetByCaseID_spResult>("pd_WorkGetByCaseID_sp", pd_WorkGetByCaseID_spParams).ToList();
            return PartialView(MVC.QHE.Views._partialRecordTimeList, data);
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeDelete)]
        public virtual JsonResult RecordTimeDelete(string id)
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
        #endregion

        #region RecordTime Add

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeAdd)]
        public virtual ActionResult QHERecordTimeAdd(string id)
        {
            var viewModel = new RecordTimeAddViewModel();
            viewModel.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            viewModel.ControlType = UtilityFunctions.GetNoteControlType("QHE/QHERecordTimeAdd");
            viewModel.HearingID = id.ToDecrypt().ToInt();
            viewModel.QHEHearingID = id;

            viewModel.WorkStartTime = viewModel.WorkEndTime = "08:30 AM";
            var wtRecordTimeGetCases_spParams = new LALoDep.Domain.pd_wt.wtRecordTimeGetCases_spParams()
            {
                ReloadFlag = 1,
                CaseID = UserManager.UserExtended.CaseID,
                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.CaseList = UtilityService.ExecStoredProcedureWithResults<wtRecordTimeGetCases_spResult>("wtRecordTimeGetCases_sp", wtRecordTimeGetCases_spParams).ToList();

            viewModel.Descriptions = UtilityFunctions.CodeGetWorkDescription(agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            viewModel.Phases = UtilityFunctions.CodeGetByTypeIdAndUserId(600, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
 
            var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
            { 
                CaseID = UserManager.UserExtended.CaseID, 
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()

            }).FirstOrDefault();
            viewModel.HoursLabel = "Hours (use tenths for partial hrs)";
            if (defaultPhase != null && defaultPhase.WorkPhaseCodeID.HasValue)
            {
                viewModel.StartDate = defaultPhase.WorkStartDate;
                viewModel.EndDate = defaultPhase.WorkEndDate;
                viewModel.WorkPhaseCodeID = defaultPhase.WorkPhaseCodeID.Value;
                viewModel.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.HasValue ? defaultPhase.WorkHoursRequiredFlag.Value : 0;
                viewModel.WorkPhaseRequiredFlag = defaultPhase.WorkPhaseRequiredFlag.HasValue ? defaultPhase.WorkPhaseRequiredFlag.Value : 0;
                viewModel.RecordTimeNoteSubjectFlag = defaultPhase.RecordTimeNoteSubjectFlag.HasValue ? defaultPhase.RecordTimeNoteSubjectFlag.Value : 0;
                viewModel.HoursLabel = string.IsNullOrEmpty(defaultPhase.HoursLabel) ? "Hours (use tenths for partial hrs)" : defaultPhase.HoursLabel;
                viewModel.UseWorkHoursForActivityLog = defaultPhase.UseWorkHoursForActivityLog.Value;
            }
            var pd_RoleGetByCaseIDClient_spParams = new pd_RoleGetByCaseIDClient_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                ReferralID = Request.QueryString["ReferralID"].ToDecrypt().ToInt()
            };
            viewModel.WorkForList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDClient_spResult>("pd_RoleGetByCaseIDClient_sp", pd_RoleGetByCaseIDClient_spParams).ToList();

            var pd_RoleGetByCaseIDBillingWorker_spParams = new pd_RoleGetByCaseIDBillingWorker_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

            };
            viewModel.StaffOnCaseList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDBillingWorker_spResult>("pd_RoleGetByCaseIDBillingWorker_sp", pd_RoleGetByCaseIDBillingWorker_spParams).ToList();
            viewModel.StaffNotOnCaseList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDBillingWorker_spResult>("pd_RoleGetByCaseIDNewWorkers_sp", pd_RoleGetByCaseIDBillingWorker_spParams).ToList();

            if (viewModel.StaffOnCaseList.Any(x => x.PersonID == UserManager.UserExtended.PersonID))
            {
                viewModel.StaffOnPersonID = UserManager.UserExtended.PersonID;
            }
            else if (viewModel.StaffOnCaseList.Any(x => x.IsCurrentUserFlag == 1))
            {
                viewModel.StaffNotOnPersonID = viewModel.StaffOnCaseList.FirstOrDefault(x => x.IsCurrentUserFlag == 1).PersonID;
            }
            if (viewModel.RecordTimeNoteSubjectFlag == 0)
            {
                viewModel.NoteSubject = "Record Time Note";
            }
            if (Request.QueryString["startDate"] != null)
                viewModel.StartDate = Request.QueryString["startDate"];

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeAdd)]
        public virtual JsonResult QHERecordTimeAddSave(RecordTimeAddViewModel viewModel)
        {
            



            #region

            var personId = 0;
            if (viewModel.StaffOnPersonID.HasValue)
            {
                personId = viewModel.StaffOnPersonID.Value;
            }
            if (viewModel.StaffNotOnPersonID.HasValue)
            {
                personId = viewModel.StaffNotOnPersonID.Value;
            }

            if (!viewModel.WorkStartTime.IsNullOrEmpty() || !viewModel.WorkEndTime.IsNullOrEmpty())
            {


                var oValidation = UtilityService.ExecStoredProcedureWithResults<WorkTimeValidate_spResult>("WorkTimeValidate_sp", new WorkTimeValidate_spParams()
                {

                    PersonID = personId,
                    StartDateTime = viewModel.WorkStartTime.ToDateTime(),
                    EndDateTime = viewModel.WorkEndTime.ToDateTime(),

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                }).FirstOrDefault();
                if (oValidation != null)
                {
                    if (!oValidation.ValidationMessage.IsNullOrEmpty())
                    {
                        return Json(new { isSuccess = false, Message = oValidation.ValidationMessage });
                    }
                }
            }
            if (viewModel.StaffNotOnPersonID.HasValue)
            {
                var pd_RoleInsert_spParams = new pd_RoleInsert_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    PersonID = (viewModel.StaffOnPersonID.HasValue) ? viewModel.StaffOnPersonID.Value : viewModel.StaffNotOnPersonID.Value,
                    RoleTypeCodeID = viewModel.RoleTypeCodeID,
                    RoleClient = 0,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };
                if (!string.IsNullOrEmpty(viewModel.StartDate))
                    pd_RoleInsert_spParams.RoleStartDate = viewModel.StartDate.ToDateTime();
                else
                    pd_RoleInsert_spParams.RoleStartDate = DateTime.Now;
                if (!string.IsNullOrEmpty(viewModel.EndDate))
                    pd_RoleInsert_spParams.RoleEndDate = viewModel.EndDate.ToDateTime();
                else
                    pd_RoleInsert_spParams.RoleEndDate = DateTime.Now;
                var roleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", pd_RoleInsert_spParams).FirstOrDefault();
            }



            var pd_WorkInsert1_spParams = new pd_WorkInsert1_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                PersonID = personId,
                WorkHours = viewModel.WorkHours,
                WorkMileage = viewModel.WorkMileage,
                WorkStartDate = viewModel.StartDate.ToDateTime(),
                WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID,
                WorkPhaseCodeID = viewModel.WorkPhaseCodeID,
                RecordStateID = 1,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                WorkIVeEligibleCodeID = viewModel.WorkIVeEligibleCodeID,
                
            };
            if (!string.IsNullOrEmpty(viewModel.StartDate))
                pd_WorkInsert1_spParams.WorkStartDate = viewModel.StartDate.ToDateTime();
            else
                pd_WorkInsert1_spParams.WorkStartDate = DateTime.Now;
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                pd_WorkInsert1_spParams.WorkEndDate = viewModel.EndDate.ToDateTime();
            else
                pd_WorkInsert1_spParams.WorkEndDate = pd_WorkInsert1_spParams.WorkStartDate;

            var workID = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_WorkInsert1_sp", pd_WorkInsert1_spParams).FirstOrDefault();
            if (!string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var noteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams
                {
                    NoteEntitySystemValueTypeID = 152,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = (int)workID,
                    NoteTypeCodeID = 16,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = UserManager.UserExtended.CaseID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();
            }
            #endregion
            foreach (var item in viewModel.WorkForList)
            {

                var workRoleID = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                {
                    WorkID = (int)workID,
                    RoleID = item.RoleID.Value,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();



            }
            if (viewModel.WorkTimeVisibleFlag == 1 && !viewModel.WorkStartTime.IsNullOrEmpty() && !viewModel.WorkEndTime.IsNullOrEmpty())
            {


                UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                {

                    IUD = "INSERT",
                    WorkTimeStart = viewModel.WorkStartTime.ToDateTime(),
                    WorkTimeEnd = viewModel.WorkEndTime.ToDateTime(),
                    WorkID = (int)workID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                });
            }
            if (!viewModel.FromZipCode.IsNullOrEmpty() && !viewModel.ToZipCode.IsNullOrEmpty())
            {


                UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                {

                    IUD = "INSERT",
                    WorkZipCodeFrom = viewModel.FromZipCode,
                    WorkZipCodeTo = viewModel.ToZipCode,
                    WorkID = (int)workID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                });
            }
           



            

            return Json(new { isSuccess = true });
        }

        #endregion RecordTime Add

        #region RecordTime Edit

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeEdit)]
        public virtual ActionResult QHERecordTimeEdit(string id, string wId)
        {

            if (Request.QueryString["CaseID"] != null)
            {
                Custom.UserEnvironment.UserManager.UpdateCaseStatusBar(Request.QueryString["CaseID"].ToDecrypt().ToInt());
            }
            int workId = wId.ToDecrypt().ToInt();
            if (UserManager.UserExtended.CaseID > 0 && workId > 0)
            {


                var viewModel = new RecordTimeEditViewModel();
                var pd_WorkGet_spParams = new pd_WorkGet_spParams
                {
                    WorkID = workId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                viewModel.QHEHearingID = id;
                var work = UtilityService.ExecStoredProcedureWithResults<pd_WorkGet_spResult>("pd_WorkGet_sp", pd_WorkGet_spParams).FirstOrDefault();
                if (work != null)
                {
                    viewModel.InjectFrom(work);
                    viewModel.WorkStartDate = work.WorkStartDate.HasValue ? work.WorkStartDate.ToShortDateString() : string.Empty;
                    viewModel.WorkEndDate = work.WorkEndDate.HasValue ? work.WorkEndDate.ToShortDateString() : string.Empty;
                    viewModel.WorkStartTime = work.WorkTimeStart.HasValue ? work.WorkTimeStart.ToString() : "08:30 AM";
                    viewModel.WorkEndTime = work.WorkTimeEnd.HasValue ? work.WorkTimeEnd.ToString() : "08:30 AM";
                    viewModel.WorkTimeID = work.WorkTimeID.ToInt();
                    viewModel.Phases = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: viewModel.WorkPhaseCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                    viewModel.Descriptions = UtilityFunctions.CodeGetWorkDescription(workDescriptionCodeId: viewModel.WorkDescriptionCodeID, workId: work.WorkID, agencyId: work.AgencyID ?? UserManager.UserExtended.CaseNumberAgencyID);
                    viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(workIVeEligibleCodeId: viewModel.WorkIVeEligibleCodeID, workId: work.WorkID, agencyId: work.AgencyID ?? UserManager.UserExtended.CaseNumberAgencyID);
                    viewModel.WorkZipCodeID = work.WorkZipCodeID;
                    viewModel.FromZipCode = work.WorkZipCodeFrom;
                    viewModel.ToZipCode = work.WorkZipCodeTo;
                    // Worker list
                    var pd_RoleGetByCaseIDBillingWorker_spParams = new pd_RoleGetByCaseIDBillingWorker_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    viewModel.WorkerList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDBillingWorker_spResult>("pd_RoleGetByCaseIDBillingWorker_sp", pd_RoleGetByCaseIDBillingWorker_spParams).ToList();

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
                    viewModel.ControlType = UtilityFunctions.GetNoteControlType("Case/RecordTimeEdit", noteId: viewModel.NoteID);

                    // Work for
                    viewModel.WorkForList = UtilityService.ExecStoredProcedureWithResults<pd_WorkRoleGetByWorkID_spResult>("pd_WorkRoleGetByWorkID_sp", new pd_WorkRoleGetByWorkID_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        WorkID = workId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).ToList();
                    var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()

                    }).FirstOrDefault();
                    if (defaultPhase != null && defaultPhase.WorkPhaseCodeID.HasValue)
                    {
                        viewModel.RecordTimeNoteSubjectFlag = defaultPhase.RecordTimeNoteSubjectFlag.HasValue ? defaultPhase.RecordTimeNoteSubjectFlag.Value : 0;
                        viewModel.UseWorkHoursForActivityLog = defaultPhase.UseWorkHoursForActivityLog.Value;
                        viewModel.WorkPhaseRequiredFlag = defaultPhase.WorkPhaseRequiredFlag.HasValue ? defaultPhase.WorkPhaseRequiredFlag.Value : 0;
                        viewModel.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.HasValue ? defaultPhase.WorkHoursRequiredFlag.Value : 0;
                    }
                    if (viewModel.RecordTimeNoteSubjectFlag == 0)
                    {
                        if (viewModel.NoteSubject.IsNullOrEmpty())
                            viewModel.NoteSubject = "Record Time Note";
                    }
                    return View(viewModel);
                }
            }


            return RedirectToAction("AccessDenied", "Home");

        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeEdit)]
        [HttpPost]
        public virtual JsonResult QHERecordTimeEditSave(RecordTimeEditViewModel viewModel, RecordTimeEditViewModel oldViewModel)
        {
            if (!viewModel.WorkStartTime.IsNullOrEmpty() || !viewModel.WorkEndTime.IsNullOrEmpty())
            {


                var oValidation = UtilityService.ExecStoredProcedureWithResults<WorkTimeValidate_spResult>("WorkTimeValidate_sp", new WorkTimeValidate_spParams()
                {

                    PersonID = viewModel.PersonID,
                    StartDateTime = viewModel.WorkStartTime.ToDateTime(),
                    EndDateTime = viewModel.WorkEndTime.ToDateTime(),
                    WorkID = viewModel.WorkID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                }).FirstOrDefault();
                if (oValidation != null)
                {
                    if (!oValidation.ValidationMessage.IsNullOrEmpty())
                    {
                        return Json(new { isSuccess = false, Message = oValidation.ValidationMessage });
                    }
                }
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
                    CaseID = UserManager.UserExtended.CaseID,
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
                    CaseID = UserManager.UserExtended.CaseID,
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
            //if any data changed then update.
            if (viewModel.PersonID != oldViewModel.PersonID || viewModel.WorkHours != oldViewModel.WorkHours
                || viewModel.WorkHoursOverTime != oldViewModel.WorkHoursOverTime || viewModel.WorkMileage != oldViewModel.WorkMileage
                || viewModel.WorkDescriptionCodeID != oldViewModel.WorkDescriptionCodeID || viewModel.WorkPhaseCodeID != oldViewModel.WorkPhaseCodeID
                || viewModel.WorkStartDate != oldViewModel.WorkStartDate || viewModel.WorkEndDate != oldViewModel.WorkEndDate || viewModel.WorkIVeEligibleCodeID != oldViewModel.WorkIVeEligibleCodeID)
            {
                // Update Work time
                var pd_WorkUpdate1_spParams = new pd_WorkUpdate1_spParams
                {
                    WorkID = viewModel.WorkID,
                    AgencyID = viewModel.AgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
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
                    WorkIVeEligibleCodeID = viewModel.WorkIVeEligibleCodeID
                };

                if (!string.IsNullOrEmpty(viewModel.WorkStartDate))
                    pd_WorkUpdate1_spParams.WorkStartDate = Convert.ToDateTime(viewModel.WorkStartDate);

                if (!string.IsNullOrEmpty(viewModel.WorkEndDate))
                    pd_WorkUpdate1_spParams.WorkEndDate = Convert.ToDateTime(viewModel.WorkEndDate);

                UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkUpdate1_sp", pd_WorkUpdate1_spParams).FirstOrDefault();
            }
            foreach (var item in viewModel.WorkForList)
            {
                if (item.WorkRoleID.ToInt() <= 0)
                {
                    var workRoleID = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                    {
                        WorkID = viewModel.WorkID,
                        RoleID = item.RoleID.Value,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).FirstOrDefault();
                }




            }
            foreach (var item in viewModel.DeleteWorkForList)
            {
                if (item.WorkRoleID.ToInt() > 0)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_WorkRoleDelete_sp", new pd_Delete_spParams
                    {
                        ID = item.WorkRoleID.Value,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    });
                }

            }
            if (!viewModel.WorkStartTime.IsNullOrEmpty() && !viewModel.WorkEndTime.IsNullOrEmpty())
            {
                if (viewModel.WorkTimeID > 0)
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                    {

                        IUD = "UPDATE",
                        WorkTimeStart = viewModel.WorkStartTime.ToDateTime(),
                        WorkTimeEnd = viewModel.WorkEndTime.ToDateTime(),
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        WorkTimeID = viewModel.WorkTimeID
                    });
                }
                else
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                    {

                        IUD = "INSERT",
                        WorkTimeStart = viewModel.WorkStartTime.ToDateTime(),
                        WorkTimeEnd = viewModel.WorkEndTime.ToDateTime(),
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        WorkTimeID = viewModel.WorkTimeID
                    });
                }

            }
            else if (viewModel.WorkTimeID > 0)
            {
                UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                {

                    IUD = "DELETE",

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    WorkTimeID = viewModel.WorkTimeID
                });
            }


            if (!viewModel.FromZipCode.IsNullOrEmpty() && !viewModel.ToZipCode.IsNullOrEmpty())
            {
                if (viewModel.WorkZipCodeID > 0)
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                    {
                        WorkZipCodeID = viewModel.WorkZipCodeID,
                        IUD = "UPDATE",
                        WorkZipCodeFrom = viewModel.FromZipCode,
                        WorkZipCodeTo = viewModel.ToZipCode,
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    });
                }
                else
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                    {

                        IUD = "INSERT",
                        WorkZipCodeFrom = viewModel.FromZipCode,
                        WorkZipCodeTo = viewModel.ToZipCode,
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    });
                }


            }
            else if (viewModel.WorkZipCodeID > 0)
            {
                UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                {
                    WorkZipCodeID = viewModel.WorkZipCodeID,
                    IUD = "DELETE",
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                });
            }


            string URL = Url.Action(MVC.QHE.QHERecordTime(viewModel.QHEHearingID));
            return Json(new { isSuccess = true, URL = URL });
        }

        #endregion RecordTime Edit
        #endregion

        #region Helper Methods
        [HttpPost]
        public virtual JsonResult NextQHECase(int id)
        {
            var nextHearing = GetNextQHECase(id);
            if (nextHearing != null)
            {
                if (nextHearing.CaseID != UserManager.UserExtended.CaseID)
                    UserManager.UpdateCaseStatusBar(nextHearing.CaseID);

                return Json(new { isSuccess = true, nextHearingId = nextHearing.NextHearingID.ToEncrypt() });
            }
            return Json(new { isSuccess = true, nextHearingId = "" });
        }
        public pd_HearingGetByQheNextHearingID_spResult GetNextQHECase(int currentHearingID)
        {
            return UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByQheNextHearingID_spResult>("pd_HearingGetByQheNextHearingID_sp",
                new pd_HearingGetByQheNextHearingID_spParams
                {
                    HearingID = currentHearingID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();
        }
        #endregion
    }
}