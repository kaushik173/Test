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
using LALoDep.Domain.CSEC;
using LALoDep.Domain.NgInvoice;
using LALoDep.Models.Administration;

namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController
    {


        public virtual ActionResult InvoiceSummary()
        {

            var viewModel = new InvoiceSummaryViewModel();


            var yearList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetYearQuarter_spResult>(
                     "NgInvoice_GetYearQuarter_sp",
                     new NgInvoice_GetYearQuarter_spParams
                     {
                         CurrentDate = DateTime.Now,
                         PersonID = UserManager.UserExtended.PersonID,
                         LoadOption = "InvoiceSummary",
                         UserID = UserManager.UserExtended.UserID,


                     }).ToList();

            var oYear = yearList.FirstOrDefault(o => o.Selected == 1);
            if (oYear != null)
                viewModel.YearQuarterID = oYear.YearQuarterID.Value;
            viewModel.YearQuarterList = yearList.Select(o => new SelectListItem() { Text = o.YearQuaterDisplay, Value = o.YearQuarterID.ToString() });

            viewModel.ContractorList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetContractors_spResult>(
                   "NgInvoice_GetContractors_sp",
                   new NgInvoice_GetContractors_spParams
                   {

                       PersonID = UserManager.UserExtended.PersonID,
                       LoadOption = "MyInvoiceQueue",
                       UserID = UserManager.UserExtended.UserID,
                       CurrentDate = DateTime.Now,


                   }).Select(o => new SelectListItem() { Text = o.PersonDisplay, Value = o.PersonID.ToString() });
            viewModel.StatusList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetStatusCodes_spResult>(
                  "NgInvoice_GetStatusCodes_sp",
                  new NgInvoice_GetStatusCodes_spParams
                  {
                       
                      LoadOption = "InvoiceSummary",
                      UserID = UserManager.UserExtended.UserID,
                      AgencyID = UserManager.UserExtended.AgencyID,


                  }).ToList();
            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult InvoiceSummary(InvoiceSummaryViewModel model)
        {
             

            var data = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetInvoiceSummary_spResult>("NgInvoice_GetInvoiceSummary_sp", new NgInvoice_GetInvoiceSummary_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                ContractorPersonID = model.ContractorPersonID,
                YearQuarterID = model.YearQuarterID,
                LoadOption = "InvoiceSummary",
                PendingInvoicesOnly = model.PendingInvoicesFlag,
                StatusCodeID = model.StatusCodeID

            }).ToList();


            foreach (var item in data)
            {
                item.EncryptedYearQuarterID = item.YearQuarterID.ToEncrypt();
                item.EncryptedContractorPersonID = item.ContractorPersonID.ToEncrypt();
                item.EncryptedPostInvoiceYearQuarterID = item.PostInvoiceYearQuarterID.ToEncrypt();

            }
            return Json(new
            {
                Status = "Done",
                SearchData = new
                {
                    SearchResult = new DataTablesResponse(0, data, data.Count, data.Count),

                }
            });

        }
       
        public virtual ActionResult InvoiceGenerateFile(string id)
        {
            #region File Name
            var fileCount = -1;
            var extension = ".txt";
            var fileNameWithOutExtension = string.Format("InvoicesToBePosted_{0}", DateTime.Now.ToString("MM_dd_yyyy"));
            var fileName = string.Format("InvoicesToBePosted_{0}.txt", DateTime.Now.ToString("MM_dd_yyyy"));
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"] + "\\InvoicesToBePosted\\";
            fileCount = -1;
            extension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
            fileNameWithOutExtension = fileName.Substring(0,
               fileName.LastIndexOf(".", StringComparison.Ordinal));
            do
            {
                fileCount++;
            } while (
                System.IO.File.Exists(rootPath + fileNameWithOutExtension +
                                      (fileCount > 0 ? "[" + fileCount + "]" : "") + extension));

            fileName = rootPath + fileNameWithOutExtension + (fileCount > 0 ? "[" + fileCount + "]" : "") + extension;
            #endregion
            var invoiceLines = UtilityService.ExecStoredProcedureWithResults<InvoiceCreateExtractFileForPaymentResult>(
                          "InvoiceCreateExtractFileForPayment",
                          new InvoiceCreateExtractFileForPaymentParams
                          {
                              YearQuarterID = id.ToDecrypt().ToInt(),
                              UserID = UserManager.UserExtended.UserID,


                          }).Select(o => o.DATA).ToList();


            System.IO.File.WriteAllLines(fileName, invoiceLines);
            HttpContext.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(fileName));

            return File(fileName, "application/force-download");
        }


    }
}