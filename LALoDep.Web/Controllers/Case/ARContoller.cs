using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Work;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using LALoDep.Domain.pd_Role;
using DataTables.Mvc;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Code;
using LALoDep.Core.Enums;
using LALoDep.Models;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Models.CaseOpening;
using LALoDep.Domain.pd_QuickRFD;
using LALoDep.Domain.pd_Address;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        #region Edit AR

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditARPage,
            PageSecurityItemID = SecurityToken.EditActionRequest)]
        public virtual ActionResult EditAR(string id)
        {
            var model = new ActionRequestEditModel();
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
                model.HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID;
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
                else
                {
                    model.CompletedDate = DateTime.Now.ToString("d");
                }

                model.Hearing += reportFillingDueGet.HearingType;
                model.RequestType = reportFillingDueGet.HearingReportFilingDueTypeCodeValue;
                model.LegalResearchType = reportFillingDueGet.LegalResearchType;
                model.RequestBy = reportFillingDueGet.RequestBy;
                model.RequestForID = reportFillingDueGet.RequestedForPersonID.HasValue
                    ? reportFillingDueGet.RequestedForPersonID.Value
                    : 0;
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

                    }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.PersonID.ToString() }).ToList();
                model.ClientRoleList =
                    UtilityService.ExecStoredProcedureWithResults<pd_RFDRoleContactGetByReportFilingDueID_spResult>(
                        "pd_RFDRoleContactGetByReportFilingDueID_sp",
                        new pd_RFDRoleContactGetByReportFilingDueID_spParams()
                        {
                            ReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),


                        }).ToList();

                var requestNote =
                    UtilityFunctions.NoteGetByRFDIDSystemValueTypeID(107, reportFillingDueGet.HearingReportFilingDueID)
                        .FirstOrDefault(o => o.CodeID == 2449);
                if (requestNote != null)
                {
                    model.RequestNote = requestNote.NoteEntry;
                }


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
                            break;
                        case 2889:
                            model.CaretakerEvaluationNote = item.NoteEntry;
                            model.CaretakerEvaluationNoteID = item.NoteID;
                            break;
                        case 2890:
                            model.SocialWorkerEvaluationNote = item.NoteEntry;
                            model.SocialWorkerEvaluationNoteID = item.NoteID;
                            break;
                        case 2891:
                            model.TherapistEvaluationNote = item.NoteEntry;
                            model.TherapistEvaluationNoteID = item.NoteID;
                            break;
                        case 2892:
                            model.ProbationOfficerEvaluationNote = item.NoteEntry;
                            model.ProbationOfficerEvaluationNoteID = item.NoteID;
                            break;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditARPage,
            PageSecurityItemID = SecurityToken.EditActionRequest)]
        public virtual JsonResult EditAR(ActionRequestEditModel model)
        {
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
                DateTime? completedDate = null;
                if (!model.CompletedDate.IsNullOrEmpty())
                {
                    completedDate = model.CompletedDate.ToDateTime();
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
                        HearingReportFilingDueEndDate =
                            model.Completed
                                ? completedDate
                                : reportFillingDueGet.HearingReportFilingDueEndDate,
                        HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID,
                        HearingReportFilingDueLegalResearchTypeCodeID =
                            reportFillingDueGet.HearingReportFilingDueLegalResearchTypeCodeID,
                        HearingReportFilingDueOrderDate = reportFillingDueGet.HearingReportFilingDueOrderDate,
                        HearingReportFilingDueTypeCodeID = reportFillingDueGet.HearingReportFilingDueTypeCodeID,
                        RequestedByPersonID = reportFillingDueGet.RequestedByPersonID,
                        RequestedForPersonID = model.RequestForID

                    });
                if (model.RoleCount > 0)
                {
                    for (var i = 0; i < model.RoleCount; i++)
                    {
                        if (Request.Form["ContactDate" + i] != null && Request.Form["ContactDate" + i].Length > 5)
                        {
                            UtilityService.ExecStoredProcedureWithoutResults("pd_RFDRoleContactUpdate_sp",
                                new pd_RFDRoleContactInsert_spParams()
                                {
                                    RFDRoleContactDate = Request.Form["ContactDate" + i].ToDateTime(),

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





                if (!model.InvestigatorEvaluationNote.IsNullOrEmpty())
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

                if (!model.CaretakerEvaluationNote.IsNullOrEmpty())
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
                if (!model.SocialWorkerEvaluationNote.IsNullOrEmpty())
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
                if (!model.TherapistEvaluationNote.IsNullOrEmpty())
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
                if (!model.ProbationOfficerEvaluationNote.IsNullOrEmpty())
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
            }
            if (Request.Form["Print"] != null)
            {

            }
            return Json(new { Status = "Done", HearingReportFilingDueID = model.HearingReportFilingDueID });
        }

        #endregion

        #region Quick AR

        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.QuickARPage, PageSecurityItemID = SecurityToken.QuickAR)]
        public virtual ActionResult QuickAR(string id, string caseId)
        {
            if (!string.IsNullOrEmpty(caseId))
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());

            var model = new QuickARModel();
            model.ControlType = UtilityFunctions.GetNoteControlType("Case/QuickAR");
            var reportFillingDueGet = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>("pd_HearingReportFilingDueGet_sp",
                                                new pd_HearingReportFilingDueGet_spParams
                                                {
                                                    HearingReportFilingDueID = id.ToDecrypt().ToInt(),
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).FirstOrDefault();

            model.HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID;

            if (reportFillingDueGet.HearingReportFilingDueOrderDate != null)
                model.RequestDate = reportFillingDueGet.HearingReportFilingDueOrderDate.Value.ToString("d");

            model.RequestTypeID = reportFillingDueGet.HearingReportFilingDueTypeCodeID;
            model.RequestType = reportFillingDueGet.HearingReportFilingDueTypeCodeValue;

            model.HearingID = reportFillingDueGet.HearingID;
            if (reportFillingDueGet.HearingDateTime != null)
                model.Hearing = reportFillingDueGet.HearingDateTime.Value.ToString() + " ";
            model.Hearing += reportFillingDueGet.HearingType;

            model.RequestByID = reportFillingDueGet.RequestedByPersonID;
            model.RequestBy = reportFillingDueGet.RequestBy;
            model.RequestForID = reportFillingDueGet.RequestedForPersonID;
            model.RequestFor = reportFillingDueGet.RequestFor;

            if (reportFillingDueGet.HearingReportFilingDueDate.HasValue)
                model.DueDate = reportFillingDueGet.HearingReportFilingDueDate.Value.ToString("d");

            if (reportFillingDueGet.HearingReportFilingDueEndDate.HasValue)
            {
                model.CompletedDate = reportFillingDueGet.HearingReportFilingDueEndDate.Value.ToString("d");
                model.Completed = true;
            }
            else
            {
              //  model.CompletedDate = DateTime.Now.ToString("d");
            }

            model.AssociationTypeList = UtilityService.ExecStoredProcedureWithResults<pd_QuickRFDAssociationTypeGet_spResult>("pd_QuickRFDAssociationTypeGet_sp",
                                                new pd_QuickRFDAssociationTypeGet_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    ReportFilingDueID = model.HearingReportFilingDueID,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).Select(x => new SelectListItem { Text = x.AssociationTypeDisplay, Value = x.AssociationTypeCodeID.ToString() });

            model.StateList = UtilityFunctions.CodeGetByTypeIdAndUserId(5, includeShortCodeValue: true);
            model.CountryList = UtilityFunctions.CodeGetByTypeIdAndUserId(27);
            model.CountryID = 2246; //Default US

            model.PlacementAgencyList = UtilityService.ExecStoredProcedureWithResults<pd_AddressGetPlacementCodes_spResult>("pd_AddressGetPlacementCodes_sp",
                                                new pd_AddressGetPlacementCodes_spParams { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                                .Select(x => new SelectListItem { Value = x.AddressID.ToString(), Text = x.PlacementAgency + " - " + x.AddressStreet + ", " + x.AddressCity + " " + x.AddressZipCode });


            model.ClientList = UtilityService.ExecStoredProcedureWithResults<pd_QuickRFDClientGet_spResult>("pd_QuickRFDClientGet_sp",
                                                new pd_QuickRFDClientGet_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    ReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                });

            model.RecordTimeTypeList = UtilityFunctions.CodeGetWorkDescription(agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.PhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, userShortValue: true);

            var investigatorNote = UtilityFunctions.NoteGetByRFDIDSystemValueTypeID(140, reportFillingDueGet.HearingReportFilingDueID).FirstOrDefault();
            if (investigatorNote != null)
            {
                model.InvestigatorEvaluationNoteID = investigatorNote.NoteID;
                model.InvestigatorEvaluationNote = investigatorNote.NoteEntry;
                model.InvestigatorEvaluationNoteControlType = UtilityFunctions.GetNoteControlType("Case/QuickAR", noteId: investigatorNote.NoteID);
            }

            var requestNote = UtilityFunctions.NoteGetByRFDIDSystemValueTypeID(107, reportFillingDueGet.HearingReportFilingDueID).FirstOrDefault(o => o.CodeID == 2449);
            if (requestNote != null)
            {
                model.RequestNote = requestNote.NoteEntry;
            }
            model.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            return View(model);
        }


        [HttpPost]
        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.QuickARPage,
            PageSecurityItemID = SecurityToken.QuickAR)]
        public virtual ActionResult QuickAR(QuickARModel model)
        {

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
                if (Request.Form["CompletedDate_oldValue"] != model.CompletedDate || (!reportFillingDueGet.HearingReportFilingDueEndDate.HasValue && model.Completed))
                {
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
                            HearingReportFilingDueDate = reportFillingDueGet.HearingReportFilingDueDate,
                            BatchLogJobID = Guid.NewGuid(),
                            HearingReportFilingDueEndDate =
                               completedDate,
                            HearingReportFilingDueID = reportFillingDueGet.HearingReportFilingDueID,
                            HearingReportFilingDueLegalResearchTypeCodeID =
                                reportFillingDueGet.HearingReportFilingDueLegalResearchTypeCodeID,
                            HearingReportFilingDueOrderDate = reportFillingDueGet.HearingReportFilingDueOrderDate,
                            HearingReportFilingDueTypeCodeID = reportFillingDueGet.HearingReportFilingDueTypeCodeID,
                            RequestedByPersonID = reportFillingDueGet.RequestedByPersonID,
                            RequestedForPersonID = reportFillingDueGet.RequestedForPersonID

                        });
                }

                if (!model.InvestigatorEvaluationNote.IsNullOrEmpty() && Request.Form["InvestigatorEvaluationNote_oldValue"] != model.InvestigatorEvaluationNote)
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
                else if (model.InvestigatorEvaluationNoteID.HasValue)
                {
                    UtilityFunctions.NoteDelete(model.InvestigatorEvaluationNoteID.Value);

                }


                var clientRoleIds = "";
                var nonClientRoleIds = "";
                var workId = 0.00;
                var clientCOunt = Request.Form["ClientCount"].ToInt();
                for (var i = 0; i < clientCOunt; i++)
                {
                    var clientRoleSelected = Request.Form["chk300_" + i];
                    if (clientRoleSelected == "true")
                    {
                        if (nonClientRoleIds != "")
                            nonClientRoleIds += ",";
                        nonClientRoleIds += Request.Form["hidClientRoleID_" + i];
                    }

                    if (Request.Form["chkDual_" + i].ToBoolean())
                    {
                        if (clientRoleIds != "")
                            clientRoleIds += ",";
                        clientRoleIds += Request.Form["hidClientRoleID_" + i];
                    }



                    if (model.RecordTimeTypeID > 0 && Request.Form["chkRT_" + i].ToBoolean())
                    {
                        var hidClientPersonId = Request.Form["hidClientPersonID_" + i].ToInt();
                        var hidClientRoleId = Request.Form["hidClientRoleID_" + i].ToInt();
                        if (workId <= 0)
                        {
                            workId = (double)UtilityService.ExecStoredProcedureWithResults<decimal>("pd_WorkInsert1_sp", new pd_WorkInsert1_spParams()
                            {
                                CaseID = UserManager.UserExtended.CaseID,
                                PersonID = reportFillingDueGet.RequestedForPersonID.Value,
                                WorkHours = model.Hours,
                                WorkMileage = 0,
                                WorkStartDate = !string.IsNullOrEmpty(model.RecordDate) ? model.RecordDate.ToDateTime() : DateTime.Now,
                                WorkEndDate = !string.IsNullOrEmpty(model.RecordDate) ? model.RecordDate.ToDateTime() : DateTime.Now,

                                WorkDescriptionCodeID = model.RecordTimeTypeID,
                                WorkPhaseCodeID = model.PhaseID,
                                RecordStateID = 1,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                WorkIVeEligibleCodeID = model.WorkIVeEligibleCodeID
                            }).FirstOrDefault();
                            if (!model.RecordTimeNote.IsNullOrEmpty())
                                UtilityFunctions.NoteInsert(152, 123, (int)workId, 16, "Record Time Note", model.RecordTimeNote);
                        }


                        var workRoleId = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                        {
                            WorkID = (int)workId,
                            RoleID = hidClientRoleId,
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                        }).FirstOrDefault();


                    }

                }

                if (model.AssociationTypeCodeID.HasValue && model.AssociationTypeCodeID.Value > 0)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_QuickRFDRoleInsert_sp",
                       new pd_QuickRFDRoleInsert_spParams()
                       {
                           UserID = UserManager.UserExtended.UserID,
                           RecordStateID = 1,
                           AddressCity = model.City,
                           AddressHomePhone = model.AddressPhone,
                           WorkPhone = model.WorkPhone,
                           CelPhone = model.CellPhone,
                           AddressCountryCodeID = model.CountryID,
                           AddressStateCodeID = model.StateID,
                           AddressStreet = model.Street,
                           AddressZipCode = model.Zip,
                           AssociationTypeCodeID = model.AssociationTypeCodeID.Value,
                           CaretakerFlag = model.CareTaker ? 1 : 0,
                           RFDID = reportFillingDueGet.HearingReportFilingDueID,
                           CaseID = UserManager.UserExtended.CaseID,
                           EmailAddress = model.EmailAddress,
                           FirstName = model.FirstName,
                           LastName = model.LastName,
                           RoleStartDate = model.RoleDate.ToDateTime(),
                           Non602ClientRoleIDList = nonClientRoleIds,
                           Number_602ClientRoleIDList = clientRoleIds,
                           AddrssStartDate = DateTime.Now
                       });
                }
                else if (model.PlacementAgencyID > 0)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_QuickRFDRoleInsert_sp",
                       new pd_QuickRFDRoleInsert_spParams()
                       {
                           UserID = UserManager.UserExtended.UserID,
                           RecordStateID = 1,
                           AddressCountryCodeID = model.CountryID,
                           CaretakerFlag = 0,
                           RFDID = reportFillingDueGet.HearingReportFilingDueID,
                           CaseID = UserManager.UserExtended.CaseID,

                           Non602ClientRoleIDList = nonClientRoleIds,
                           Number_602ClientRoleIDList = clientRoleIds,
                           AddressID = model.PlacementAgencyID
                       });
                }
            }



            return Json(new { Status = "Done" });

        }

        #endregion
    }
}