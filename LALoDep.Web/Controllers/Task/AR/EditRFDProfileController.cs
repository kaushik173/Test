using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.PD_PDAction;
using LALoDep.Domain.pd_Profile;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Controllers.CaseOpening;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using DataTables.Mvc;
using LALoDep.Models.CaseOpening;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using LALoDep.Models;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {


        #region  AR Profile

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditRFDProfilePage)]
        public virtual ActionResult EditRFDProfile(string id, string roleId, string profileTypeId)
        {

            var viewModel = new EditRfdProfileViewModel();

            viewModel.CurrentRoleID = roleId.ToDecrypt().ToInt();
            viewModel.CurrentProfileTypeID = profileTypeId.ToDecrypt().ToInt();


            var reportFillingDueGet =
                UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>(
                    "pd_HearingReportFilingDueGet_sp", new pd_HearingReportFilingDueGet_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        HearingReportFilingDueID = id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID

                    }).FirstOrDefault();

            if (reportFillingDueGet != null)
            {

                ViewBag.Id = id;
                viewModel.HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID;
                viewModel.RfdHeader = reportFillingDueGet.RFDHeader;
                viewModel.ProfileList =
                    UtilityService.ExecStoredProcedureWithResults<pd_ProfileGetList_spResult>("pd_ProfileGetList_sp",
                        new pd_ProfileGetList_spParams
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            RFDID = reportFillingDueGet.HearingReportFilingDueID,
                            BatchLogJobID = Guid.NewGuid()
                        }).ToList();
                if (viewModel.CurrentRoleID > 0)
                {
                    var profile =
                        viewModel.ProfileList.FirstOrDefault(o => o.RoleID.Value == viewModel.CurrentRoleID && o.ProfileTypeCodeID.Value == viewModel.CurrentProfileTypeID);
                    if (profile != null)
                    {

                        viewModel.ProfileQuestionList =
                            UtilityService.ExecStoredProcedureWithResults<pd_ProfileGetCurrentByRoleIDRFD_spResult>(
                                "pd_ProfileGetCurrentByRoleIDRFD_sp", new pd_ProfileGetCurrentByRoleIDRFD_spParams
                                {
                                    RFDID = reportFillingDueGet.HearingReportFilingDueID,
                                    RoleID = profile.RoleID.Value,
                                    ProfileTypeCodeID = profile.ProfileTypeCodeID.Value,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                }).ToList();
                    }

                }

            }


            viewModel.PersonList =
                UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDAndSysValue_spResult>(
                    "pd_RoleGetByCaseIDAndSysValue_sp", new pd_RoleGetByCaseIDAndSysValue_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        SysVal = 206,
                        BatchLogJobID = Guid.NewGuid()
                    })
                    .Select(
                        o =>
                            new SelectListItem()
                            {
                                Text = o.PersonNameDisplay + "(" + o.Role + ")",
                                Value = o.RoleID.ToEncrypt()
                            })
                    .ToList();

            viewModel.ProfileTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(400, agencyId: UserManager.UserExtended.CaseNumberAgencyID,encryptedValue: true);



            return View("~/Views/Task/AR/EditRFDProfile.cshtml", viewModel);
        }




        [HttpPost]
        public virtual ActionResult PrintARProfile(string rfdId, string roleId, string profileTypeCodeId)
        {
            var comReportSourceGetByReportIdSpParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 81,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };

            #region Delete Report Parameter

            var dictionaryParam = new Dictionary<string, object>()
            {
                {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
            };

            /*Delete Existing Report Parameters saved for this User*/
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueDelete_sp", dictionaryParam);
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderDelete_sp", dictionaryParam);

            #endregion


            #region Insert Report Parameter

            /*Insert New Report Parameters saved for this User*/
            /*
             RoleID
             ProfileTypeCodeID
             RFDID*/

            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 1}
                    ,
                    {"@Value", roleId}
                });
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 2}
                    ,
                    {"@Value", profileTypeCodeId}
                });
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 3}
                    ,
                    {"@Value", rfdId}
                });

            #endregion

            var spResult =
                UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>(
                    "com_ReportSourceGetByReportID_sp", comReportSourceGetByReportIdSpParams).ToList();


            var rpt = new ReportClass
            {
                FileName = HttpContext.Server.MapPath("~/Reports/" + spResult[0].ReportSourceDocumentName)
            };
            try
            {
                rpt.Load();
                var table =
                    UtilityService.ExecStoredProcedureForDataTable(
                        spResult[0].ReportSourceStoredProcedureName.Replace(".dbo", ""),
                        comReportSourceGetByReportIdSpParams);
                rpt.SetDataSource(table);

                foreach (
                    var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != spResult[0].ReportSourceDocumentName))
                {
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(
                        UtilityService.ExecStoredProcedureForDataTable(
                            subRptDt.ReportSourceStoredProcedureName.Replace(".dbo", ""),
                            comReportSourceGetByReportIdSpParams));
                }

                var filename = spResult[0].ReportDisplayName + comReportSourceGetByReportIdSpParams.ReportID.ToEncrypt() +
                               ".pdf";

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {


                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;

                    //if file already exists then delete it
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(ExportFormatType.PortableDocFormat, filePath);


                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();


                    return RedirectToAction("Preview", "Home",
                        new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + filename) });
                }


                var stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf", filename);
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
            return Content("Report not generating");


        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditRFDProfilePage,
            PageSecurityItemID = SecurityToken.EditRFDViewProfile)]
        public virtual ActionResult ARProfileQuestionNote(string profileId)
        {


            if (!profileId.IsNullOrEmpty())
            {
                var noteEntry =
                    UtilityFunctions.NoteGetByEntity(profileId.ToDecrypt().ToInt(), 120, 123).FirstOrDefault();
                if (noteEntry != null)
                {
                    ViewBag.NoteEntry = noteEntry.NoteEntry;
                }
            }

            return View("~/Views/Task/AR/ViewNote.cshtml");
        }

        #endregion



        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditRFDProfileQuestion)]
        public virtual ActionResult EditRFDProfileQuestion(string id, string roleId, string profileTypeId, string questionId, string profileId)
        {

            var viewModel = new EditRfdProfileQuestionViewModel
            {
                ProfileTypeID = profileTypeId.ToDecrypt().ToInt(),
                ProfileID = profileId.ToDecrypt().ToInt(),
                QuestionID = questionId.ToDecrypt().ToInt(),
                RoleID = roleId.ToDecrypt().ToInt()
            };

            if (viewModel.ProfileID > 0)
            {
                var profile = UtilityService.ExecStoredProcedureWithResults<pd_ProfileGet_spResult>("pd_ProfileGet_sp", new pd_ProfileGet_spParams
                {
                    BatchLogJobID = Guid.NewGuid(),
                    ProfileID = viewModel.ProfileID,
                    UserID = UserManager.UserExtended.UserID

                }).FirstOrDefault();
                if (profile != null)
                {
                    if (profile.ProfileQuestionID.HasValue)
                        viewModel.QuestionID = profile.ProfileQuestionID.Value;

                    viewModel.FreeFormAnswer = profile.ProfileFreeformAnswer;

                    if (profile.ProfileAnswerID.HasValue)
                        viewModel.ProfileAnswerID = profile.ProfileAnswerID.Value;

                    if (profile.NoteID.HasValue)
                        viewModel.NoteID = profile.NoteID.Value;

                    viewModel.NoteEntry = profile.NoteEntry;

                }
            }

            if (viewModel.QuestionID == 0)
            {
                var getQuestionId = UtilityService.ExecStoredProcedureWithResults<pd_ProfileQuestionGetByProfileTypeCodeID_spResult>("pd_ProfileQuestionGetByProfileTypeCodeID_sp", new pd_ProfileQuestionGetByProfileTypeCodeID_spParams
                {
                    BatchLogJobID = Guid.NewGuid(),
                    ProfileTypeCodeID = viewModel.ProfileTypeID,
                    UserID = UserManager.UserExtended.UserID

                }).FirstOrDefault();
                if (getQuestionId != null)
                {
                    viewModel.QuestionID = getQuestionId.ProfileQuestionID;

                }
            }



            var reportFillingDueGet =
                UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>(
                    "pd_HearingReportFilingDueGet_sp", new pd_HearingReportFilingDueGet_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        HearingReportFilingDueID = id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID

                    }).FirstOrDefault();

            if (reportFillingDueGet != null)
            {

                ViewBag.Id = id;
                viewModel.HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID;
                viewModel.RfdHeader = reportFillingDueGet.RFDHeader;


            }
            //Profile History
            viewModel.ProfileAnswerHistoryList =
                UtilityService.ExecStoredProcedureWithResults<pd_ProfileQuestionGetAllByQuestionIDRoleID_spResult>("pd_ProfileQuestionGetAllByQuestionIDRoleID_sp",
                    new pd_ProfileQuestionGetAllByQuestionIDRoleID_spParams
                    {
                        RoleID = viewModel.RoleID,
                        ProfileQuestionID = viewModel.QuestionID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).ToList();

            //Get First Row for populate Question Form Area
            if (viewModel.ProfileAnswerHistoryList.Count > 0)
            {
                var questionEntry = viewModel.ProfileAnswerHistoryList.FirstOrDefault();
                if (questionEntry != null)
                {

                    viewModel.Question = questionEntry.ProfileQuestionEntry;
                    viewModel.QuestionHeader = "Profile Question for " + questionEntry.PersonNameFirst + " " +
                                               questionEntry.PersonNameLast;
                    if (questionEntry.FreeformQuestion == 0)
                    {
                        viewModel.IsFreeFormAnswer = false;
                        // Populate Answer DropDownload For Choiceable Answer DropDownlist
                        viewModel.ProfileAnswerList = UtilityService.ExecStoredProcedureWithResults<pd_ProfileAnswerGetByProfileQuestionID_spResult>("pd_ProfileAnswerGetByProfileQuestionID_sp", new pd_ProfileAnswerGetByProfileQuestionID_spParams { ProfileQuestionID = viewModel.QuestionID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).Select(o => new SelectListItem() { Text = o.ProfileAnswerEntry, Value = o.ProfileAnswerID.ToString() }).ToList();
                    }
                    else
                    {
                        viewModel.IsFreeFormAnswer = true;
                    }
                }
            }

            // Popoulate Person Role List to copy their answer.
            if (viewModel.ProfileID == 0)
            {

                viewModel.PersonRoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDAndSysValue_spResult>("pd_RoleGetByCaseIDAndSysValue_sp",
                     new pd_RoleGetByCaseIDAndSysValue_spParams
                     {
                         SysVal = 206,
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,
                         BatchLogJobID = Guid.NewGuid()
                     }).ToList();
            }

            //Next Question Redirection Or Otherwise back to Profile Page
            var nextQuestion = UtilityService.ExecStoredProcedureWithResults<pd_ProfileGetNextProfileQuestionIDRFD_spResult>("pd_ProfileGetNextProfileQuestionIDRFD_sp",
                 new pd_ProfileGetNextProfileQuestionIDRFD_spParams
                 {
                     RoleID = viewModel.RoleID,
                     ProfileQuestionID = viewModel.QuestionID,
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = Guid.NewGuid()
                 }).FirstOrDefault();
            viewModel.NextQuestionUrl = string.Format("/Task/EditRfdProfile/{0}?roleId={1}&profileTypeId={2}", viewModel.HearingReportFilingDueID.ToEncrypt(), viewModel.RoleID.ToEncrypt(), viewModel.ProfileTypeID.ToEncrypt());
            if (nextQuestion != null)
            {
                viewModel.NextQuestionUrl =
       string.Format("/Task/EditRFDProfileQuestion/{0}?questionId={1}&roleId={2}&profileTypeId={3}",
           viewModel.HearingReportFilingDueID.ToEncrypt(), nextQuestion.QuestionID.ToEncrypt(), viewModel.RoleID.ToEncrypt(),
           viewModel.ProfileTypeID.ToEncrypt());



            }
            viewModel.ControlType = UtilityFunctions.GetNoteControlType("Task/EditRFDProfileQuestion",noteId: viewModel.NoteID);

            return View("~/Views/Task/AR/EditRFDProfileQuestion.cshtml", viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditRFDProfileQuestion)]
        public virtual ActionResult SaveRFDProfileQuestion(EditRfdProfileQuestionViewModel model)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.EditRFDAddProfile))
            {
                return Json(new { Status = "PermissionIssue", URL = "/Home/AccessDenied" });
            }
            if ((model.IsFreeFormAnswer && !model.FreeFormAnswer.IsNullOrEmpty()) || (!model.IsFreeFormAnswer && model.ProfileAnswerID > 0) || (!model.NoteEntry.IsNullOrEmpty() || model.NoteID > 0))
            {

                if (model.ProfileID == 0)
                {
                    var insertedId = UtilityService.ExecStoredProcedureScalar("pd_ProfileInsert_sp",
                               new pd_ProfileInsert_spParams
                               {
                                   RoleID = model.RoleID,
                                   ProfileQuestionID = model.QuestionID,
                                   AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                   HearingReportFilingDueID = model.HearingReportFilingDueID,
                                   ProfileAnswerID = model.ProfileAnswerID,
                                   ProfileFreeformAnswer = model.FreeFormAnswer,
                                   ProfileDate = DateTime.Now,
                                   RecordStateID = 1,
                                   UserID = UserManager.UserExtended.UserID,
                                   BatchLogJobID = Guid.NewGuid()

                               }).ToInt();

                    if (!model.NoteEntry.IsNullOrEmpty())
                    {
                        UtilityFunctions.NoteInsert(120, 123, insertedId, 0, "", model.NoteEntry);
                    }
                    //Check Names Below To Copy This Profile Answer To:
                    if (!model.SelectedRoleIdsForCopyAnswer.IsNullOrEmpty())
                    {
                        foreach (var roleId in model.SelectedRoleIdsForCopyAnswer.Split(','))
                        {
                            insertedId = UtilityService.ExecStoredProcedureScalar("pd_ProfileInsert_sp",
                             new pd_ProfileInsert_spParams
                             {
                                 RoleID = roleId.ToInt(),
                                 ProfileQuestionID = model.QuestionID,
                                 AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                 HearingReportFilingDueID = model.HearingReportFilingDueID,
                                 ProfileAnswerID = model.ProfileAnswerID,
                                 ProfileFreeformAnswer = model.FreeFormAnswer,
                                 ProfileDate = DateTime.Now,
                                 RecordStateID = 1,
                                 UserID = UserManager.UserExtended.UserID,
                                 BatchLogJobID = Guid.NewGuid()

                             }).ToInt();

                            if (!model.NoteEntry.IsNullOrEmpty())
                            {
                                UtilityFunctions.NoteInsert(120, 123, insertedId, 0, "", model.NoteEntry);
                            }
                        }
                    }

                }

            }







            return Json(new { Status = "Done" });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditRFDProfileQuestion)]
        public virtual ActionResult UpdateRFDProfileQuestion(EditRfdProfileQuestionViewModel model)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.EditRFDEditProfile))
            {
                return Json(new { Status = "PermissionIssue", URL = "/Home/AccessDenied" });
            }



            if (model.ProfileID > 0)
            {
                if ((Request.Form["FreeFormAnswer_oldValue"] != null && model.FreeFormAnswer != Request.Form["FreeFormAnswer_oldValue"]) || (Request.Form["ProfileAnswerID_oldValue"] != null && model.ProfileAnswerID.ToString() != Request.Form["ProfileAnswerID_oldValue"]))
                {


                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_ProfileUpdate_sp",
                               new pd_ProfileInsert_spParams
                               {
                                   ProfileID = model.ProfileID,
                                   RoleID = model.RoleID,
                                   ProfileQuestionID = model.QuestionID,
                                   AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                   HearingReportFilingDueID = model.HearingReportFilingDueID,
                                   ProfileAnswerID = model.ProfileAnswerID,
                                   ProfileFreeformAnswer = model.FreeFormAnswer,
                                   ProfileDate = DateTime.Now,
                                   RecordStateID = 1,
                                   UserID = UserManager.UserExtended.UserID,
                                   BatchLogJobID = Guid.NewGuid()

                               });
                }

                if (!model.NoteEntry.IsNullOrEmpty() && model.NoteEntry != Request.Form["NoteEntry_oldValue"])
                {
                    UtilityFunctions.NoteUpdate(model.NoteID, 3286, 3288, model.ProfileID, 0, "", model.NoteEntry);
                }

            }

            var nextQuestionUrl =
       string.Format("/Task/EditRFDProfileQuestion/{0}?questionId={1}&roleId={2}&profileTypeId={3}",
           model.HearingReportFilingDueID.ToEncrypt(), model.QuestionID.ToEncrypt(), model.RoleID.ToEncrypt(),
           model.ProfileTypeID.ToEncrypt());


            return Json(new { Status = "Done", URL = nextQuestionUrl });
        }
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditRFDProfileQuestion)]
        public virtual ActionResult DeleteRFDProfile(int id)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.EditRFDDeleteProfile))
            {
                return Json(new { Status = "PermissionIssue", URL = "/Home/AccessDenied" });
            }


            UtilityService.ExecStoredProcedureWithoutResultADO("pd_ProfileDelete_sp",
                       new pd_ProfileDelete_spParams
                       {
                           ID = id,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid()

                       });




            return Json(new { Status = "Done" });
        }

    }
}