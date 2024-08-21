using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using DataTables.Mvc;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_HourlyInvoiceList;
using LALoDep.Domain.pd_InvoiceQueue;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Enums;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Inquiry;
using LALoDep.Domain.pd_Person;

namespace LALoDep.Controllers.Inquiry
{
    public partial class InvoiceQueueController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public InvoiceQueueController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        // GET: InvoiceQueue
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.InvoiceQueue, PageSecurityItemID = SecurityToken.InvoiceQueue)]
        public virtual ActionResult Search()
        {
            var viewModel = new InvoiceQueueViewModel();
            viewModel.BranchList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                    "pd_CodeGetByTypeIDAndUserID_sp",
                    new pd_CodeGetByTypeIDAndUserID_spParams() { CodeTypeID = CodeType.Branch.GetHashCode(), UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                    {
                        Value = e.CodeID.ToString(),
                        //Text = e.CodeShortValue
                        Text = e.CodeValue
                    }).ToList();
            //viewModel.OnViewLoad = true;
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult Search(InvoiceQueueViewModel viewModel)
        {
            var result = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetPending_spResult>(
                "pd_InvoiceGetPending_sp",
                new pd_InvoiceGetPending_spParams()
                {
                    UserID = UserManager.UserExtended.UserID,
                    BranchCodeID = viewModel.BranchId,
                    BatchLogJobID = new Guid()
                }).ToList();
            var InvoiceQueueModel = result.Select(x => new
            {
                InvoiceDate = x.InvoiceDate,
                Status = x.Status,
                Client = x.Client,
                PetitionNumber = x.Petitions,
                Hearing = x.Hearings,
                Division = x.Division,
                Branch = x.Branch,
                CaseID = x.CaseID.ToEncrypt(),
                InvoiceID = x.InvoiceID.ToEncrypt()
            });

            return Json(new DataTablesResponse(0, InvoiceQueueModel, result.Count, result.Count));
        }
        [HttpPost]
        public virtual ActionResult PrintInvoiceQueue()
        {
            var spParams = new
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString()
            };
            var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", new com_ReportSourceGetByReportID_spParams()).ToList();
            ReportClass rpt = new ReportClass();
            string filename = "InvoiceQueue_" + UserManager.UserExtended.UserID.ToEncrypt() + ".pdf";

            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptInvoiceQueue.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_InvoiceQueuePrintableVersion_sp", spParams);
                rpt.SetDataSource(table);
               rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);


              

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


            return Download(filename);
        }
        public virtual ActionResult Download(string file)
        {
            string fullPath = UtilityFunctions.GetDocumentDownloadFolderPath() + file;


            if (UserManager.UserExtended.PrintDocumentOn == "NewWindow" && Path.GetExtension(fullPath).Contains("pdf"))
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                return File(fullPath, "application/pdf");
            }
            if (System.IO.File.Exists(fullPath))
            {

                return File(fullPath, "application/force-download", file);
            }
            else
            {
                return Content("File not found");
            }

        }

        public virtual ActionResult InvoiceVerifyPopup(string id)
        {
            //var personContact = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetJcatsEmailByUserID_spResult>("pd_PersonContactGetJcatsEmailByUserID_sp",
            //                                new pd_PersonContactGetJcatsEmailByUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).ToList();

            //var personContactRevenueRecovery = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetJcatsEmailByUserID_spResult>("pd_PersonContactGetJcatsEmailForRevenueRecovery_sp",
            //                                new pd_PersonContactGetJcatsEmailByUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).ToList();
            var viewModel = new InvoiceVerifyViewModel();
            viewModel.InvoiceID = id.ToDecrypt().ToInt();

            var agencyInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceID_spResult>("pd_InvoiceGetUnsentByInvoiceID_sp",
                                            new pd_InvoiceGetUnsentByInvoiceID_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).FirstOrDefault();
            viewModel.AgencyName = agencyInfo.AgencyName;
            viewModel.HHSANumber = agencyInfo.HHSA;
            viewModel.JCATSNumber = agencyInfo.CaseNumber;
            viewModel.Client = agencyInfo.PersonNameFirst + " " + agencyInfo.PersonNameLast;
            viewModel.DOB = agencyInfo.PersonDOB.ToDefaultFormat();
            viewModel.SSN = agencyInfo.SSN;
            if (!agencyInfo.MailStreet.IsNullOrEmpty())
            {
                viewModel.Address = agencyInfo.MailStreet + ", " + agencyInfo.MailCity + ", " + agencyInfo.MailState + " " + agencyInfo.MailZipCode + " " + agencyInfo.MailCountry;

            }
            else if (!agencyInfo.HomeStreet.IsNullOrEmpty())
            {
                viewModel.Address = agencyInfo.HomeStreet + ", " + agencyInfo.HomeCity + ", " + agencyInfo.HomeState + " " + agencyInfo.HomeZipCode + " " + agencyInfo.HomeCountry;

            }else { viewModel.Address = "";
            }
            var caseInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDPetition_spResult>("pd_InvoiceGetUnsentByInvoiceIDPetition_sp",
                                            new pd_InvoiceGetUnsentByInvoiceIDPetition_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).ToList();

            viewModel.CaseNumber = String.Join(", ", caseInfo.Select(o=>o.DocketNumber).ToArray());  

            viewModel.ParentList = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDParent_spResult>("pd_InvoiceGetUnsentByInvoiceIDParent_sp",
                                            new pd_InvoiceGetUnsentByInvoiceIDParent_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                        .Select(x => new ParentViewModel
                                        {
                                            Parent = x.PersonNameFirst + " " + x.PersonNameLast,
                                            Address =!x.Mailstreet.IsNullOrEmpty()? ((x.Mailstreet + "," + x.Mailcity + "," + x.Mailstate + " " + x.MailzipCode + "," + x.Mailstreet).Trim(',').Trim()): ((x.Homestreet + "," + x.Homecity + "," + x.Homestate + " " + x.HomezipCode + "," + x.HomeCountry).Trim(',').Trim()),
                                            DOB = x.DOB.ToDefaultFormat(),
                                            SSN = x.ssn
                                        }).ToList();


            //var bilingInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDService_spResult>("pd_InvoiceGetUnsentByInvoiceIDService_sp",
            //                                new pd_InvoiceGetUnsentByInvoiceIDService_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
            //                                .FirstOrDefault();


            //viewModel.Amount = bilingInfo.Amount;
            //viewModel.ServiceStartDate = bilingInfo.StartDate;
            //viewModel.ServiceEndDate = bilingInfo.EndDate;
            //viewModel.ServiceType = bilingInfo.ServiceType;

            viewModel.InvoiceDetails = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDService_spResult>("pd_InvoiceGetUnsentByInvoiceIDService_sp",
                                            new pd_InvoiceGetUnsentByInvoiceIDService_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                            .Select(x => new InvoiceDetails
                                            {
                                                Amount = x.Amount.ToDecimal(),
                                                ServiceStartDate = x.StartDate,
                                                ServiceEndDate = x.EndDate,
                                                ServiceType = x.ServiceType,
                                                HearingID = x.HearingID
                                            }).ToList();


            var note = UtilityFunctions.NoteGetByEntity(viewModel.InvoiceID, 122, 123).FirstOrDefault();
            if (note != null)
            {
                viewModel.NoteEntry = note.NoteEntry;
            }

            return View(viewModel);
        }

        private void updateInvoice(int invoiceID, List<InvoiceDetails> invoiceDetails)
        {
            var bilingInfos = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDService_spResult>("pd_InvoiceGetUnsentByInvoiceIDService_sp",
                                        new pd_InvoiceGetUnsentByInvoiceIDService_spParams() { InvoiceID = invoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).ToList();

            foreach (var bilingInfo in bilingInfos)
            {
                var changedHearing = invoiceDetails.FirstOrDefault(x => x.HearingID == bilingInfo.HearingID);

                if (changedHearing != null)
                {
                    if (bilingInfo.StartDate != changedHearing.ServiceStartDate)
                    {
                        var invoiceParam = new pd_InvoiceHearingUpdate_spParams()
                        {
                            InvoiceHearingID = bilingInfo.Identifier,
                            AgencyID = bilingInfo.AgencyID,
                            InvoiceID = bilingInfo.InvoiceID,
                            HearingID = bilingInfo.HearingID,
                            InvoiceHearingStartDate = DateTime.Parse(changedHearing.ServiceStartDate),
                            InvoiceHearingAmount = bilingInfo.Amount.ToDecimal(),
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = new Guid()
                        };

                        UtilityService.ExecStoredProcedureWithResults<object>("pd_InvoiceHearingUpdate_sp", invoiceParam).FirstOrDefault();
                    }
                }
            }
            TempData["InvoiceSaved"] = true;
        }

        [HttpPost]
        public virtual ActionResult UpdateInvoice(int id, List<InvoiceDetails> invoiceDetails)
        {
            updateInvoice(id, invoiceDetails);
            return Json(new { isSuccess = true });
        }

        [HttpPost]
        public virtual ActionResult ApproveInvoice(int id, List<InvoiceDetails> invoiceDetails)
        {
            if (invoiceDetails != null && invoiceDetails.Any())
                updateInvoice(id, invoiceDetails);

            var invoiceInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceID_spResult>("pd_InvoiceGetUnsentByInvoiceID_sp",
                                            new pd_InvoiceGetUnsentByInvoiceID_spParams() { InvoiceID = id, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).FirstOrDefault();
            if (invoiceInfo != null)
            {
                var pd_InvoiceUpdate_spParams = new pd_InvoiceUpdate_spParams
                {
                    InvoiceID = invoiceInfo.InvoiceID,
                    ClientRoleID = invoiceInfo.ClientID,
                    InvoiceDateTime = invoiceInfo.InvoiceDateTime,
                    InvoiceStatusCodeID = 2, //invoiceInfo.InvoiceStatusCodeID,
                    DepartmentID = invoiceInfo.DepartmentID,
                    InvoiceSentDate = invoiceInfo.InvoiceSentDate,
                    RecordStateID = invoiceInfo.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                UtilityService.ExecStoredProcedureWithResults<object>("pd_InvoiceUpdate_sp", pd_InvoiceUpdate_spParams).FirstOrDefault();
            }


            return Json(new { isSuccess = true });
        }
    }
}