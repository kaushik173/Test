using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Note;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {


        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.EditARPage, PageSecurityItemID = SecurityToken.EditActionRequest)]
        public virtual ActionResult EditRFD(string id, string caseId)
        {
            ViewBag.Id = id;//for populate tabs
            if (!string.IsNullOrEmpty(caseId))
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());
            var model = new EditRfdViewModel();

            model.ControlType = UtilityFunctions.GetNoteControlType("Task/EditRFD");
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


                model.HearingReportFilingDueID = id.ToDecrypt().ToInt();
                if (reportFillingDueGet.HearingReportFilingDueDate != null)
                    model.DueDate = reportFillingDueGet.HearingReportFilingDueDate.Value.ToString("d");
                if (reportFillingDueGet.HearingReportFilingDueOrderDate != null)
                    model.RequestDate = reportFillingDueGet.HearingReportFilingDueOrderDate.Value.ToString("d");
                if (reportFillingDueGet.HearingDateTime != null)
                    model.Hearing = reportFillingDueGet.HearingDateTime.Value.ToString() + " ";

                if (reportFillingDueGet.HearingReportFilingDueEndDate.HasValue)
                {
                    model.CompletedDate = reportFillingDueGet.HearingReportFilingDueEndDate.Value.ToString("d");
                    model.Completed = true;
                }


                model.Hearing += reportFillingDueGet.HearingType;
                model.RequestType = reportFillingDueGet.HearingReportFilingDueTypeCodeValue;
                model.LegalResearchType = reportFillingDueGet.LegalResearchType;
                model.RequestBy = reportFillingDueGet.RequestBy;
                model.RequestForID = reportFillingDueGet.RequestedForPersonID.HasValue
                    ? reportFillingDueGet.RequestedForPersonID.Value
                    : 0;



                //var requestNote =
                //    UtilityFunctions.NoteGetByRFDIDSystemValueTypeID(107, reportFillingDueGet.HearingReportFilingDueID)
                //        .FirstOrDefault(o => o.CodeID == 2449);
                //if (requestNote != null)
                //{
                //    model.RequestNote = requestNote.NoteEntry;
                //}
                model.NoteBoxList = UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_NoteGetByRFDIDSystemValueTypeID_spResult>("pd_NoteGetByRFDIDSystemValueTypeID_sp",
                    new pd_NoteGetByRFDIDSystemValueTypeID_spParams()
                    {

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        RFDID = model.HearingReportFilingDueID,
                        SystemValueTypeID = 107

                    }).ToList();
                var otherNotes = UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_NoteGetByRFDIDSystemValueTypeID_spResult>("pd_NoteGetByRFDIDSystemValueTypeID_sp",
                      new pd_NoteGetByRFDIDSystemValueTypeID_spParams()
                      {

                          UserID = UserManager.UserExtended.UserID,
                          BatchLogJobID = Guid.NewGuid(),
                          RFDID = model.HearingReportFilingDueID,
                          SystemValueTypeID = 108

                      }).ToList();
                foreach (var note in otherNotes)
                    model.NoteBoxList.Add(note);
                /*
                var otherNotes =
                    UtilityFunctions.NoteGetByRFDIDSystemValueTypeID(108, reportFillingDueGet.HearingReportFilingDueID)
                        .ToList();
                foreach (var item in otherNotes)
                {
                    switch (item.CodeID)
                    {
                        case 2888:
                            model.InvestigatorEvaluationNote = item.NoteEntry;
                            model.InvestigatorEvaluationNoteID = item.NoteID;
                            model.InvestigatorEvaluationNoteControlType = UtilityFunctions.GetNoteControlType("Task/EditRFD", noteId: item.NoteID);


                            break;
                        case 2889:
                            model.CaretakerEvaluationNote = item.NoteEntry;
                            model.CaretakerEvaluationNoteID = item.NoteID;
                            model.CaretakerEvaluationNoteControlType = UtilityFunctions.GetNoteControlType("Task/EditRFD", noteId: item.NoteID);

                            break;
                        case 2890:
                            model.SocialWorkerEvaluationNote = item.NoteEntry;
                            model.SocialWorkerEvaluationNoteID = item.NoteID;
                            model.SocialWorkerEvaluationNoteControlType = UtilityFunctions.GetNoteControlType("Task/EditRFD", noteId: item.NoteID);

                            break;
                        case 2891:
                            model.TherapistEvaluationNote = item.NoteEntry;
                            model.TherapistEvaluationNoteID = item.NoteID;
                            model.TherapistEvaluationNoteControlType = UtilityFunctions.GetNoteControlType("Task/EditRFD", noteId: item.NoteID);
                            break;
                        case 2892:
                            model.ProbationOfficerEvaluationNote = item.NoteEntry;
                            model.ProbationOfficerEvaluationNoteID = item.NoteID;
                            model.ProbationOfficerEvaluationNoteControlType = UtilityFunctions.GetNoteControlType("Task/EditRFD", noteId: item.NoteID);
                            break;
                    }
                }


    */

                model.RequestForList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetARRequestFor_spResult>(
                "pd_RoleGetARRequestFor_sp",
                new pd_RoleGetARRequestFor_spParams()
                {
                    CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    IncludePersonID =
                        reportFillingDueGet.RequestedForPersonID.HasValue
                            ? reportFillingDueGet.RequestedForPersonID.Value
                            : 0
                            ,
                    HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID
                }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.PersonID.ToString() }).ToList();

                model.ClientRoleList = UtilityService.ExecStoredProcedureWithResults<pd_RFDRoleContactGetByReportFilingDueID_spResult>(
                "pd_RFDRoleContactGetByReportFilingDueID_sp",
                new pd_RFDRoleContactGetByReportFilingDueID_spParams()
                {
                    ReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),


                }).ToList();

                if (Request.QueryString["dataentry"] != null)
                {
                    Session[id + "_ARExitUrl"] = "/CaseOpening/ActionRequest?dataentry=true";

                }
                else if (Request.QueryString["page"] != null)
                {
                    Session[id + "_ARExitUrl"] = Request.QueryString["page"];

                }
                else
                {
                    Session[id + "_ARExitUrl"] = "/Task/MyARQueue/" + model.RequestForID.ToEncrypt();

                }

            }

            return View("~/Views/Task/AR/EditRFD.cshtml", model);
        }



        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditARPage,
            PageSecurityItemID = SecurityToken.EditActionRequest)]
        public virtual JsonResult EditRFD(EditRfdViewModel model, EditRfdViewModel oldModel)
        {
            if (!model.ForceCreateARAnyway)
            {
                var arAssignmentCheckResult = UtilityService.ExecStoredProcedureWithResults<sup_ARAssignmentConflicts_spResult>(
                             "sup_ARAssignmentConflicts_sp",
                             new sup_ARAssignmentConflicts_spParams()
                             {
                                 CaseID = UserManager.UserExtended.CaseID,
                                 PersonID = model.RequestForID,
                                 UserID = UserManager.UserExtended.UserID,
                             }).FirstOrDefault();
                if (arAssignmentCheckResult != null)
                {
                    return Json(new { Status = "AssignmentFail", ErrorMessage = arAssignmentCheckResult.Comment, DialogType = arAssignmentCheckResult.DialogType });
                }
            }
            var reportFillingDueGet =
                UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>(
                    "pd_HearingReportFilingDueGet_sp", new pd_HearingReportFilingDueGet_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        HearingReportFilingDueID = model.HearingReportFilingDueID,
                        UserID = UserManager.UserExtended.UserID

                    }).FirstOrDefault();
            if (reportFillingDueGet != null)
            {
                if ((reportFillingDueGet.RequestedForPersonID.HasValue &&
                     reportFillingDueGet.RequestedForPersonID.Value != model.RequestForID) ||
                    (reportFillingDueGet.HearingReportFilingDueDate.HasValue &&
                     reportFillingDueGet.HearingReportFilingDueDate.Value.ToString("d") != model.DueDate) ||
                    (!reportFillingDueGet.HearingReportFilingDueEndDate.HasValue && model.Completed) || (reportFillingDueGet.HearingReportFilingDueEndDate.HasValue && reportFillingDueGet.HearingReportFilingDueEndDate.Value.ToString("d") != model.CompletedDate))
                {
                    //     model.CompletedDate = model.CompletedDate ?? DateTime.Now.ToString("d");
                    DateTime? completedDate = null;
                    if (!model.CompletedDate.IsNullOrEmpty())
                    {
                        completedDate = model.CompletedDate.ToDateTime();
                    }
                    else if (model.CompletedDate.IsNullOrEmpty() && model.Completed)
                    {
                        completedDate = DateTime.Today;
                    }

                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingReportFilingDueUpdate_sp",
                      new pd_HearingReportFilingDueUpdate_spParams()
                      {
                          AgencyID = reportFillingDueGet.AgencyID,
                          UserID = UserManager.UserExtended.UserID,
                          RecordStateID = 1,
                          HearingID = reportFillingDueGet.HearingID,
                          HearingReportFilingDueDate = model.DueDate.ToDateTime(),
                          BatchLogJobID = Guid.NewGuid(),
                          //HearingReportFilingDueEndDate = model.Completed ? completedDate: reportFillingDueGet.HearingReportFilingDueEndDate,
                          HearingReportFilingDueEndDate = completedDate,

                          HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID,
                          HearingReportFilingDueLegalResearchTypeCodeID =
                              reportFillingDueGet.HearingReportFilingDueLegalResearchTypeCodeID,
                          HearingReportFilingDueOrderDate = reportFillingDueGet.HearingReportFilingDueOrderDate,
                          HearingReportFilingDueTypeCodeID = reportFillingDueGet.HearingReportFilingDueTypeCodeID,
                          RequestedByPersonID = reportFillingDueGet.RequestedByPersonID,
                          RequestedForPersonID = model.RequestForID

                      });

                }
                if (model.RoleCount > 0)
                {
                    for (var i = 0; i < model.RoleCount; i++)
                    {
                        var contactDate = Request.Form["ContactDate" + i] ?? string.Empty;
                        var oldContactDate = Request.Form["ContactDate" + i + "_oldValue"] ?? string.Empty;

                        //if (Request.Form["ContactDate" + i] != null && Request.Form["ContactDate" + i].Length > 5 && Request.Form["ContactDate" + i] != Request.Form["ContactDate" + i + "_oldValue"])
                        if (contactDate != oldContactDate)
                        {
                            UtilityService.ExecStoredProcedureWithoutResultADO("pd_RFDRoleContactUpdate_sp",
                                new pd_RFDRoleContactInsert_spParams()
                                {
                                    RFDRoleContactDate = Request.Form["ContactDate" + i].ToDateTimeValue(),

                                    BatchLogJobID = Guid.NewGuid(),

                                    UserID = UserManager.UserExtended.UserID,
                                    RecordStateID = 1,
                                    RFDRoleID = Request.Form["RFDRoleID" + i].ToInt(),
                                    RFDRoleContactTypeCodeID = Request.Form["ContactTypeID" + i].ToInt(),
                                    RFDRoleContactID = Request.Form["ContactID" + i].ToInt()

                                });
                        }
                    }
                }

                if (model.NoteBoxList.Any())
                {
                    foreach (var note in model.NoteBoxList)
                    {
                        if (!note.NoteID.HasValue)
                        {
                            if (!note.NoteEntry.IsNullOrEmpty())
                                UtilityFunctions.NoteInsert(117, 123, reportFillingDueGet.HearingReportFilingDueID, note.CodeID.ToInt(), "",
                                note.NoteEntry);
                        }
                        else
                        {
                            if (!note.NoteEntry.IsNullOrEmpty())
                                UtilityFunctions.NoteUpdate(note.NoteID.Value, note.NoteEntityCodeID.ToInt(), note.NoteEntityTypeCodeID.ToInt(), note.EntityPrimaryKeyID.ToInt(), note.NoteTypeCodeID.ToInt(), note.NoteSubject, note.NoteEntry);
                            else
                                UtilityFunctions.NoteDelete(note.NoteID.Value);

                        }
                    }
                }
                /*
                                if (!model.InvestigatorEvaluationNote.IsNullOrEmpty() && model.InvestigatorEvaluationNote != Request.Form["InvestigatorEvaluationNote_oldValue"])
                                {
                                    if (!model.InvestigatorEvaluationNoteID.HasValue)
                                    {
                                        UtilityFunctions.NoteInsert(117, 123, reportFillingDueGet.HearingReportFilingDueID, 2888, "",
                                            model.InvestigatorEvaluationNote);
                                    }
                                    else
                                    {
                                        UtilityFunctions.NoteUpdate(model.InvestigatorEvaluationNoteID.Value, 3283, 3288,
                                            reportFillingDueGet.HearingReportFilingDueID, 2888, "",
                                            model.InvestigatorEvaluationNote);

                                    }


                                }
                                else if (model.InvestigatorEvaluationNote.IsNullOrEmpty() && model.InvestigatorEvaluationNoteID.HasValue)
                                {
                                    UtilityFunctions.NoteDelete(model.InvestigatorEvaluationNoteID.Value);
                                }

                                if (!model.CaretakerEvaluationNote.IsNullOrEmpty() && model.CaretakerEvaluationNote != Request.Form["CaretakerEvaluationNote_oldValue"])
                                {
                                    if (!model.CaretakerEvaluationNoteID.HasValue)
                                    {
                                        UtilityFunctions.NoteInsert(117, 123, reportFillingDueGet.HearingReportFilingDueID, 2889, "",
                                            model.CaretakerEvaluationNote);
                                    }
                                    else
                                    {
                                        UtilityFunctions.NoteUpdate(model.CaretakerEvaluationNoteID.Value, 3283, 3288,
                                            reportFillingDueGet.HearingReportFilingDueID, 2889, "",
                                            model.CaretakerEvaluationNote);

                                    }


                                }
                                else if (model.CaretakerEvaluationNote.IsNullOrEmpty() && model.CaretakerEvaluationNoteID.HasValue)
                                {
                                    UtilityFunctions.NoteDelete(model.CaretakerEvaluationNoteID.Value);
                                }
                                if (!model.SocialWorkerEvaluationNote.IsNullOrEmpty() && model.SocialWorkerEvaluationNote != Request.Form["SocialWorkerEvaluationNote_oldValue"])
                                {
                                    if (!model.SocialWorkerEvaluationNoteID.HasValue)
                                    {
                                        UtilityFunctions.NoteInsert(117, 123, reportFillingDueGet.HearingReportFilingDueID, 2890, "",
                                            model.SocialWorkerEvaluationNote);
                                    }
                                    else
                                    {
                                        UtilityFunctions.NoteUpdate(model.SocialWorkerEvaluationNoteID.Value, 3283, 3288,
                                            reportFillingDueGet.HearingReportFilingDueID, 2890, "",
                                            model.SocialWorkerEvaluationNote);

                                    }


                                }
                                else if (model.SocialWorkerEvaluationNote.IsNullOrEmpty() && model.SocialWorkerEvaluationNoteID.HasValue)
                                {
                                    UtilityFunctions.NoteDelete(model.SocialWorkerEvaluationNoteID.Value);
                                }
                                if (!model.TherapistEvaluationNote.IsNullOrEmpty() && model.TherapistEvaluationNote != Request.Form["TherapistEvaluationNote_oldValue"])
                                {
                                    if (!model.TherapistEvaluationNoteID.HasValue)
                                    {
                                        UtilityFunctions.NoteInsert(117, 123, reportFillingDueGet.HearingReportFilingDueID, 2891, "",
                                            model.TherapistEvaluationNote);
                                    }
                                    else
                                    {
                                        UtilityFunctions.NoteUpdate(model.TherapistEvaluationNoteID.Value, 3283, 3288,
                                            reportFillingDueGet.HearingReportFilingDueID, 2891, "",
                                            model.TherapistEvaluationNote);

                                    }


                                }
                                else if (model.TherapistEvaluationNote.IsNullOrEmpty() && model.TherapistEvaluationNoteID.HasValue)
                                {
                                    UtilityFunctions.NoteDelete(model.TherapistEvaluationNoteID.Value);
                                }
                                if (!model.ProbationOfficerEvaluationNote.IsNullOrEmpty() && model.ProbationOfficerEvaluationNote != Request.Form["ProbationOfficerEvaluationNote_oldValue"])
                                {
                                    if (!model.ProbationOfficerEvaluationNoteID.HasValue)
                                    {
                                        UtilityFunctions.NoteInsert(117, 123, reportFillingDueGet.HearingReportFilingDueID, 2892, "",
                                            model.ProbationOfficerEvaluationNote);
                                    }
                                    else
                                    {
                                        UtilityFunctions.NoteUpdate(model.ProbationOfficerEvaluationNoteID.Value, 3283, 3288,
                                            reportFillingDueGet.HearingReportFilingDueID, 2892, "",
                                            model.ProbationOfficerEvaluationNote);

                                    }
                                }
                                else if (model.ProbationOfficerEvaluationNote.IsNullOrEmpty() && model.TherapistEvaluationNoteID.HasValue)
                                {
                                    UtilityFunctions.NoteDelete(model.ProbationOfficerEvaluationNoteID.Value);
                                }*/
            }
            if (Request.Form["Print"] != null)
            {

            }
            return Json(new { Status = "Done", HearingReportFilingDueID = model.HearingReportFilingDueID });
        }



    }
}