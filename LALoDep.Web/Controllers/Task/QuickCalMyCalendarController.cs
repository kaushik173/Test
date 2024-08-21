using System;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Task;
using LALoDep.Core.Custom.Extensions;
using System.Collections.Generic;
using LALoDep.Custom;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Models.CaseOpening;
using LALoDep.Domain.PD_PDAction;
using LALoDep.Domain.pd_Profile;
using LALoDep.Models.Case;
using LALoDep.Domain.qcal;
using LALoDep.Domain.pd_Petition;
using Omu.ValueInjecter;
using CrystalDecisions.CrystalReports.Engine;
using LALoDep.Domain.com_Report;
using System.Data;
using LALoDep.Domain;
using LALoDep.Domain.qa;
using LALoDep.Domain.pd_Calendar;
using Jcats.GeorgiaPD.SQL.SPS.SPS;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.HearingPrepNote;
using LALoDep.Domain.Advisement;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class TaskController
    {

        //  [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddApptCasePage, PageSecurityItemID = SecurityToken.ViewCase)]
        public virtual ActionResult QuickCalMyCalendar(string HearingDate, int PendingHearingsOnly = 1, int AttorneyPersonID = 0)
        {
            if (Request.QueryString["load"] != null)
            {
                Session["QuickCalHearingDate"] = Session["QuickCalAttorneyPersonID"] = null;
                return RedirectToAction("QuickCalMyCalendar");
            }
            var viewModel = new QuickCalMyCalendarViewModel();
            viewModel.PendingHearingsOnly = PendingHearingsOnly;
            if (AttorneyPersonID > 0 && Request.QueryString["_uniquerequest"] != null)
            {
                Session["QuickCalAttorneyPersonID"] = AttorneyPersonID;
            }
            else if (Session["QuickCalAttorneyPersonID"] != null)
            {
                AttorneyPersonID = Session["QuickCalAttorneyPersonID"].ToInt();

            }
            else
            {
                AttorneyPersonID = UserManager.UserExtended.PersonID;
            }

            if (!HearingDate.IsNullOrEmpty() && Request.QueryString["_uniquerequest"] != null)
            {
                Session["QuickCalPendingHearingsOnly"] = viewModel.PendingHearingsOnly;
                Session["QuickCalHearingDate"] = HearingDate;
            }
            else if (Session["QuickCalHearingDate"] != null)
            {
                viewModel.PendingHearingsOnly = Session["QuickCalPendingHearingsOnly"].ToInt();
                HearingDate = Session["QuickCalHearingDate"].ToString();

            }
            else
            {
                HearingDate = DateTime.Now.ToString("d");
            }
            viewModel.HearingDate = HearingDate.IsNullOrEmpty() ? DateTime.Now.ToString("d") : HearingDate;

            var data = UtilityService.ExecStoredProcedureWithResults<qcal_MyCalendar_spResult>("qcal_MyCalendar_sp",
                     new qcal_MyCalendar_spParams()
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                         PersonID = AttorneyPersonID,
                         //PersonID = 60389311,//60749611
                         StartDate = viewModel.HearingDate.ToDateTime(),
                         PendingHearingOnlyFlag = viewModel.PendingHearingsOnly

                     }).ToList();

            viewModel.HearingList = data;
            viewModel.AdvisementList = UtilityService.ExecStoredProcedureWithResults<Advisement_GetForAttorney_spResult>("Advisement_GetForAttorney_sp",
                   new Advisement_GetForAttorney_spParams()
                   {
                       BatchLogJobID = Guid.NewGuid(),
                       UserID = UserManager.UserExtended.UserID,
                       AttorneyPersonID = AttorneyPersonID

                   }).ToList();
            var attorneyList = UtilityService.ExecStoredProcedureWithResults<qcal_MyCalendarAttorneyList_spResult>("qcal_MyCalendarAttorneyList_sp",
                    new qcal_MyCalendarAttorneyList_spParams()
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,


                    }).Select(o => new SelectListItem() { Text = (o.ShowClientPresentReadyFlag.HasValue ? o.ShowClientPresentReadyFlag.Value.ToString() : "0") + "|" + o.NameDisplay, Value = o.PersonID.ToString() }).ToList();

            viewModel.AttorneyList = attorneyList;
            viewModel.AttorneyPersonID = AttorneyPersonID;

            if (attorneyList.Any(o => o.Value == viewModel.AttorneyPersonID.ToString() && o.Text.StartsWith("1|")))
            {
                ViewBag.ShowClientPresentReadyFlag = "1";
            }

            return View(viewModel);
        }
        [HttpPost]
        public virtual JsonResult QuickCalMyCalendar(string ids, string cpIds, string readyIds)
        {
            if (!ids.IsNullOrEmpty())
            {


                var arrIds = ids.Split(',');

                foreach (var id in arrIds)
                {
                    var hearingId = id.Split('|')[0].ToInt();
                    var calNumber = id.Split('|')[1];

                    UtilityService.ExecStoredProcedureWithoutResultADO("qcal_HearingUpdateCalendarNumber_sp",
                       new qcal_HearingUpdateCalendarNumber_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid(),
                           HearingCourtCalendarNumber = calNumber
                       });
                }
            }
            if (!cpIds.IsNullOrEmpty())
            {


                var arrIds = cpIds.Split(',');

                foreach (var id in arrIds)
                {
                    var hearingId = id.Split('|')[0].ToInt();
                    var clientPresentFlag = Convert.ToByte(id.Split('|')[1]);

                    UtilityService.ExecStoredProcedureWithoutResultADO("qcal_HearingUpdateClientPresentFlag_sp",
                       new qcal_HearingUpdateClientPresentFlag_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid(),
                           ClientPresentFlag = clientPresentFlag

                       });
                }
            }
            if (!readyIds.IsNullOrEmpty())
            {


                var arrIds = readyIds.Split(',');

                foreach (var id in arrIds)
                {
                    var hearingId = id.Split('|')[0].ToInt();
                    var readyFlag = Convert.ToByte(id.Split('|')[1]);

                    UtilityService.ExecStoredProcedureWithoutResultADO("qcal_HearingUpdateReadyFlag_sp",
                       new qcal_HearingUpdateReadyFlag_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid(),
                           ReadyFlag = readyFlag

                       });
                }
            }
            return Json(new { Status = "Done" });
        }
        [ChildActionOnly]
        public virtual PartialViewResult CalendarSummaryBar(string id)
        {

            var data = UtilityService.ExecStoredProcedureWithResults<qcal_StatusBarHeader_spResult>("qcal_StatusBarHeader_sp", new qcal_StatusBarHeader_spParams
            {
                HearingID = id.ToInt(),

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).FirstOrDefault();



            return PartialView("_partialCalendarSummaryBar", data);
        }

        [HttpPost]
        public virtual ActionResult GetAttachedFiles(string id)
        {
            var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];


            int hearingId = id.ToDecrypt().ToInt();
            var files = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingCaseFileGetList_spResult>("qcal_AS_HearingCaseFileGetList_sp",
                    new qcal_AS_HearingCaseFileGetList_spParams()
                    {
                        HearingID = hearingId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).ToList();

            if (files.Any())
            {
                var caseId = files[0].CaseID.ToInt();

                var oCaseFileGetPathByCase = UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                      "CaseFileGetPathByCaseID_sp",
                      new CaseFileGetPathByCaseID_spParams
                      {
                          BatchLogJobID = Guid.NewGuid(),
                          CaseID = caseId,
                          UserID = UserManager.UserExtended.UserID,


                      }).FirstOrDefault();
                var sharePoint_UseFlag = 0;
                var useGoogleDriveUpload = 0;
                if (oCaseFileGetPathByCase != null)
                    sharePoint_UseFlag = oCaseFileGetPathByCase.SharePoint_UseFlag.ToInt();
                if (oCaseFileGetPathByCase.UseGoogleDocsFlag.HasValue && oCaseFileGetPathByCase.UseGoogleDocsFlag.Value > 0)
                {
                    var genvironment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
                    var parentFolderId = "";
                    if (genvironment == "Test")
                    {
                        parentFolderId = oCaseFileGetPathByCase.GoogleFolderID_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        parentFolderId = oCaseFileGetPathByCase.GoogleFolderID_PROD;

                    }
                    if (!parentFolderId.IsNullOrEmpty())
                        useGoogleDriveUpload = oCaseFileGetPathByCase.UseGoogleDocsFlag.Value;

                }
                foreach (var item in files)
                {
                    var sharePoint_FilePath = "";
                    if (!item.SharePointFileID.IsNullOrEmpty())
                    {
                        item.SharePoint_UseFlag = 1;
                        if (environment == "Test" || environment == "Dev")
                        {
                            sharePoint_FilePath = item.SharePointFile_URL_TEST;

                        }
                        else if (environment == "Prod")
                        {
                            sharePoint_FilePath = item.SharePointFile_URL_PROD;

                        }
                        item.SharePoint_FilePath = sharePoint_FilePath;
                        item.SharePoint_UseFlag = sharePoint_UseFlag;
                    }
                    item.DownloadPath = item.GoogleFileID.IsNullOrEmpty() ? "/Case/DownloadCaseFile/" + item.CaseFileID.ToEncrypt() + "?CaseId=" + item.CaseID.Value.ToEncrypt() : "/Case/DownloadDriveFile/" + item.GoogleFileID + "?fileName=" + item.FileDisplay.ToEncrypt() + "&CaseId=" + item.CaseID.Value.ToEncrypt();
                    item.KamiUrl = "https://web.kamihq.com/web/viewer.html?state=%7B\"ids\":%5B\"" + item.GoogleFileID + "\"%5D,\"action\":\"open\"%7D";
                    item.UseGoogleDocsFlag = useGoogleDriveUpload;
                }
            }
            return PartialView("_partialAttachedFiles", files);
        }

        [HttpPost]
        public virtual ActionResult SaveAttachedFiles(List<HearingCaseFile> viewModels)
        {
            if (viewModels != null && viewModels.Any())
            {
                foreach (var file in viewModels)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("HearingCaseFileIUD_sp", new HearingCaseFileIUD_spParams
                    {
                        IUD = file.Selected ? "INSERT" : "DELETE",
                        CaseFileID = file.CaseFileID,
                        HearingCaseFileID = file.HearingCaseFileID,
                        HearingID = file.HearingID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();
                }
            }
            return Json(new { IsSuccess = true });
        }



        public virtual ActionResult CalendarAppearanceSheet(string id, string caseId)
        {
            ViewBag.HearingID = id.ToDecrypt();
            int hearingId = id.ToDecrypt().ToInt();
            if (caseId.ToDecrypt().ToInt() > 0)
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());

            var hearingGet = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", new pd_HearingGet_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                HearingID = id.ToDecrypt().ToInt()
            }).FirstOrDefault();


            if (hearingGet != null)
            {
                if (caseId.ToDecrypt().ToInt() != hearingGet.CaseID.Value)
                    UserManager.UpdateCaseStatusBar(hearingGet.CaseID.Value);
            }
            var appearingAttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForHearingAttendingAttorney_spResult>(
                   "pd_RoleGetForHearingAttendingAttorney_sp",
                   new pd_RoleGetForHearingAttendingAttorney_spParams()
                   {
                       CaseID = UserManager.UserExtended.CaseID,
                       HearingID = hearingId,
                       UserID = UserManager.UserExtended.UserID,
                       BatchLogJobID = Guid.NewGuid()
                   }).ToList();



            var model = new AppearanceSheetViewModel
            {
                HearingID = hearingId,
                HearingAttendance =
                   UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingAttendanceGet_spResult>(
                       "qcal_AS_HearingAttendanceGet_sp",
                       new qcal_AS_HearingAttendanceGet_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).Select(c => (HearingAttendance)(new HearingAttendance()).InjectFrom(c)).ToList(),
                CaseStatusList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetClientStatus_spResult>(
                    "pd_CodeGetClientStatus_sp",
                    new pd_CodeGetClientStatus_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        HearingID = hearingId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList()
,
                AppearingAttorneyList = appearingAttorneyList.Select(o => new SelectListItem() { Value = o.PersonID.ToString() + "|" + o.RoleID.ToString() + "|" + o.HearingAttendanceID.ToString(), Text = o.PersonNameDisplay, Selected = o.HearingAttendanceID > 0 }).ToList(),

                CurrentDCCList =
                   UtilityService.ExecStoredProcedureWithResults<qcal_AS_DCCGet_spResult>(
                       "qcal_AS_DCCGet_sp",
                       new qcal_AS_DCCGet_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).ToList(),
                NoteList =
                   UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingNoteGetList_spResult>(
                       "qcal_AS_HearingNoteGetList_sp",
                       new qcal_AS_HearingNoteGetList_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).ToList()
                       ,
                ContinuanceRequestedByList = UtilityFunctions.CodeGetByTypeIdAndUserId(28),



            };
            model.IsCaseStatusExists = model.CaseStatusList.Any();
            model.ControlType = UtilityFunctions.GetNoteControlType("Task/CalendarAppearanceSheet");
            model.HourTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(222, includeCodeId: model.HourTypeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: model.PhaseID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            var hearingData = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingGet_spResult>(
                       "qcal_AS_HearingGet_sp",
                       new qcal_AS_HearingGet_spParams()
                       {
                           HearingID = hearingId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).FirstOrDefault();
            if (hearingData != null)
            {

                if (caseId.ToDecrypt().ToInt() > 0)
                    UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());
                var attonory = appearingAttorneyList.Where(o => o.HearingAttendanceID > 0).FirstOrDefault();
                if (attonory != null)
                {
                    model.AppearingAttorneyID = attonory.PersonID + "|" + attonory.RoleID.ToString() + "|" + attonory.HearingAttendanceID.ToString();
                    model.HearingAttendanceID = attonory.HearingAttendanceID;
                }

                model.GeneralHearingNote = hearingData.GeneralHearingNote;
                model.CourtReporter = hearingData.CourtReporter;
                model.CourtOfficer = hearingData.CourtOfficer;
                model.CSW = hearingData.CSW;
                model.CSWPresentFlag = hearingData.CSWPresentFlag;
                model.MediaPresentFlag = hearingData.MediaPresentFlag.HasValue && hearingData.MediaPresentFlag.Value == 1;
                model.NoticeProperFlag = hearingData.NoticeProperFlag.HasValue && hearingData.NoticeProperFlag.Value == 1;
                model.ReasonableEffortFlag = hearingData.ReasonableEffortFlag.HasValue && hearingData.ReasonableEffortFlag.Value == 1;
                model.OfficerPersonID = hearingData.OfficerPersonID.HasValue ? hearingData.OfficerPersonID.Value : 0;

                model.CanAddAPOFFE = hearingData.CanAddAPOFFE;
                model.IsForParent = hearingData.ClientType == "PARENT";

                model.HearingOfficerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>(
                       "pd_HearingOfficerGet_sp",
                       new pd_HearingOfficerGet_spParams()
                       {
                           CaseID = UserManager.UserExtended.CaseID,
                           AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).Select(o => new SelectListItem() { Text = o.PersonNameLast + ", " + o.PersonNameFirst, Value = o.PersonID.ToString() }).ToList();
                model.CounselList = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingCounselGetList_spResult>(
                               "qcal_AS_HearingCounselGetList_sp",
                               new qcal_AS_HearingCounselGetList_spParams()
                               {
                                   HearingID = hearingId,
                                   UserID = UserManager.UserExtended.UserID,
                                   BatchLogJobID = Guid.NewGuid()
                               }).Select(o => new SelectListItem() { Text = o.FullName, Value = o.PersonID.ToString() }).ToList();
                model.CounselListDCC = UtilityService.ExecStoredProcedureWithResults<qcal_AS_DCCGetList_spResult>(
                             "qcal_AS_DCCGetList_sp",
                             new qcal_AS_DCCGetList_spParams()
                             {
                                 HearingID = hearingId,
                                 UserID = UserManager.UserExtended.UserID,
                                 BatchLogJobID = Guid.NewGuid()
                             }).Select(o => new SelectListItem() { Text = o.FullName, Value = o.PersonID.ToString() }).ToList();
                model.NoteTypeList = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingNoteTypeGetNew_spResult>(
                           "qcal_AS_HearingNoteTypeGetNew_sp",
                           new qcal_AS_HearingNoteTypeGetNew_spParams()
                           {
                               HearingID = hearingId,
                               UserID = UserManager.UserExtended.UserID,
                               BatchLogJobID = Guid.NewGuid()
                           }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

                model.PetitionByHearingList =
                UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetAllByHearingID_spResult>(
                    "pd_PetitionGetAllByHearingID_sp", new pd_PetitionGetAllByHearingID_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,

                        BatchLogJobID = Guid.NewGuid(),
                        HearingID = hearingId
                    }).ToList();

                var hearingResults = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults_ForHearingResult>(
                "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                {
                    CodeTypeID = 11,
                    SortOption = "IncludeInActive",
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserEnvironment.UserManager.UserExtended.UserID,
                    AgencyID = 0
                }).ToList();

                model.GlobalResultList = hearingResults.Where(x => x.ActiveFlag == 1).Select(x => new SelectListItem() { Text = x.CodeDisplay, Value = x.CodeID.ToString() });
                model.PetitionType602List = UtilityFunctions.CodeGetBySystemValueTypeId(179);
                model.HearingType602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(239);

                model.HearingResult602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(240);

                model.CourtDepartment602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(241);
                model.HearingResultContinuanceList = UtilityFunctions.CodeGetBySystemValueTypeId(47);
                model.HearingTypeNonHearingEventList = UtilityFunctions.CodeGetBySystemValueTypeId(214);
                model.HearingResultFutureHearingsList = UtilityFunctions.CodeGetBySystemValueTypeId(207);
                model.HearingResultListForHoursNotRequiredValidation = UtilityFunctions.CodeGetBySystemValueTypeId(262, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                model.HearingTypeHoursNotRequired = UtilityFunctions.CodeGetBySystemValueTypeId(265, agencyId: UserManager.UserExtended.CaseNumberAgencyID);



                if (hearingGet != null)
                {


                    if (hearingGet.HearingCourtDepartmentCodeID != null)
                        model.DepartmentID = hearingGet.HearingCourtDepartmentCodeID.Value;
                    if (hearingGet.HearingDateTime != null)
                    {
                        model.HearingDate = hearingGet.HearingDateTime.Value.ToString("d");

                    }
                    if (hearingGet.HearingTypeCodeID != null) model.HearingTypeID = hearingGet.HearingTypeCodeID.Value;
                    if (hearingGet.HearingRequestedByCodeID.HasValue)
                        model.ContinuanceRequestedByID = hearingGet.HearingRequestedByCodeID.Value;

                    if (hearingGet.WorkDescriptionCodeID != null && hearingGet.WorkDescriptionCodeID.Value != 24901)
                        model.HourTypeID = hearingGet.WorkDescriptionCodeID.Value;
                    if (hearingGet.WorkPhaseCodeID != null && hearingGet.WorkDescriptionCodeID != null && hearingGet.WorkDescriptionCodeID.Value != 24901)
                        model.PhaseID = hearingGet.WorkPhaseCodeID.Value;
                    if (hearingGet.WorkHours != null && hearingGet.WorkDescriptionCodeID != null && hearingGet.WorkDescriptionCodeID.Value != 24901)
                        model.Hours = hearingGet.WorkHours.Value;
                }


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


            }
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
                model.HoursNotRequiredBeforeHearingDate = caseDefault.HoursNotRequiredBeforeHearingDate;
                model.DataValidation_RequireHearingHoursFlag = caseDefault.DataValidation_RequireHearingHoursFlag.HasValue && caseDefault.DataValidation_RequireHearingHoursFlag.Value == 1 ? 1 : 0;
            }
            model.HoursLabel = "Hours (use tenths for partial hrs)";
            var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()

            }).FirstOrDefault();

            if (!string.IsNullOrEmpty(defaultPhase?.HoursLabel))
                model.HoursLabel = defaultPhase.HoursLabel;
            if (defaultPhase.WorkHoursRequiredFlag.HasValue)
                model.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.Value;

            model.HearingNoteGetPreviousNotes = UtilityService.ExecStoredProcedureWithResults<qcal_HearingNoteGetPreviousNotes_spResult>("qcal_HearingNoteGetPreviousNotes_sp",
                                                                         new qcal_HearingNoteGetPreviousNotes_spParams()
                                                                         {
                                                                             CaseID = UserManager.UserExtended.CaseID,
                                                                             HearingID = hearingId,
                                                                             UserID = UserManager.UserExtended.UserID,
                                                                             BatchLogJobID = Guid.NewGuid()
                                                                         }).ToList();

            model.HearingNoteGetMain = UtilityService.ExecStoredProcedureWithResults<qcal_HearingNoteGetMain_spResult>("qcal_HearingNoteGetMain_sp",
                                                                       new qcal_HearingNoteGetMain_spParams()
                                                                       {
                                                                           CaseID = UserManager.UserExtended.CaseID,
                                                                           HearingID = hearingId,
                                                                           UserID = UserManager.UserExtended.UserID,
                                                                           BatchLogJobID = Guid.NewGuid()
                                                                       }).FirstOrDefault();

            var oHearingPrepNoteGet = UtilityService.ExecStoredProcedureWithResults<HearingPrepNoteGet_spResult>(
                      "HearingPrepNoteGet_sp",
                      new HearingPrepNoteGet_spParams()
                      {
                          CaseID = UserManager.UserExtended.CaseID,
                          HearingID = hearingId,
                          UserID = UserManager.UserExtended.UserID,
                          BatchLogJobID = Guid.NewGuid()
                      }).FirstOrDefault();
            if (oHearingPrepNoteGet != null)
            {
                model.HearingPrepNoteEntry = oHearingPrepNoteGet.NoteEntry;
                model.HearingPrepNoteID = oHearingPrepNoteGet.NoteID;
                model.HearingPrepNoteTotalPrepHoursLink = oHearingPrepNoteGet.TotalPrepHoursLink;
            }
            model.HearingContinuanceReasonGetForHearingList = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<HearingContinuanceReasonGetForHearing_spResult>(
           "HearingContinuanceReasonGetForHearing_sp", new HearingContinuanceReasonGetForHearing_spParams
           {
               CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
               CaseID = UserManager.UserExtended.CaseID,
               HearingID = hearingId,
               UserID = UserEnvironment.UserManager.UserExtended.UserID,
           }).ToList();

            model.HearingContinuanceRequestedByGetForHearingList = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<HearingContinuanceRequestedByGetForHearing_spResult>(
            "HearingContinuanceRequestedByGetForHearing_sp", new HearingContinuanceRequestedByGetForHearing_spParams
            {
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = hearingId,
                UserID = UserEnvironment.UserManager.UserExtended.UserID,
            }).ToList();
            if (model.IsForParent)
            {

                model.NewClientCurrentAddress = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ClientAddressGet_spResult>("qcal_AS_ClientAddressGet_sp",
                                                        new qcal_AS_ClientAddressGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

                model.Detention_PlacementList = UtilityFunctions.CodeGetBySystemValueTypeId(248);
                model.AllegationFindingList = UtilityFunctions.CodeGetByTypeIdAndUserId(68);

                model.IncarcerationFacilityList = UtilityService.ExecStoredProcedureWithResults<qcal_AS_IncarcerationFacilityGet_spResult>("qcal_AS_IncarcerationFacilityGet_sp",
                                                                                new qcal_AS_IncarcerationFacilityGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                                                                    .Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });



                var contact = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ClientAddressContactGet_spResult>("qcal_AS_ClientAddressContactGet_sp",
                                                                                new qcal_AS_ClientAddressContactGet_spParams() { HearingID = hearingId, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();
                model.ContactPreferenceList = UtilityFunctions.CodeGetByTypeIdAndUserId(1400);
                model.NewAddressStateCodeID = 841;
                if (contact != null)
                {
                    model.WorkPhone = contact.WorkPhone;
                    model.EmailAddress = contact.EmailAddress;
                    model.MobilePhone = contact.MobilePhone;
                    model.NewAddressPhone = contact.HomePhone;
                    model.NewAddressCity = contact.City;
                    model.NewAddressStateCodeID = contact.StateCodeID ?? 841;
                    model.NewAddressZipCode = contact.ZipCode;
                    model.NewAddressStreet = contact.Street;
                    model.ContactPreferenceID = contact.PreferenceCodeID;
                    model.ContactPreferenceComment = contact.PreferenceComment;
                    model.ClientAddressContactGetModel = contact;
                }

                model.StateList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(5, includeCodeId: model.NewAddressStateCodeID ?? 0);
                return View("CalendarAppearanceSheetForParent", model);

            }
         
            return View(model);

        }
        [HttpPost]
        public virtual JsonResult CalendarAppearanceSheet(AppearanceSheetViewModel model)
        {
            #region QCal
            if (model.UpdateHearing == 1)
            {

                UtilityService.ExecStoredProcedureWithoutResults(
                  "qcal_AS_HearingUpdate_sp",
                  new qcal_AS_HearingUpdate_spParams()
                  {
                      HearingID = model.HearingID,
                      HearingCourtReporter = model.CourtReporter,
                      HearingMediaPresentFlag = model.MediaPresentFlag ? 1 : 0,
                      HearingNoticeProperFlag = model.NoticeProperFlag ? 1 : 0,
                      HearingReasonableEffortFlag = model.ReasonableEffortFlag ? 1 : 0,
                      HearingOfficerPersonID = model.OfficerPersonID,
                      UserID = UserManager.UserExtended.UserID,
                      BatchLogJobID = Guid.NewGuid(),
                      HearingCourtOfficer = model.CourtOfficer,
                      HearingCSW = model.CSW,
                      HearingCSWPresentFlag = (model.CSWPresentFlag.HasValue ? (byte)model.CSWPresentFlag.Value : (byte?)null)
                  });
            }

            if (model.HearingAttendance != null && model.HearingAttendance.Count() > 0)
            {
                foreach (var item in model.HearingAttendance)
                {
                    if (item.UpdateHearingAttendenceRecord)
                    {
                        UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_HearingAttendanceIUD_sp",
                          new qcal_AS_HearingAttendanceIUD_spParams()
                          {
                              HearingID = model.HearingID,
                              AttendedFlag = item.AttendedFlag,
                              CounselPersonID = item.CounselPersonID,
                              FillinCounselPersonID = item.FillinCounselPersonID,
                              HearingAttendanceID = item.HearingAttendanceID,
                              Placement = item.Placement,
                              RoleID = item.RoleID,

                              UserID = UserManager.UserExtended.UserID,
                              BatchLogJobID = Guid.NewGuid(),
                              AppearanceRequiredFlag = item.AppearanceRequiredFlag,

                          });
                    }


                    if (model.IsForParent) // Appareance sheet for parent client
                    {
                        if (item.ClientFlag == 1)
                        {

                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", new pd_HearingPetitionUpdate_spParams
                            {
                                HearingID = model.HearingID,
                                PetitionID = item.PetitionID ?? 0,
                                HearingPetitionResultCodeID = item.HearingPetitionResultCodeID ?? 0,
                                OrderBackFlag = item.OrderBackFlag,
                                ASFAFlag = item.ASFAFlag,
                                AppearanceWaivedFlag = item.AppearanceWaivedFlag,
                                NonOffendingFlag = item.NonOffendingFlag,
                                IncarcerationFacilityCodeID = item.IncarcerationFacilityCodeID,

                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,

                            });
                        }
                        else
                        {
                            if (item.UpdateHearingAttendenceRecord && item.ChildFlag == 1)
                            {
                                UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_PersonClassificationAddRemoveICWA_sp",
                                                              new qcal_AS_PersonClassificationAddRemoveICWA_spParams()
                                                              {
                                                                  PersonID = item.PersonID,
                                                                  ICWAFlag = item.ICWAFlag,
                                                                  UserID = UserManager.UserExtended.UserID,
                                                                  BatchLogJobID = Guid.NewGuid()
                                                              });

                                if (item.UpdatePlacement)
                                {
                                    UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_PersonClassificationAddRemoveDetPlacement_sp",
                                                                 new qcal_AS_PersonClassificationAddRemoveDetPlacement_spParams()
                                                                 {
                                                                     DetPlacementPersonClassificationID = item.DetPlacementPersonClassificationID,
                                                                     PersonID = item.PersonID,
                                                                     DetPlacementCodeID = item.DetPlacementCodeID,
                                                                     DetPlacementStartDate = item.DetPlacementStartDate,
                                                                     UserID = UserManager.UserExtended.UserID,
                                                                     BatchLogJobID = Guid.NewGuid(),
                                                                     DetPlacementComment = item.DetPlacementComment,
                                                                     DetPlacementHearingID = item.DetPlacementHearingID
                                                                 });
                                }
                            }

                            if (item.AllegationFindingCodeID.HasValue)
                            {
                                UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_AllegationFindingUpdate_sp",
                                                            new qcal_AS_AllegationFindingUpdate_spParams()
                                                            {
                                                                AllegationID = item.AllegationID,
                                                                AllegationFindingCodeID = item.AllegationFindingCodeID,
                                                                UserID = UserManager.UserExtended.UserID,
                                                                BatchLogJobID = Guid.NewGuid()
                                                            });
                            }

                        }

                    }
                    else // Appareance sheet for child client
                    {
                        if (item.UpdateHearingAttendenceRecord && item.ClientFlag == 1)
                        {
                            UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_PersonClassificationAddRemoveICWA_sp",
                                                           new qcal_AS_PersonClassificationAddRemoveICWA_spParams()
                                                           {
                                                               PersonID = item.PersonID,
                                                               ICWAFlag = item.ICWAFlag,
                                                               UserID = UserManager.UserExtended.UserID,
                                                               BatchLogJobID = Guid.NewGuid()
                                                           });
                        }
                    }
                }
            }

            if (model.IsForParent && model.AddressChanged)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_ClientAddressContactUpdate_sp",
                             new qcal_AS_ClientAddressContactUpdate_spParams()
                             {
                                 HearingID = model.HearingID,

                                 Street = model.NewAddressStreet,
                                 City = model.NewAddressCity,
                                 StateCodeID = model.NewAddressStateCodeID,
                                 ZipCode = model.NewAddressZipCode,
                                 HomePhone = model.NewAddressPhone,
                                 MobilePhone = model.MobilePhone,
                                 EmailAddress = model.EmailAddress,
                                 PreferenceComment = model.ContactPreferenceComment,
                                 PreferenceCodeID = model.ContactPreferenceID,
                                 WorkPhone = model.WorkPhone,
                                 AddressID = model.ClientAddressContactGetModel.AddressID,
                                 AgencyID = model.ClientAddressContactGetModel.AgencyID,
                                 EmailAddressPersonContactID = model.ClientAddressContactGetModel.EmailAddressPersonContactID,
                                 MobilePhonePersonContactID = model.ClientAddressContactGetModel.MobilePhonePersonContactID,
                                 PersonAddressID = model.ClientAddressContactGetModel.PersonAddressID,
                                 PersonID = model.ClientAddressContactGetModel.PersonID,
                                 PreferenceID = model.ClientAddressContactGetModel.PreferenceID,
                                 WorkPhonePersonContactID = model.ClientAddressContactGetModel.WorkPhonePersonContactID,
                                 UserID = UserManager.UserExtended.UserID,
                                 BatchLogJobID = Guid.NewGuid()
                             });
            }

            if (model.IsForParent && model.APOFFE)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_CreateAPOFFE_sp",
                              new qcal_AS_CreateAPOFFE_spParams()
                              {
                                  HearingID = model.HearingID,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid()
                              });
            }

            if (model.CurrentDCCList != null && model.CurrentDCCList.Count() > 0)
            {
                foreach (var item in model.CurrentDCCList)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>(
                        "qcal_AS_DCCSet_sp",
                        new qcal_AS_DCCSet_spParams()
                        {
                            HearingID = model.HearingID,
                            AttendedFlag = item.AttendedFlag,
                            CounselPersonID = item.CounselPersonID,
                            FillinCounselPersonID = item.FillinCounselPersonID,
                            HearingAttendanceID = item.HearingAttendanceID,
                            CurrentDCCPersonID = item.CurrentDCCPersonID,
                            CurrentDCCRoleID = item.CurrentDCCRoleID,

                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        }).ToList();


                }

            }
            #endregion
            #region Appearing Attorney

            var AppearingAttorneyID = 0;
            if (!string.IsNullOrEmpty(model.AppearingAttorneyID))
            {
                var aapersonId = AppearingAttorneyID = model.AppearingAttorneyID.Split('|')[0].ToInt();

                var aaroleId = model.AppearingAttorneyID.Split('|')[1].ToInt();

                var aahearingAttorneyAttendanceId = model.AppearingAttorneyID.Split('|')[2];

                if (aahearingAttorneyAttendanceId != model.HearingAttendanceID.ToString() || model.HearingAttendanceID == 0)
                {
                    model.IsHoursChanged = true;
                    if (model.HearingAttendanceID > 0)
                    {
                        UtilityFunctions.DeleteRecord("pd_HearingAttendanceDelete_sp", model.HearingAttendanceID);
                    }
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = model.HearingID,
                        NewAttendingAttorneyPersonID = aaroleId > 0 ? (int?)null : aapersonId,
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
            #endregion
            if (model.HearingPrepNoteEntryChanged)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("HearingPrepNoteIUD_sp",
                            new HearingPrepNoteIUD_spParams()
                            {
                                HearingID = model.HearingID,
                                CaseID = UserManager.UserExtended.CaseID,
                                NoteID = model.HearingPrepNoteID,
                                NoteEntry = model.HearingPrepNoteEntry,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                IUD = "Auto"

                            });
            }
            if (model.HearingPrepNoteHours.HasValue && model.HearingPrepNoteHours.Value > 0)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("HearingPrepNote_HoursIUD_sp",
                            new HearingPrepNote_HoursIUD_spParams()
                            {
                                HearingID = model.HearingID,
                                CaseID = UserManager.UserExtended.CaseID,
                                WorkDate = DateTime.Now,
                                WorkHours = model.HearingPrepNoteHours,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                IUD = "INSERT"
                                ,
                                WorkID = 0
                            });

            }
            #region Hearing Update
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
                if (model.ContinuanceRequestedByID != hearingGet.HearingRequestedByCodeID)
                {


                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingUpdate_sp", new pd_HearingInsert_spParams
                    {
                        HearingID = model.HearingID,
                        CaseID = UserManager.UserExtended.CaseID,
                        AgencyID = hearingGet.AgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        HearingCourtDepartmentCodeID = hearingGet.HearingCourtDepartmentCodeID.HasValue ? hearingGet.HearingCourtDepartmentCodeID.Value : 0,
                        HearingDateTime = hearingGet.HearingDateTime.HasValue ? hearingGet.HearingDateTime.Value : DateTime.Now,
                        HearingMediaPresentFlag = hearingGet.HearingMediaPresentFlag,
                        HearingOfficerPersonID = hearingGet.HearingOfficerPersonID.HasValue ? hearingGet.HearingOfficerPersonID.Value : 0,
                        HearingTypeCodeID = hearingGet.HearingTypeCodeID.HasValue ? hearingGet.HearingTypeCodeID.Value : 0,
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1,
                        HearingRequestedByCodeID = model.ContinuanceRequestedByID,
                        HearingFollowedRecommendations = hearingGet.HearingFollowedRecommendations,
                        HearingInvoiceAmount = hearingGet.HearingInvoiceAmount,
                        HearingResultCodeID = hearingGet.HearingResultCodeID

                    });
                }
                if (model.IsHoursChanged)
                {


                    //if (model.Hours.HasValue && model.Hours.Value > 0)
                    if (model.Hours > 0 || model.HourTypeID > 0 || model.PhaseID > 0)
                    {
                        if (hearingGet.WorkID.HasValue && hearingGet.WorkID > 0)
                        {
                            var workGet = UtilityService.ExecStoredProcedureWithResults<pd_WorkGet_spResult>("pd_WorkGet_sp", new pd_WorkGet_spParams()
                            {
                                WorkID = hearingGet.WorkID.Value,
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID
                            }).FirstOrDefault();
                            if (workGet != null)
                            {

                                if (workGet.WorkHours != model.Hours || workGet.WorkPhaseCodeID != model.PhaseID || workGet.WorkDescriptionCodeID != model.HourTypeID || workGet.PersonID != AppearingAttorneyID)
                                {
                                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_WorkUpdate1_sp", new pd_WorkUpdate1_spParams()
                                    {
                                        WorkID = hearingGet.WorkID.Value,
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
                                        WorkIVeEligibleCodeID = -1
                                    });
                                }
                            }
                        }
                        else if (model.Hours > 0)
                        {
                            UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_WorkInsertByHearingID_sp", new pd_WorkInsertByHearingID_spParams()
                            {
                                HearingID = model.HearingID,
                                AttorneyPersonID = AppearingAttorneyID,
                                WorkDescriptionCodeID = model.HourTypeID,
                                WorkHours = (decimal)model.Hours,
                                WorkPhaseCodeID = model.PhaseID,

                                RecordStateID = 1,
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID
                            }).FirstOrDefault();
                        }

                    }
                    else if (hearingGet.WorkID > 0 && !model.Hours.HasValue && model.HourTypeID == 0 && model.PhaseID == 0)
                    {
                        UtilityFunctions.DeleteRecord("pd_WorkDelete_sp", hearingGet.WorkID.Value);
                    }
                }
            }

            #endregion


            #region Hearing Result

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
                    var orderBack = petitionId.Split('|')[7].ToInt();
                    var asfa = petitionId.Split('|')[8].ToInt();
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
                            var oParams = new pd_HearingPetitionUpdate_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,

                                HearingID = model.HearingID,
                                PetitionID = id,
                                HearingPetitionResultCodeID = resultId.HasValue ? resultId.Value : 0,
                                ASFAFlag = asfa,
                                OrderBackFlag = orderBack
                            };

                            if (model.IsForParent)
                            {
                                if (model.HearingAttendance.Any(x => x.PetitionID == id))
                                {
                                    var item = model.HearingAttendance.FirstOrDefault(x => x.PetitionID == id);
                                    oParams.OrderBackFlag = item.OrderBackFlag;
                                    oParams.ASFAFlag = item.ASFAFlag;
                                    oParams.AppearanceWaivedFlag = item.AppearanceWaivedFlag;
                                    oParams.NonOffendingFlag = item.NonOffendingFlag;
                                    oParams.IncarcerationFacilityCodeID = item.IncarcerationFacilityCodeID;
                                }
                            }

                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", oParams);
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
                            var oParam = new pd_HearingPetitionUpdate_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                HearingID = model.HearingID,
                                PetitionID = id,
                                HearingPetitionResultCodeID = resultId.Value,

                                ASFAFlag = asfa,
                                OrderBackFlag = orderBack

                            };

                            if (model.IsForParent)
                            {
                                if (model.HearingAttendance.Any(x => x.PetitionID == id))
                                {
                                    var item = model.HearingAttendance.FirstOrDefault(x => x.PetitionID == id);
                                    oParam.OrderBackFlag = item.OrderBackFlag;
                                    oParam.ASFAFlag = item.ASFAFlag;
                                    oParam.AppearanceWaivedFlag = item.AppearanceWaivedFlag;
                                    oParam.NonOffendingFlag = item.NonOffendingFlag;
                                    oParam.IncarcerationFacilityCodeID = item.IncarcerationFacilityCodeID;
                                }
                            }

                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", oParam);
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
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID

                        }).FirstOrDefault();
                    }
                }
                UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionAutoUpdate_sp", new pd_HearingPetitionAutoUpdate_spParams()
                {
                    HearingID = model.HearingID,

                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                });

                UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);
            }

            #endregion

            if (model.HearingNoteGetMain != null)
            {
                var note = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingNoteIUD_spResult>(
                                 "qcal_AS_HearingNoteIUD_sp",
                                 new qcal_AS_HearingNoteIUD_spParams2()
                                 {
                                     HearingID = model.HearingID,
                                     NoteTypeCodeID = model.HearingNoteGetMain.NoteTypeCodeID,
                                     UserID = UserManager.UserExtended.UserID,
                                     BatchLogJobID = Guid.NewGuid(),
                                     NoteEntry = model.HearingNoteGetMain.NoteEntry,
                                     NoteID = model.HearingNoteGetMain.NoteID,
                                     IUD = model.HearingNoteGetMain.NoteEntry.IsNullOrEmpty() && model.HearingNoteGetMain.NoteID.ToInt() > 0 ? "DELETE" : ""
                                 }).FirstOrDefault();
            }
            if (model.HearingAttendance != null && model.HearingAttendance.Count() > 0)
            {
                foreach (var item in model.HearingAttendance)
                {
                    if (!model.IsForParent)
                    {
                        if (item.UpdateCaseStatusRecord)
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
                }
            }

            var alertMessage = UtilityService.ExecStoredProcedureWithResults<Advisement_GetAlert_spResult>("Advisement_GetAlert_sp", new Advisement_GetAlert_spParams()
            {
                HearingID = model.HearingID,
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault().AdvisementAlert;

            UtilityFunctions.HearingContinuanceReasonAndRequestDataInsert(appearanceSheetViewModel: model);

            return Json(new
            {
                Status = "Done",
                AlertMessage = alertMessage
            });

        }


        public virtual ActionResult CalendarAppearanceSheetNotes(string id, string hearingId, int? codeId)
        {

            var model = new AppearanceSheetNotesViewModel();
            model.HearingID = hearingId.ToDecrypt().ToInt();
            model.NoteID = id.ToDecrypt().ToInt();
            model.ControlType = UtilityFunctions.GetNoteControlType("Task/CalendarAppearanceSheet", noteId: model.NoteID);
            if (codeId.HasValue)
            {
                model.NoteTypeID = codeId.Value;
            }
            if (model.NoteID > 0)
            {
                var note = LALoDep.Custom.UtilityFunctions.NoteGet(model.NoteID);
                if (note != null)
                {
                    model.NoteText = note.NoteEntry;
                    model.NoteTypeID = note.NoteTypeCodeID.HasValue ? note.NoteTypeCodeID.Value : 0;
                    model.CanDeleteFlag = note.CanDeleteFlag;
                    model.CanEditFlag = note.CanEditFlag;
                }
            }

            if (model.NoteTypeID > 0)
            {
                var code = LALoDep.Custom.UtilityFunctions.CodeGet(model.NoteTypeID);
                if (code != null)
                {
                    model.NoteCodeTypeName = code.CodeValue;


                }
                model.PredeterminedAnswers = UtilityService.ExecStoredProcedureWithResults<qcal_AS_PredeterminedAnswersGet_spResult>(
                            "qcal_AS_PredeterminedAnswersGet_sp",
                            new qcal_AS_PredeterminedAnswersGet_spParams()
                            {
                                HearingID = model.HearingID,
                                NoteTypeCodeID = model.NoteTypeID,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid()
                            }).ToList();
            }
            model.NotePerson = UtilityService.ExecStoredProcedureWithResults<qcal_AS_NotePersonGetAll_spResult>(
                          "qcal_AS_NotePersonGetAll_sp",
                          new qcal_AS_NotePersonGetAll_spParams()
                          {
                              HearingID = model.HearingID,
                              NoteID = model.NoteID,
                              UserID = UserManager.UserExtended.UserID,
                              BatchLogJobID = Guid.NewGuid()
                          }).ToList();
            return View(model);

        }
        [HttpPost]
        public virtual ActionResult CalendarAppearanceSheetNotes(AppearanceSheetNotesViewModel model)
        {

            var note = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingNoteIUD_spResult>(
                                  "qcal_AS_HearingNoteIUD_sp",
                                  new qcal_AS_HearingNoteIUD_spParams()
                                  {
                                      HearingID = model.HearingID,
                                      NoteTypeCodeID = model.NoteTypeID,
                                      UserID = UserManager.UserExtended.UserID,
                                      BatchLogJobID = Guid.NewGuid(),
                                      NoteEntry = model.NoteText,
                                      NoteID = model.NoteID

                                  }).FirstOrDefault();
            if (note != null)
            {
                if (note.NoteID.HasValue)
                {
                    if (model.NotePerson != null)
                    {
                        foreach (var item in model.NotePerson)
                        {
                            var iud = "";
                            if (item.Selected == 1 && item.NotePersonID == 0)
                            {
                                iud = "INSERT";

                            }
                            else if (item.Selected == 0 && item.NotePersonID > 0)
                            {
                                iud = "DELETE";

                            }
                            if (!iud.IsNullOrEmpty())
                            {



                                UtilityService.ExecStoredProcedureWithResults<object>(
                                       "qcal_AS_NotePersonIUD_sp",
                                       new qcal_AS_NotePersonIUD_spParams()
                                       {
                                           NotePersonID = item.NotePersonID,
                                           PersonID = item.PersonID,
                                           IUD = iud,
                                           UserID = UserManager.UserExtended.UserID,
                                           BatchLogJobID = Guid.NewGuid(),
                                           NoteID = note.NoteID.Value

                                       }).ToList();
                            }
                        }
                    }

                }
            }


            return Json(new
            {
                Status = "Done"


            });
        }
        [HttpPost]
        public virtual ActionResult CalendarAppearanceSheetNotesDelete(int id)
        {
            var note = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingNoteIUD_spResult>(
                             "qcal_AS_HearingNoteIUD_sp",
                             new qcal_AS_HearingNoteIUD_spParams()
                             {
                                 UserID = UserManager.UserExtended.UserID,
                                 BatchLogJobID = Guid.NewGuid(),

                                 NoteID = id,
                                 IUD = "DELETE"

                             }).FirstOrDefault();

            return Json(new { Status = "Done" });
        }
        public virtual ActionResult CalendarActivitySheet(string id, string caseId)
        {
            ViewBag.HearingID = id.ToDecrypt();
            if (caseId.ToDecrypt().ToInt() > 0)
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());

            var data = UtilityService.ExecStoredProcedureWithResults<qcal_ActivitySheet_spResult>("qcal_ActivitySheet_sp",
                   new qcal_ActivitySheet_spParams()
                   {
                       HearingID = id.ToDecrypt().ToInt(),
                       CaseID = caseId.ToDecrypt().ToInt(),
                       BatchLogJobID = Guid.NewGuid(),
                       UserID = UserManager.UserExtended.UserID,
                   }).OrderByDescending(o => o.SortDate).ToList();
            return View(data);

        }


        public virtual ActionResult CalendarMostRecentAR(string id, string caseId)
        {
            ViewBag.HearingID = id.ToDecrypt();
            if (caseId.ToDecrypt().ToInt() > 0)
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());
            var model = new ActionRequestModel
            {
                ActionRequestList =
                   UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetByCaseID_spResults>(
                       "pd_HearingReportFilingDueGetByCaseID_sp",
                       new pd_HearingReportFilingDueGetByCaseID_spParams()
                       {
                           CaseID = UserManager.UserExtended.CaseID,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).ToList()


            };
            foreach (var item in model.ActionRequestList)
            {
                if (item.HearingReportFilingDueID != null)
                {
                    var profile = UtilityService.ExecStoredProcedureWithResults<pd_ProfileGetList_spResult>("pd_ProfileGetList_sp",
                        new pd_ProfileGetList_spParams
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            RFDID = item.HearingReportFilingDueID.Value,
                            BatchLogJobID = Guid.NewGuid()
                        }).ToList();
                    model.ProfileList.AddRange(profile);

                }
            }

            var hearingReportFilingDueId = UtilityService.ExecStoredProcedureWithResults<qcal_MostRecentAR_spResult>("qcal_MostRecentAR_sp",
                    new qcal_MostRecentAR_spParams()
                    {
                        HearingID = id.ToDecrypt().ToInt(),
                        CaseID = caseId.ToDecrypt().ToInt(),
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                    }).FirstOrDefault();
            if (hearingReportFilingDueId != null)
            {

                var profile = UtilityService.ExecStoredProcedureWithResults<pd_ProfileGetList_spResult>("pd_ProfileGetList_sp",
                      new pd_ProfileGetList_spParams
                      {
                          CaseID = UserManager.UserExtended.CaseID,
                          UserID = UserManager.UserExtended.UserID,
                          RFDID = hearingReportFilingDueId.HearingReportFilingDueID,
                          BatchLogJobID = Guid.NewGuid()
                      }).FirstOrDefault();
                if (profile != null)
                {
                    model.PrintARProfile = true;
                }
                else
                {
                    model.PrintAR = true;
                }
                model.MostRecentARHearingRequestID = hearingReportFilingDueId.HearingReportFilingDueID;

            }
            return View(model);

        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile)]
        public virtual ActionResult CalendarAttachedFiles(string id, string caseId)
        {
            ViewBag.HearingID = id.ToDecrypt();
            if (caseId.ToDecrypt().ToInt() > 0)
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());
            var model = new AttachPathViewModel
            {
                CaseFileGetPathByCaseList =
                     UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                         "CaseFileGetPathByCaseID_sp",
                         new CaseFileGetPathByCaseID_spParams
                         {
                             BatchLogJobID = Guid.NewGuid(),
                             CaseID = UserManager.UserExtended.CaseID,
                             UserID = UserManager.UserExtended.UserID,


                         }).ToList(),
                RoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                     "pd_RoleGetByCaseIDChildRespondent_sp",
                     new pd_RoleGetByCaseIDChildRespondent_spParams
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,


                     }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.RoleID.ToString() }),
                CategoryList =
                     UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(900)
                         .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() })
                         .ToList(),

                FileDate = DateTime.Now.ToShortDateString()
            };
            model.UseGoogleDriveUpload = model.CaseFileGetPathByCaseList.Any(o => o.UseGoogleDocsFlag.HasValue && o.UseGoogleDocsFlag.Value == 1) ? 1 : 0;
            var caseFileGetPath = model.CaseFileGetPathByCaseList.FirstOrDefault();
            if (caseFileGetPath != null)
            {
                if (caseFileGetPath.UseGoogleDocsFlag.HasValue && caseFileGetPath.UseGoogleDocsFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
                    var parentFolderId = "";
                    if (environment == "Test")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_PROD;

                    }
                    if (!parentFolderId.IsNullOrEmpty())
                        model.UseGoogleDriveUpload = caseFileGetPath.UseGoogleDocsFlag.Value;

                }

                if (caseFileGetPath.SharePoint_UseFlag.HasValue && caseFileGetPath.SharePoint_UseFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];

                    if (environment == "Test" || environment == "Dev")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_PROD;

                    }

                    model.SharePoint_UseFlag = caseFileGetPath.SharePoint_UseFlag.Value;
                }
            }

            var settings = UtilityFunctions.JcatsNGConfigGetAll(UserManager.UserExtended.CaseID);

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes))
                ViewBag.AttachFileTypes = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes).JcatsNGConfigValue;
            else
                ViewBag.AttachFileTypes = "gif|jpe?g|png|pdf|txt|docx|doc|xls|xlsx|csv|ppt|pptx";

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize))
                ViewBag.AttachFileMaxSize = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize).JcatsNGConfigValue;
            else
                ViewBag.AttachFileMaxSize = "15000000";

            return View(model);

        }

        public virtual ActionResult CalendarAppearanceNotes(string id, string caseId)
        {
            int intHearingId = id.ToDecrypt().ToInt();
            if (!caseId.IsNullOrEmpty())
            {
                int intCaseId = caseId.ToDecrypt().ToInt();
                if (intCaseId > 0)
                    UserManager.UpdateCaseStatusBar(intCaseId);
            }

            ViewBag.HearingID = intHearingId;



            var result =
                     UtilityService.ExecStoredProcedureWithResults<qcal_AppearanceNotes_spResult>(
                         "qcal_AppearanceNotes_sp",
                         new qcal_AppearanceNotes_spParams
                         {
                             CaseID = UserManager.UserExtended.CaseID,
                             HearingID = intHearingId,
                             UserID = UserManager.UserExtended.UserID,
                             BatchLogJobID = Guid.NewGuid(),

                         }).ToList();
            return View(result);
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing)]
        public virtual ActionResult QuickCalNextHearing(string id)
        {
            if (Request.QueryString["CaseID"] != null)
            {
                if (Request.QueryString["CaseID"].ToDecrypt().ToInt() > 0)
                    UserManager.UpdateCaseStatusBar(Request.QueryString["CaseID"].ToDecrypt().ToInt());
            }

            var model = new HearingModel
            {
                HearingOfficerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp", new pd_HearingOfficerGet_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID
                }).Select(o => new SelectListItem() { Value = o.PersonID.ToString(), Text = o.PersonNameLast + ", " + o.PersonNameFirst }).ToList(),

            };


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
                model.HearingTime = caseDefault.DefaultHearingTime;
                model.DefaultHearingTimeContested = caseDefault.DefaultHearingTimeContested;
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
            }
            var qNextHearingDefaultGet =
              UtilityService.ExecStoredProcedureWithResults<qcal_AS_NextHearingDefaultGet_spResult>("qcal_AS_NextHearingDefaultGet_sp",
                  new qcal_AS_NextHearingDefaultGet_spParams
                  {
                      UserID = UserManager.UserExtended.UserID,

                      BatchLogJobID = Guid.NewGuid(),
                      CaseID = UserManager.UserExtended.CaseID,
                      PreviousHearingID = id.ToDecrypt().ToInt()
                  }).FirstOrDefault();
            if (qNextHearingDefaultGet != null)
            {
                model.Note = qNextHearingDefaultGet.DefaultNote;
            }
            model.PetitionType602List = UtilityFunctions.CodeGetBySystemValueTypeId(179);
            model.HearingType602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(239);

            model.HearingResult602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(240);

            model.CourtDepartment602PetitionList = UtilityFunctions.CodeGetBySystemValueTypeId(241);
            model.HearingResultContinuanceList = UtilityFunctions.CodeGetBySystemValueTypeId(47);
            model.HearingTypeNonHearingEventList = UtilityFunctions.CodeGetBySystemValueTypeId(214);
            model.HearingResultFutureHearingsList = UtilityFunctions.CodeGetBySystemValueTypeId(207);
            model.HearingTypeContestedList = UtilityFunctions.CodeGetBySystemValueTypeId(46, agencyId: UserManager.UserExtended.CaseNumberAgencyID);


            var summary = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (summary != null)
            {
                model.CaseDepartmentID = summary.DepartmentID ?? 0;

            }
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

                    if (hearingGet.HearingCourtDepartmentCodeID != null)
                        model.DepartmentID = hearingGet.HearingCourtDepartmentCodeID.Value;
                    if (hearingGet.HearingOfficerPersonID != null)
                        model.HearingOfficerID = hearingGet.HearingOfficerPersonID.Value;
                    //if (hearingGet.HearingTypeCodeID != null)
                    //    model.HearingTypeID = hearingGet.HearingTypeCodeID.Value;
                    // model.Note = hearingGet.NoteEntry;
                    //  model.NoteID = hearingGet.NoteID;
                    if (hearingGet.HearingID != null)
                        model.HearingID = hearingGet.HearingID.Value;




                    model.PetitionByHearingList =
                      UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetAllByHearingID_spResult>(
                          "pd_PetitionGetAllByHearingID_sp", new pd_PetitionGetAllByHearingID_spParams
                          {
                              UserID = UserManager.UserExtended.UserID,

                              BatchLogJobID = Guid.NewGuid(),
                              HearingID = hearingGet.HearingID,

                          }).ToList();
                }


                model.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10, includeCodeId: model.HearingTypeID);
                model.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, includeCodeId: model.DepartmentID);
                model.HourTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(222, includeCodeId: model.HourTypeID);
                model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: model.PhaseID);


                return View(model);
            }
            else
            {
                model.PetitionByHearingList =
                UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetAllByHearingID_spResult>(
                    "pd_PetitionGetAllByHearingID_sp", new pd_PetitionGetAllByHearingID_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,

                        BatchLogJobID = Guid.NewGuid(),

                        CaseID = UserManager.UserExtended.CaseID
                    }).ToList();
                model.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10, includeCodeId: model.HearingTypeID);
                model.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, includeCodeId: model.DepartmentID);
                model.HourTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(222, includeCodeId: model.HourTypeID);
                model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: model.PhaseID);
            }


            return View(model);
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningHearing, IsCasePage = true)]
        public virtual JsonResult QuickCalNextHearingSave(HearingModel model)
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
                HearingMediaPresentFlag = 0,
                HearingOfficerPersonID = model.HearingOfficerID,
                HearingTypeCodeID = model.HearingTypeID,
                UserID = UserManager.UserExtended.UserID,
                RecordStateID = 1,

            }).FirstOrDefault();

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

                    var appearanceRequiredFlag = petitionId.Split('|')[1].ToInt();

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        HearingID = (int)hearingId,
                        PetitionID = id,
                        AppearanceRequiredFlag = appearanceRequiredFlag

                    }).FirstOrDefault();

                }
            }


            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionAutoUpdate_sp", new pd_HearingPetitionAutoUpdate_spParams()
            {
                HearingID = (int)hearingId,

                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            });



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
            return Json(new { Status = "Done", Data = model });
        }



        public virtual ActionResult QuickCalDCCAndNewPrivateCounsel(string id, string addMode)
        {
            var model = new QuickCalDCCAndNewPrivateCounselViewModel();
            model.HearingID = id.ToDecrypt().ToInt();
            model.AddMode = addMode;
            return View(model);

        }

        [HttpPost]
        public virtual ActionResult QuickCalDCCAndNewPrivateCounsel(QuickCalDCCAndNewPrivateCounselViewModel model)
        {
            if (model.HearingID.HasValue && !model.FirstName.IsNullOrEmpty() && !model.AddMode.IsNullOrEmpty())
            {

                var result = UtilityService.ExecStoredProcedureWithResults<qcal_AS_AddNewStaff_spResult>("qcal_AS_AddNewStaff_sp", new qcal_AS_AddNewStaff_spParams
                {

                    BatchLogJobID = Guid.NewGuid(),

                    UserID = UserManager.UserExtended.UserID,
                    AddMode = model.AddMode,
                    FirstName = model.FirstName,
                    HearingID = model.HearingID,
                    LastName = model.LastName

                }).FirstOrDefault();

                if (result != null)
                {
                    return Json(new
                    {
                        Status = "Done",
                        ErrorMessage = result.ErrorMessage,
                        NewPersonID = result.NewPersonID.HasValue ? result.NewPersonID.Value : 0,
                        NewPersonNameID = result.NewPersonNameID.HasValue ? result.NewPersonNameID.Value : 0,
                        NewRoleID = result.NewRoleID.HasValue ? result.NewRoleID.Value : 0
                    });
                }
            }
            return Json(new
            {
                Status = "Done",
                ErrorMessage = "HearingID or First Name is required",
                NewPersonID = 0,
                NewPersonNameID = 0,
                NewRoleID = 0
            });

        }


        [HttpPost]
        public virtual ActionResult QuickCalDCCAndNewPrivateCounselPopulateDropdown(string mode, int hearingId)
        {
            var list = new List<SelectListItem>();
            if (mode == "DCC")
            {
                list = UtilityService.ExecStoredProcedureWithResults<qcal_AS_DCCGetList_spResult>(
                  "qcal_AS_DCCGetList_sp",
                  new qcal_AS_DCCGetList_spParams()
                  {
                      HearingID = hearingId,
                      UserID = UserManager.UserExtended.UserID,
                      BatchLogJobID = Guid.NewGuid()
                  }).Select(o => new SelectListItem() { Text = o.FullName, Value = o.PersonID.ToString() }).ToList();

            }
            else
            {
                list = UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingCounselGetList_spResult>(
                                "qcal_AS_HearingCounselGetList_sp",
                                new qcal_AS_HearingCounselGetList_spParams()
                                {
                                    HearingID = hearingId,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                }).Select(o => new SelectListItem() { Text = o.FullName, Value = o.PersonID.ToString() }).ToList();



            }
            return Json(new { Status = "Done", SelectList = list });
        }

        [HttpPost]
        public virtual ActionResult PrintAppearanceSheet()
        {

            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 118,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();

            string fileName = "AppearanceSheet.pdf";
            try
            {


                var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

                rpt.FileName = HttpContext.Server.MapPath("~/Reports/AppearanceSheetPrintable_PreHearingVersion.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_AppearanceSheetPrintable_PreHearingVersion_sp", com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "AppearanceSheetPrintable_PreHearingVersion.rpt"))
                {
                    var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams));
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                }


                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

            }
            catch (Exception ex)
            {



            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }


            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }

        public virtual ActionResult CalendarToDo(string id, string caseId, string pdActionId)
        {
            if (UserManager.UserExtended.CaseID != caseId.ToDecrypt().ToInt())
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());
            ViewBag.HearingID = id.ToDecrypt();
            var model = new CalendarToDoViewModel();
            var hearingId = id.ToDecrypt().ToInt();
            model.PDActionID = pdActionId.ToDecrypt().ToInt();
            model.Heading = (string.IsNullOrEmpty(pdActionId)) ? "Add" : "Edit";
            model.HearingID = hearingId;
            model.AssignToStaffList = UtilityService.ExecStoredProcedureWithResults<qcal_ToDo_AssignToStaff_spResult>("qcal_ToDo_AssignToStaff_sp", new qcal_ToDo_AssignToStaff_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                HearingID = hearingId,
                PDActionID = model.PDActionID
            }).Select(o => new SelectListItem() { Text = o.FullName, Value = o.PersonID.ToString(), Selected = (o.DefaultFlag.Value == 1) }).ToList();

            model.ActionTypeList = UtilityService.ExecStoredProcedureWithResults<qcal_ToDo_ActionType_spResult>("qcal_ToDo_ActionType_sp", new qcal_ToDo_ActionType_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                HearingID = hearingId,
                PDActionID = model.PDActionID
            }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();
            if (model.PDActionID.HasValue && model.PDActionID.Value > 0)
            {
                var oPdAction = UtilityService.ExecStoredProcedureWithResults<pd_PDActionGet_spResults>("pd_PDActionGet_sp", new pd_PDActionGet_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),

                    UserID = UserManager.UserExtended.UserID,

                    PDActionID = model.PDActionID.Value
                }).FirstOrDefault();
                if (oPdAction != null)
                {
                    model.PdActionOldModel = oPdAction;
                    model.ActionTypeID = oPdAction.ActionTypeCodeID;
                    model.AssignToStaffID = oPdAction.AssignedToPersonID;
                    model.ActionNote = oPdAction.ActionNote;
                    model.DueDate = oPdAction.ActionDueDate;
                    model.ReminderDate = oPdAction.ActionReminderDate;

                }

            }
            return View(model);
        }

        public virtual ActionResult CalendarToDoHistory(int id, int showAll = 0)
        {
            var actionList = UtilityService.ExecStoredProcedureWithResults<qcal_ToDo_History_spResult>("qcal_ToDo_History_sp", new qcal_ToDo_History_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                HearingID = id
            }).Where(o => o.Completed == 0 || showAll == 1).ToList();

            var list = actionList.Select(x => new
            {
                ActionType = x.ActionType,
                ActionNote = x.ActionNote,

                ActionReminderDate = x.ActionReminderDate,
                ActionDueDate = x.ActionDueDate,
                PDActionEcryptedID = x.PDActionID.ToEncrypt(),
                PDActionID = x.PDActionID,
                ActionStatusCodeID = x.ActionStatusCodeID,
                AssignedTo = x.AssignedTo,
                Completed = x.Completed
            }).ToList();
            var total = list.Count > 0 ? list.Count : 0;
            return Json(new DataTablesResponse(0, list, total, total), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public virtual JsonResult CalendarToDo(CalendarToDoViewModel model)
        {

            if (model.PdActionOldModel != null)
            {
                if (model.ActionTypeID != model.PdActionOldModel.ActionStatusCodeID ||
                    model.ActionNote != model.PdActionOldModel.ActionNote ||
                    model.DueDate != model.PdActionOldModel.ActionDueDate ||
                    model.ReminderDate != model.PdActionOldModel.ActionReminderDate ||
                    model.AssignToStaffID != model.PdActionOldModel.AssignedToPersonID
                    )
                {

                    UtilityService.ExecStoredProcedureWithoutResults(
                      "pd_PDActionUpdate_sp",
                      new pd_PDActionUpdate_spParams()
                      {
                          ActionDueDate = model.DueDate,
                          ActionReminderDate = model.ReminderDate,
                          ActionNote = model.ActionNote,
                          ActionTypeCodeID = model.ActionTypeID,
                          AssignedToPersonID = model.AssignToStaffID,
                          PDActionID = model.PdActionOldModel.PDActionID,
                          ActionAssociatedToEntityID = model.PdActionOldModel.ActionAssociatedToEntityID,
                          ActionAssociatedToEntityTypeCodeID = model.PdActionOldModel.ActionAssociatedToEntityTypeCodeID,
                          ActionStatusCodeID = model.PdActionOldModel.ActionStatusCodeID,
                          ActionStatusDate = model.PdActionOldModel.ActionStatusDate,
                          AgencyID = model.PdActionOldModel.AgencyID,
                          CaseID = model.PdActionOldModel.CaseID,
                          BranchID = model.PdActionOldModel.BranchID,
                          PersonID = model.PdActionOldModel.PersonID,
                          RecordStateID = model.PdActionOldModel.RecordStateID.Value,
                          UserID = UserManager.UserExtended.UserID,
                          BatchLogJobID = Guid.NewGuid()
                      });
                }
            }
            else if (model.ActionTypeID.HasValue &&

                   !model.DueDate.IsNullOrEmpty() &&
                   !model.ReminderDate.IsNullOrEmpty() &&
                   model.AssignToStaffID.HasValue
                   )
            {

                var id = UtilityService.ExecStoredProcedureScalar(
                     "pd_PDActionInsert_sp",
                     new pd_PDActionInsert_spParams()
                     {
                         ActionDueDate = model.DueDate,
                         ActionReminderDate = model.ReminderDate,
                         ActionNote = model.ActionNote,
                         ActionTypeCodeID = model.ActionTypeID,
                         AssignedToPersonID = model.AssignToStaffID,
                         ActionStatusCodeID = 3382,
                         ActionStatusDate = DateTime.Now.ToString("d"),
                         AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                         CaseID = UserManager.UserExtended.CaseID,
                         BranchID = UserManager.UserExtended.CaseNumberBranchID,
                         RecordStateID = 1,
                         UserID = UserManager.UserExtended.UserID,
                         BatchLogJobID = Guid.NewGuid()
                     });


            }


            if (!model.PDActionCompletedIds.IsNullOrEmpty())
            {

                UtilityService.ExecStoredProcedureWithoutResults(
                "pd_PDActionUpdateStatusOnly_sp",
                new pd_PDActionUpdateStatusOnly_spParams()
                {
                    PDActionIDList = model.PDActionCompletedIds,
                    ActionStatusDate = DateTime.Now.ToString("d"),
                    ActionStatusCodeID = 3383,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }



            return Json(new
            {
                Status = "Done"
            });

        }

        [HttpPost]
        public virtual ActionResult PrintCalendarAppearanceNotes(int? reportId, int? hearingId)
        {

            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = reportId.Value,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            List<object> valueDelete = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueDelete_sp", com_ReportSourceGetByReportID_spParams).ToList();
            List<object> headerDelete = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportSourceGetByReportID_spParams).ToList();

            if (hearingId.HasValue)
            {
                var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                {
                    ReportID = reportId.Value,
                    Sequence = 1,
                    Value = hearingId.Value.ToString(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
            }



            string fileName = "AppearanceSheet.pdf";
            var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp",
                                                                                 com_ReportSourceGetByReportID_spParams).ToList();
            var rpt = new ReportClass { FileName = HttpContext.Server.MapPath("~/Reports/" + spResult[0].ReportSourceDocumentName) };

            try
            {


                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable(spResult[0].ReportSourceStoredProcedureName.Replace(".dbo", ""), com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);

                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != spResult[0].ReportSourceDocumentName))
                {
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(
                                        UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace(".dbo", ""),
                                        com_ReportSourceGetByReportID_spParams));
                }
                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);


            }
            catch (Exception ex)
            {



            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }


            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }

        public virtual ActionResult QuickCalAddNewHearing(int id)
        {
            var viewModel = new QuickCalAddNewHearingCaseViewModels();

            var data = UtilityService.ExecStoredProcedureWithResults<qa_CaseloadGetForHearingAdd_spResult>("qa_CaseloadGetForHearingAdd_sp",
                     new qa_CaseloadGetForHearingAdd_spParams()
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                         AttorneyPersonID = id

                     }).ToList();

            viewModel.HearingList = data;
            viewModel.AttorneyPersonID = id;

            return View(viewModel);

        }


        public virtual ActionResult HearingPreparationHours(string id)
        {
            var viewModel = new HearingPreparationHoursViewModels();

            viewModel.HoursHistory = UtilityService.ExecStoredProcedureWithResults<HearingPrepNote_HoursGetHistory_spResult>("HearingPrepNote_HoursGetHistory_sp",
                     new HearingPrepNote_HoursGetHistory_spParams()
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                         HearingID = id.ToDecrypt().ToInt(),
                         CaseID = UserManager.UserExtended.CaseID,

                     }).ToList();


            viewModel.HearingID = id.ToDecrypt().ToInt();

            return View(viewModel);

        }
        public virtual JsonResult HearingPreparationHoursSave(HearingPreparationHoursViewModels model)
        {

            if (model.HoursHistory.Any())
            {
                foreach (var item in model.HoursHistory)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("HearingPrepNote_HoursIUD_sp",
                        new HearingPrepNote_HoursIUD_spParams()
                        {
                            HearingID = model.HearingID,
                            CaseID = UserManager.UserExtended.CaseID,
                            WorkDate = item.WorkDate,
                            WorkHours = item.WorkHours,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            IUD = "AUTO",
                            WorkID = item.WorkID

                        });
                }
            }

            return Json(new { Status = "Done" });
        }




        public virtual ActionResult QuickCalNewFacility(string id)
        {
            var model = new QuickCalNewFacilityViewModel();
            model.HearingID = id.ToDecrypt().ToInt();

            return View(model);

        }

        [HttpPost]
        public virtual ActionResult QuickCalNewFacility(QuickCalNewFacilityViewModel model)
        {
            if (model.HearingID.HasValue && !model.FacilityName.IsNullOrEmpty())
            {

                var result = UtilityService.ExecStoredProcedureWithResults<qcal_AS_IncarcerationFacilityAddNew_spResult>("qcal_AS_IncarcerationFacilityAddNew_sp", new qcal_AS_IncarcerationFacilityAddNew_spParams
                {

                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    HearingID = model.HearingID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
                    FacilityName = model.FacilityName

                }).FirstOrDefault();

                if (result != null)
                {
                    return Json(new
                    {
                        Status = "Done",
                        ErrorMessage = "",
                        CodeID = result.CodeID,
                        CodeDisplay = result.CodeDisplay,

                    });
                }
            }
            return Json(new
            {
                Status = "Done",
                ErrorMessage = "HearingID or FacilityName is required",

            });

        }

        #region Quick Add Adult
        public virtual ActionResult QuickAddAdult()
        {
            var viewModel = new QuickAddAdultViewModel()
            {
                PetitionList = UtilityService.ExecStoredProcedureWithResults<qa_PetitionGetForAdultPartyAdd_spResult>("qa_PetitionGetForAdultPartyAdd_sp",
                      new qa_PetitionGetForAdultPartyAdd_spParams()
                      {
                          LoadMode = "AddAdult",
                          AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                          CaseID = UserManager.UserExtended.CaseID,
                          UserID = UserManager.UserExtended.UserID,
                          BatchLogJobID = Guid.NewGuid()
                      }).ToList(),

                AssociationList = UtilityService.ExecStoredProcedureWithResults<qa_AssociationCodes_spResult>("qa_AssociationCodes_sp",
                      new qa_AssociationCodes_spParams()
                      {
                          LoadMode = "AddAdult",
                          AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                          CaseID = UserManager.UserExtended.CaseID,
                          UserID = UserManager.UserExtended.UserID,
                          BatchLogJobID = Guid.NewGuid()
                      }).ToList(),
                RoleList = UtilityService.ExecStoredProcedureWithResults<qa_RoleCodes_spResult>("qa_RoleCodes_sp",
                      new qa_RoleCodes_spParams()
                      {
                          LoadMode = "AddAdult",
                          AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                          CaseID = UserManager.UserExtended.CaseID,
                          UserID = UserManager.UserExtended.UserID,
                          BatchLogJobID = Guid.NewGuid()
                      }).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() })

            };

            viewModel.RaceList = UtilityFunctions.CodeGetByTypeIdAndUserId(35, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            viewModel.SexList = UtilityFunctions.CodeGetByTypeIdAndUserId(1, agencyId: UserManager.UserExtended.CaseNumberAgencyID);


            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult QuickAddAdult(QuickAddAdultViewModel viewModel)
        {
            var insertedRoleResult = UtilityService.ExecStoredProcedureWithResults<qa_PersonRoleAdd_spResult>("qa_PersonRoleAdd_sp",
                  new qa_PersonRoleAdd_spParams()
                  {
                      AddMode = "AddAdult",
                      AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                      CaseID = UserManager.UserExtended.CaseID,
                      PersonNameLast = viewModel.LastName,
                      PersonNameFirst = viewModel.FirstName,
                      PersonDOB = viewModel.BirthDate.ToDateTimeValue(),
                      PersonRaceCodeID = viewModel.RaceCodeID,
                      PersonSexCodeID = viewModel.SexCodeID,
                      RoleTypeCodeID = viewModel.RoleTypeID,
                      UserID = UserManager.UserExtended.UserID,
                      BatchLogJobID = Guid.NewGuid()
                  }).FirstOrDefault();

            if (viewModel.PetitionList != null && viewModel.PetitionList.Any())
            {
                foreach (var item in viewModel.PetitionList)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_AssociationInsert_sp",
                              new pd_AssociationInsert_spParams()
                              {
                                  AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                  PersonID = insertedRoleResult.PersonID ?? 0,
                                  RelatedPersonID = item.ChildPersonID ?? 0,
                                  AssociationCodeID = item.DefaultAssocationTypeCodeID ?? 0,
                                  AssociationStartDate = item.PetitionFileDate.ToDateTimeNullableValue(),
                                  RecordStateID = 1,
                                  CaseID = UserManager.UserExtended.CaseID,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid()
                              });
                }

                if (viewModel.PetitionList.Any(x => x.PetitionID > 0))
                {
                    var ids = string.Join(",", viewModel.PetitionList.Where(x => x.PetitionID > 0).Select(c => c.PetitionID));

                    UtilityService.ExecStoredProcedureWithoutResults("qa_PetitionRoleAdd_sp",
                               new qa_PetitionRoleAdd_spParams()
                               {
                                   AddMode = "AddAdult",
                                   AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                   CaseID = UserManager.UserExtended.CaseID,
                                   AdultRoleID = insertedRoleResult.RoleID,
                                   PetitionIDList = ids,
                                   UserID = UserManager.UserExtended.UserID,
                                   BatchLogJobID = Guid.NewGuid()
                               });
                }
            }

            return Json(new { isSuccess = true });
        }

        public virtual ActionResult QuickAddChild()
        {
            var viewModel = new QuickAddChildViewModel();
            viewModel.RoleList = UtilityService.ExecStoredProcedureWithResults<qa_RoleGetForPetitionAdd_spResult>("qa_RoleGetForPetitionAdd_sp",
                                          new qa_RoleGetForPetitionAdd_spParams()
                                          {
                                              LoadMode = "AddChild",
                                              AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                              CaseID = UserManager.UserExtended.CaseID,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid()
                                          }).ToList();

            viewModel.HearingList = UtilityService.ExecStoredProcedureWithResults<qa_HearingGetForPetitionAdd_spResult>("qa_HearingGetForPetitionAdd_sp",
                                          new qa_HearingGetForPetitionAdd_spParams()
                                          {
                                              LoadMode = "AddChild",
                                              AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                              CaseID = UserManager.UserExtended.CaseID,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid()
                                          }).ToList();

            viewModel.AssociationList = UtilityService.ExecStoredProcedureWithResults<qa_AssociationCodes_spResult>("qa_AssociationCodes_sp",
                                          new qa_AssociationCodes_spParams()
                                          {
                                              LoadMode = "AddChild",
                                              AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                              CaseID = UserManager.UserExtended.CaseID,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid()
                                          }).ToList();

            viewModel.RaceList = UtilityFunctions.CodeGetByTypeIdAndUserId(35, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            viewModel.SexList = UtilityFunctions.CodeGetByTypeIdAndUserId(1, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            viewModel.PetitionTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(3, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            viewModel.AllegationList = UtilityFunctions.CodeGetByTypeIdAndUserId(22, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult QuickAddChild(QuickAddChildViewModel viewModel)
        {
            var insertedRoleResult = UtilityService.ExecStoredProcedureWithResults<qa_PersonRoleAdd_spResult>("qa_PersonRoleAdd_sp",
                 new qa_PersonRoleAdd_spParams()
                 {
                     AddMode = "CHILD",
                     AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                     CaseID = UserManager.UserExtended.CaseID,
                     PersonNameLast = viewModel.LastName,
                     PersonNameFirst = viewModel.FirstName,
                     PersonDOB = viewModel.BirthDate.ToDateTimeValue(),
                     PersonRaceCodeID = viewModel.RaceCodeID,
                     PersonSexCodeID = viewModel.SexCodeID,
                     RoleStartDate = viewModel.PetitionFileDate.ToDateTimeValue(),
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = Guid.NewGuid()
                 }).FirstOrDefault();

            if (insertedRoleResult != null)
            {
                if (viewModel.RoleList != null && viewModel.RoleList.Any())
                {
                    foreach (var item in viewModel.RoleList)
                    {
                        UtilityService.ExecStoredProcedureWithoutResults("pd_AssociationInsert_sp",
                              new pd_AssociationInsert_spParams()
                              {
                                  AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                  PersonID = item.PersonID,
                                  RelatedPersonID = insertedRoleResult.PersonID,

                                  AssociationCodeID = item.DefaultAssociationTypeCodeID,
                                  AssociationStartDate = viewModel.PetitionFileDate.ToDateTimeValue(),
                                  RecordStateID = 1,
                                  CaseID = UserManager.UserExtended.CaseID,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid()
                              });
                    }

                    var otherRoleList = string.Join(",", viewModel.RoleList.Where(x => x.RoleID > 0).Select(x => x.RoleID));

                    string hearingList = string.Empty;
                    if (viewModel.HearingList != null && viewModel.HearingList.Any(x => x.HearingID > 0))
                        hearingList = string.Join(",", viewModel.HearingList.Where(x => x.HearingID > 0).Select(x => x.HearingID));

                    UtilityService.ExecStoredProcedureWithoutResults("qa_PetitionAdd_sp",
                             new qa_PetitionAdd_spParams()
                             {
                                 AddMode = "CHILD",
                                 AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                 CaseID = UserManager.UserExtended.CaseID,
                                 PetitionTypeCodeID = viewModel.PetitionTypeCodeID,
                                 PetitionFileDate = viewModel.PetitionFileDate.ToDateTimeValue(),
                                 PetitionDocketNumber = viewModel.CaseNumber,
                                 ChildRoleID = insertedRoleResult.RoleID,
                                 OtherRoleIDList = otherRoleList,
                                 AllegationTypeCodeID1 = viewModel.Allegation1,
                                 AllegationTypeCodeID2 = viewModel.Allegation2,
                                 AllegationTypeCodeID3 = viewModel.Allegation3,
                                 AllegationTypeCodeID4 = viewModel.Allegation4,
                                 AllegationTypeCodeID5 = viewModel.Allegation5,
                                 HearingIDList = hearingList,
                                 UserID = UserManager.UserExtended.UserID,
                                 BatchLogJobID = Guid.NewGuid()
                             });
                }
            }

            return Json(new { isSuccess = true });
        }


        public virtual ActionResult ClientStatusHistory(int personId, int hearingId)
        {
            var list = UtilityService.ExecStoredProcedureWithResults<ClientStatusGetHistory_spResult>("ClientStatusGetHistory_sp",
                                             new ClientStatusGetHistory_spParams()
                                             {
                                                 HearingID = hearingId,
                                                 PersonID = personId,
                                                 CaseID = UserManager.UserExtended.CaseID,
                                                 UserID = UserManager.UserExtended.UserID,
                                                 BatchLogJobID = Guid.NewGuid()
                                             }).ToList();
            return View(list);
        }

        #endregion

        #region QuickCal Current ERH
        public virtual ActionResult QuickCalCurrentERH(string id)
        {
            var model = new QuickCalCurrentERHViewModel();
            model.HearingID = id.ToDecrypt().ToInt();

            #region load data
            model.ERHChildRoles = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ERH_ChildRoles_spResult>("qcal_AS_ERH_ChildRoles_sp", new qcal_AS_ERH_ChildRoles_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = model.HearingID
            }).ToList();
            model.ERHHistory = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ERH_History_spResult>("qcal_AS_ERH_History_sp", new qcal_AS_ERH_History_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),

                UserID = UserManager.UserExtended.UserID,

                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = model.HearingID
            }).ToList();
            model.CaseRoleTypeList = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ERH_NewCaseRoleTypes_spResult>("qcal_AS_ERH_NewCaseRoleTypes_sp", new qcal_AS_ERH_NewCaseRoleTypes_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = model.HearingID
            }).Select(o => new SelectListItem { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            model.AssociationToChildList = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ERH_NewAssociationTypes_spResult>("qcal_AS_ERH_NewAssociationTypes_sp", new qcal_AS_ERH_NewAssociationTypes_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = model.HearingID
            }).Select(o => new SelectListItem { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();
            model.ExistingRolesList = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ERH_ExistingRoles_spResult>("qcal_AS_ERH_ExistingRoles_sp", new qcal_AS_ERH_ExistingRoles_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = model.HearingID
            }).Select(o => new SelectListItem { Text = o.FullName, Value = o.PersonID.ToString() }).ToList();
            #endregion

            model.StartDate = DateTime.Now.ToString("d");
            return View(model);

        }
        [HttpPost]
        public virtual ActionResult QuickCalCurrentERH(QuickCalCurrentERHViewModel model)
        {
            if (model.ExistingRoleID.ToInt() == 0 && (!model.LastName.IsNullOrEmpty() || !model.FirstName.IsNullOrEmpty() || model.CaseRoleTypeID.HasValue))
            {
                var insertedRoleResult = UtilityService.ExecStoredProcedureWithResults<qcal_AS_ERH_AddNewCaseRole_spResult>("qcal_AS_ERH_AddNewCaseRole_sp",
                  new qcal_AS_ERH_AddNewCaseRole_spParams()
                  {

                      CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                      CaseID = UserManager.UserExtended.CaseID,
                      LastName = model.LastName,
                      FirstName = model.FirstName,
                      HearingID = model.HearingID,
                      RoleTypeCodeID = model.CaseRoleTypeID,
                      StartDate = model.StartDate.ToDateTime(),
                      UserID = UserManager.UserExtended.UserID,
                      BatchLogJobID = Guid.NewGuid()
                  }).FirstOrDefault();
                if (insertedRoleResult != null)
                {
                    model.ExistingRoleID = insertedRoleResult.NewPersonID;
                }
            }
            if (model.ExistingRoleID.ToInt() > 0)
            {

                foreach (var item in model.ERHChildRoles)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_ERH_AddAssociation_sp",
                     new qcal_AS_ERH_AddAssociation_spParams()
                     {

                         CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                         CaseID = UserManager.UserExtended.CaseID,
                         AssociationCodeID = item.AssociationToChildID,
                         ChildPersonID = item.ChildPersonID,
                         ERHPersonID = model.ExistingRoleID.ToInt(),
                         HearingID = model.HearingID,
                         StartDate = model.StartDate.ToDateTime(),
                         UserID = UserManager.UserExtended.UserID,
                         BatchLogJobID = Guid.NewGuid()
                     });


                }
            }
            if (model.ERHHistory.Any())
            {
                foreach (var item in model.ERHHistory)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("qcal_AS_ERH_UpdateAssociation_sp",
                     new qcal_AS_ERH_UpdateAssociation_spParams()
                     {
                         UserID = UserManager.UserExtended.UserID,
                         BatchLogJobID = Guid.NewGuid(),
                         AssociationID = item.AssociationID,
                         StartDate = item.StartDate.ToDateTime(),
                         EndDate = item.EndDate.ToDateTimeNullableValue()
                     });
                }
            }
            return Json(new
            {
                Status = "Done",
                ErrorMessage = "",
                Model = model,


            });
        }
        #endregion
        #region Additional Attendees
        public virtual ActionResult AdditionalAttendees(string id)
        {
            var viewModel = new AdditionalAttendeesViewModel();
            viewModel.HearingID = id.ToDecrypt().ToInt();


            viewModel.HearingAttendance =
                   UtilityService.ExecStoredProcedureWithResults<qcal_AS_HearingAttendanceGetAdditionalAttendee_spResult>(
                       "qcal_AS_HearingAttendanceGetAdditionalAttendee_sp",
                       new qcal_AS_HearingAttendanceGet_spParams()
                       {
                           HearingID = viewModel.HearingID.Value,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()
                       }).ToList();
            return View(viewModel);

        }
        [HttpPost]
        public virtual ActionResult AdditionalAttendees(AdditionalAttendeesViewModel model)
        {
            if (model.HearingAttendance.Any())
            {
                foreach (var item in model.HearingAttendance)
                {
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = model.HearingID.Value,
                        RoleID = item.RoleID.Value,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        AttendedFlag = 1
                    }).FirstOrDefault();
                }
            }


            return Json(new
            {
                Status = "Done",
                ErrorMessage = "",
                Model = model,


            });
        }
        #endregion 

    }
}