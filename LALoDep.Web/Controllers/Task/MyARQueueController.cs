using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using DataTables.Mvc;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using LALoDep.Models;
using LALoDep.Custom;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        #region My AR Queue

        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.ViewActionRequest)]
        public virtual ActionResult MyARQueue(string id)
        {

            var viewModel = new MyARQueueViewModel();
            //viewModel.CanAddAccess = UserManager.IsUserAccessTo(SecurityToken.ConflictAdd);
            //viewModel.CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.ConfictEdit);
            //viewModel.CanDeleteAccess = UserManager.IsUserAccessTo(SecurityToken.ConflictDelete);

            //When came to this page via menu (so for own account)
            viewModel.PersonID = UserManager.UserExtended.PersonID;
            viewModel.PersonName = UserManager.UserExtended.FullName;
            if (!string.IsNullOrEmpty(id))
            {
                //When came to this page via AR Summuary (so for selected person)
                var pd_PersonGet_spParams = new pd_PersonGet_spParams()
                {
                    PersonID = id.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var PersonInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", pd_PersonGet_spParams).FirstOrDefault();
                if (PersonInfo != null)
                {
                    viewModel.PersonID = PersonInfo.PersonID;
                    viewModel.PersonName = PersonInfo.LastName + ", " + PersonInfo.FirstName;
                }
            }
            return View(viewModel);
        }


        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.ViewActionRequest)]
        [HttpPost]
        public virtual JsonResult MyARQueueList(MyARQueueViewModel viewModel)
        {
            var pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams = new pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams()
            {
                DateRangeType = viewModel.DateRangeType,
                IncludeCompletedFlag = (byte)viewModel.IncludeCompletedFlag,
                PersonID = viewModel.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Investigator"
            };
            if (!string.IsNullOrEmpty(viewModel.StartDate))
                pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams.StartDate = viewModel.StartDate.ToDateTime();
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams.EndDate = viewModel.EndDate.ToDateTime();
            var hearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetByDateRangeTypePersonID_spResult>("pd_HearingReportFilingDueGetByDateRangeTypePersonID_sp", pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams).ToList();

            int? PriviousId = 0;
            foreach (var item in hearingList)
            {
                if (PriviousId == item.ReportFilingDueID)
                {
                    item.RequestedByName = string.Empty;
                    item.RequestedForName = string.Empty;
                    item.ReportFilingDueType = string.Empty;
                    item.RequestDate = null;
                    item.DueDate = null;

                    item.HearingDate = null;
                    item.HearingType= string.Empty;
                }

                PriviousId = item.ReportFilingDueID;
            }
            var list = hearingList.Select(x => new
            {
                RequestedByName = x.RequestedByName,
                RequestedForName = x.RequestedForName,
                ReportFilingDueType = x.ReportFilingDueType,
                RequestDate = (x.RequestDate != null) ? x.RequestDate.ToShortDateString() : string.Empty,
                DueDate = (x.DueDate != null) ? x.DueDate.ToShortDateString() : string.Empty,
                CompleteDate = x.CompleteDate,
                HearingType = x.HearingType,
                HearingDate = (x.HearingDate != null) ? x.HearingDate.ToShortDateString() : string.Empty,
                PetitionNumber = x.PetitionNumber,
                Client = x.Client,
                EncryptedCaseID = x.CaseID.ToEncrypt(),
                DisplayQuickAR = string.IsNullOrEmpty(x.CompleteDate),
                EncryptedReportFilingDueID = x.ReportFilingDueID.ToEncrypt()
            }).ToList();

            var total = list.Count;
            if (hearingList.Any())
            {
                total = hearingList.First().TotalRFD ?? 0;
            }


            var jsonResult = Json(new DataTablesResponse(0, list, total, total));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        public virtual ActionResult MyARQueuePrint(MyARQueueViewModel viewModel)
        {
            var rpt = new ReportClass();
            try
            {
                var spResult = new List<com_ReportSourceGetByReportID_spResult>
            {
                new com_ReportSourceGetByReportID_spResult()
                {
                    ReportDisplayName = "CaseSummary",
                    ReportSourceStoredProcedureName = "rpt_CaseSummary_sp",
                    ReportSourceDocumentName = "CaseSummary.rpt",
                    ReportSourceSequence = 1
                }
            };




                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptRFDQueuePrintableVersion.rpt");
                rpt.Load();
                var oParams = new pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams()
                {
                    DateRangeType = viewModel.DateRangeType,
                    IncludeCompletedFlag = (byte)viewModel.IncludeCompletedFlag,
                    PersonID = viewModel.PersonID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Investigator",

                };
                if (!string.IsNullOrEmpty(viewModel.StartDate))
                    oParams.StartDate = viewModel.StartDate.ToDateTime();
                if (!string.IsNullOrEmpty(viewModel.EndDate))
                    oParams.EndDate = viewModel.EndDate.ToDateTime();
                var hearingList = UtilityService.ExecStoredProcedureForDataTableADO("pd_HearingReportFilingDueGetByDateRangeTypePersonID_sp", oParams);

                rpt.SetDataSource(hearingList);

                var fileName = "MyARQueue.pdf";

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {
                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + fileName;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
                    rpt.Close();
                    rpt.Dispose();

                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + fileName) });
                }

                var stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf", fileName);
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

        #endregion

        #region AR Transfer
        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.TransferActionRequest)]
        public virtual ActionResult ARTransfer(string id)
        {
            var viewModel = new ARTransferViewModel();
            //When came to this page via menu (so for own account)
            viewModel.PersonID = UserManager.UserExtended.PersonID;
            viewModel.PersonName = UserManager.UserExtended.FullName;
            if (!string.IsNullOrEmpty(id))
            {
                //When came to this page via AR Summuary (so for selected person)
                var pd_PersonGet_spParams = new pd_PersonGet_spParams()
                {
                    PersonID = id.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var PersonInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", pd_PersonGet_spParams).FirstOrDefault();
                if (PersonInfo != null)
                {
                    viewModel.PersonID = PersonInfo.PersonID;
                    viewModel.PersonName = PersonInfo.LastName + ", " + PersonInfo.FirstName;
                }
            }

            var personParam = new pd_RoleAgencyInvestigatorGet_spParams { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() };
            viewModel.TransferTo = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyInvestigatorGet_spResults>("pd_RoleAgencyInvestigatorGet_sp", personParam)
                                    .Select(x => new PersonViewModel
                                    {
                                        PersonNameDisplay = x.PersonNameDisplay,
                                        PersonID = x.PersonID
                                    }).ToList();
            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.TransferActionRequest)]
        [HttpPost]
        public virtual JsonResult ARTransferList(ARTransferViewModel viewModel)
        {
            var pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams = new pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams()
            {
                DateRangeType = viewModel.DateRangeType,
                IncludeCompletedFlag = 0,
                PersonID = viewModel.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Transfer"
            };
            if (!string.IsNullOrEmpty(viewModel.StartDate))
                pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams.StartDate = viewModel.StartDate.ToDateTime();
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams.EndDate = viewModel.EndDate.ToDateTime();
            var hearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetByDateRangeTypePersonID_spResult>("pd_HearingReportFilingDueGetByDateRangeTypePersonID_sp", pd_HearingReportFilingDueGetByDateRangeTypePersonID_spParams).ToList();

            int? PriviousId = 0;
            foreach (var item in hearingList)
            {
                if (PriviousId == item.ReportFilingDueID)
                {
                    item.RequestedByName = string.Empty;
                    item.RequestedForName = string.Empty;
                    item.ReportFilingDueType = string.Empty;
                    item.RequestDate = null;
                    item.DueDate = null;
                }
                PriviousId = item.ReportFilingDueID;
            }

            var list = hearingList.Select(x => new
            {
                RequestedByName = x.RequestedByName,
                RequestedForName = x.RequestedForName,
                ReportFilingDueType = x.ReportFilingDueType,
                RequestDate = (x.RequestDate != null) ? x.RequestDate.ToShortDateString() : string.Empty,
                DueDate = (x.DueDate != null) ? x.DueDate.ToShortDateString() : string.Empty,
                CompleteDate = x.CompleteDate ?? string.Empty,
                HearingType = x.HearingType,
                HearingDate = (x.HearingDate != null) ? x.HearingDate.ToShortDateString() : string.Empty,
                PetitionNumber = x.PetitionNumber,
                Client = x.Client,

                ReportFilingDueID = x.ReportFilingDueID,
                AgencyID = x.AgencyID,
                HearingID = x.HearingID,
                HearingReportFilingDueTypeCodeID = x.HearingReportFilingDueTypeCodeID,
                RequestedByPersonID = x.RequestedByPersonID,
                HearingReportFilingDueLegalResearchTypeCodeID = x.HearingReportFilingDueLegalResearchTypeCodeID,
                RecordStateID = x.RecordStateID
            }).ToList();

            var total = list.Count;
            return Json(new DataTablesResponse(0, list, total, total));
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.TransferActionRequest)]
        public virtual ActionResult ARTransfer(ARTransferModel model)
        {
            foreach (var ar in model.ARsToTransfer)
            {
                var param = new pd_HearingReportFilingDueUpdate_spParams
                {
                    HearingReportFilingDueID = ar.ReportFilingDueID,
                    AgencyID = ar.AgencyID,
                    HearingID = ar.HearingID,
                    HearingReportFilingDueTypeCodeID = ar.HearingReportFilingDueTypeCodeID,
                    HearingReportFilingDueOrderDate = ar.RequestDate,
                    HearingReportFilingDueDate = ar.DueDate,
                    RequestedByPersonID = ar.RequestedByPersonID,

                    RequestedForPersonID = model.TransferToPersonID,

                    HearingReportFilingDueLegalResearchTypeCodeID = ar.HearingReportFilingDueLegalResearchTypeCodeID,
                    RecordStateID = ar.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };

                if (!string.IsNullOrEmpty(ar.CompleteDate))
                    param.HearingReportFilingDueEndDate = DateTime.Parse(ar.CompleteDate);

                UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingReportFilingDueUpdate_sp", param);
            }

            return Json(new { isSuccess = true });
        }
        #endregion

    }
}