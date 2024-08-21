using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_HourlyInvoiceList;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using LALoDep.Core.Custom.Extensions;
using CrystalDecisions.Shared;
using Omu.ValueInjecter;

namespace LALoDep.Controllers
{
    public partial class HourlyInvoiceListController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public HourlyInvoiceListController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        #region Hourly Invoice List
        // GET: HourlyInvoiceList
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HourlyInvoiceList, PageSecurityItemID = SecurityToken.HourlyInvoiceList)]
        public virtual ActionResult Search(string id)
        {
            var viewModel = new HourlyInvoiceListViewModel()
            {
                OnViewLoad = true
            };
            viewModel.AttorneyID = id;
            viewModel.AttorneyList =
                UtilityService.ExecStoredProcedureWithResults<pd_HourlyInvoiceGetAttorneyList_spResult>(
                    "pd_HourlyInvoiceGetAttorneyList_sp",
                    new pd_HourlyInvoiceGetAttorneyList_spParams() { LoadOption = "SEARCH", UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                    {
                        Value = (e.PersonID + "-" + e.AgencyCountyID).ToEncrypt(),
                        Text = e.PersonNameDisplay
                    }).ToList();

            return View(viewModel);
        }
        [HttpPost]
        public virtual JsonResult Search(HourlyInvoiceListViewModel model)
        {
            int? personId = null; int? agencyCountryID = null;
            if (!string.IsNullOrEmpty(model.AttorneyID))
            {
                var pesronAndCountryID = model.AttorneyID.ToDecrypt().Split('-');
                personId = pesronAndCountryID[0].ToInt();
                agencyCountryID = pesronAndCountryID[1].ToInt();
            }
            var result = UtilityService.ExecStoredProcedureWithResults<pd_HourlyInvoiceSearch_spResult>(
                        "pd_HourlyInvoiceSearch_sp",
                        new pd_HourlyInvoiceSearch_spParams()
                        {
                            SortOption = "SEARCH",
                            AgencyCountyID = agencyCountryID,
                            PersonID = personId,
                            UserID = UserManager.UserExtended.UserID,
                            HourlyInvoiceID = model.InvoiceNumber,
                            BatchLogJobID = new Guid()
                        }).ToList();
            var hourlyInvoiceModel = result.Select(x => new
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceDate = x.InvoiceDate,
                InvoiceAmount = x.InvoiceAmount,
                Attorney = x.Attorney,
                ApprovalDate = x.ApprovalDate,
                ApprovalAmount = x.ApprovalAmount,
                EncryptedHourlyInvoiceID = x.HourlyInvoiceID.ToEncrypt()
            });

            return Json(new DataTablesResponse(0, hourlyInvoiceModel, result.Count, result.Count));
        }

        [HttpPost]
        public virtual ActionResult PrintHourlyInvoiceList(string id)
        {

            var com_ReportParameterValueDelete_spParams = new com_ReportParameterValueDelete_spParams()
            {
                ReportID = 92,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> deletedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueDelete_sp", com_ReportParameterValueDelete_spParams).ToList();

            List<object> deletedHeaderValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterValueDelete_spParams).ToList();

            if (!string.IsNullOrEmpty(id))
            {
                var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                {
                    ReportID = 92,
                    Sequence = 1,
                    Value = id.ToDecrypt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
            }

            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 92,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();
            string fileName = "HourlyInvoiceList_" + id + ".pdf";

            try
            {
                var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptAttorneyHourlyInvoice.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_AttorneyHourlyInvoice_sp", com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "rptAttorneyHourlyInvoice.rpt"))
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
        #endregion

        #region Hourly Invoice add
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HourlyInvoiceList, PageSecurityItemID = SecurityToken.HourlyInvoiceAdd)]
        public virtual ActionResult Add(string id)
        {
            ViewBag.AttorneyID = id;
            ViewBag.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_HourlyInvoiceGetAttorneyList_spResult>("pd_HourlyInvoiceGetAttorneyList_sp",
                                            new pd_HourlyInvoiceGetAttorneyList_spParams() { LoadOption = "ADD", UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                            .Select(e => new SelectListItem() { Value = (e.PersonID + "-" + e.AgencyCountyID).ToEncrypt(), Text = e.PersonNameDisplay }).ToList();


            return View();
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HourlyInvoiceList, PageSecurityItemID = SecurityToken.HourlyInvoiceAdd)]
        public virtual ActionResult SearchInvoiceToAdd(string id)
        {
            var pesronAndCountryID = id.ToDecrypt().Split('-');
            var personId = pesronAndCountryID[0].ToInt();
            var agencyCountryID = pesronAndCountryID[1].ToInt();

            var result = UtilityService.ExecStoredProcedureWithResults<pd_HourlyInvoiceGetWorkExpenseForInvoice_spResult>("pd_HourlyInvoiceGetWorkExpenseForInvoice_sp",
                                            new pd_HourlyInvoiceGetWorkExpenseForInvoice_spParams()
                                            {
                                                PersonID = personId,
                                                AgencyCountyID = agencyCountryID,
                                                LoadOption = "Add",
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = new Guid()
                                            });

            var count = result.Count();
            return Json(new DataTablesResponse(0, result, count, count));
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HourlyInvoiceList, PageSecurityItemID = SecurityToken.HourlyInvoiceAdd)]
        [HttpPost]
        public virtual ActionResult Add(string id, int agencyId, string workIds, string batchGuid)
        {
            var pesronAndCountryID = id.ToDecrypt().Split('-');
            var personId = pesronAndCountryID[0].ToInt();

            UtilityService.ExecStoredProcedureWithResults<object>("pd_HourlyInvoiceInsertByBatch_sp",
                                            new pd_HourlyInvoiceInsertByBatch_spParams()
                                            {
                                                AgencyID = agencyId,
                                                PersonID = personId,
                                                BatchGUID = Guid.Parse(batchGuid),
                                                WorkIDList = workIds,
                                                RecordStateID = 1,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            }).FirstOrDefault();


            return Json(new { isSuccess = true, URL = Url.Action(MVC.HourlyInvoiceList.Search(id)) });
        }

        #endregion

        #region Hourly Invoice Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HourlyInvoiceList, PageSecurityItemID = SecurityToken.HourlyInvoiceEdit)]
        public virtual ActionResult Edit(string id)
        {
            var viewModel = new HourlyInvoiceEditViewModel();
            var hourlyInvoice = UtilityService.ExecStoredProcedureWithResults<pd_HourlyInvoiceGet_spResult>("pd_HourlyInvoiceGet_sp",
                                            new pd_HourlyInvoiceGet_spParams()
                                            {
                                                HourlyInvoiceID = id.ToDecrypt().ToInt(),
                                                UserID = UserManager.UserExtended.UserID
                                            }).FirstOrDefault();
            viewModel.InjectFrom(hourlyInvoice);
            if (hourlyInvoice.HourlyInvoiceStatusDate.HasValue)
                viewModel.HourlyInvoiceStatusDate = hourlyInvoice.HourlyInvoiceStatusDate.Value.ToShortDateString();

            if (hourlyInvoice.HourlyInvoiceCourtApprovalDate.HasValue)
                viewModel.HourlyInvoiceCourtApprovalDate = hourlyInvoice.HourlyInvoiceCourtApprovalDate.Value.ToShortDateString();

            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HourlyInvoiceList, PageSecurityItemID = SecurityToken.HourlyInvoiceEdit)]
        [HttpPost]
        public virtual ActionResult Edit(HourlyInvoiceEditViewModel viewModel)
        {

            UtilityService.ExecStoredProcedureWithResults<object>("pd_HourlyInvoiceUpdate_sp",
                                            new pd_HourlyInvoiceUpdate_spParams()
                                            {
                                                HourlyInvoiceID = viewModel.HourlyInvoiceID,
                                                AgencyID = viewModel.AgencyID,
                                                PersonID = viewModel.PersonID,
                                                HourlyInvoiceStatusCodeID = viewModel.HourlyInvoiceStatusCodeID,
                                                HourlyInvoiceCourtApprovalAmount = viewModel.HourlyInvoiceCourtApprovalAmount,
                                                HourlyInvoiceCourtApprovalDate = DateTime.Parse(viewModel.HourlyInvoiceCourtApprovalDate),
                                                RecordStateID = (int)viewModel.RecordStateID,
                                                UserID = UserManager.UserExtended.UserID
                                            }).FirstOrDefault();

            return Json(new { isSuccess = true, URL = @Url.Action(MVC.HourlyInvoiceList.Search()) });
        }
        #endregion
    }
}