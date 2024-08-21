using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.com_Jcats;
using LALoDep.Domain.NgInvoice;
using LALoDep.Domain.pd_CountyCounselList;
using LALoDep.Domain.pd_JcatsGroup;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using Jcats.SD.Domain.com_Jcats;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Models.Administration;


namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController : Controller
    {
        [ClaimsAuthorize(IsPageMainMethod = true, PageSecurityItemID = SecurityToken.ImportPaidInvoices)]
        public virtual ActionResult ImportPaidInvoices()
        {
            var viewModel = new ImportPaidInvoicesViewModel();
            return View(viewModel);
        }
        [HttpPost]
        [ClaimsAuthorize(IsPageMainMethod = true, PageSecurityItemID = SecurityToken.ImportPaidInvoices)]
        public virtual ActionResult ImportPaidInvoices(FormCollection form)
        {
            var viewModel = new ImportPaidInvoicesViewModel();

            #region FileUpload
            HttpPostedFileBase file = Request.Files[0];
            string FileExtension = Path.GetExtension(file.FileName);
            if (FileExtension.ToLower() != ".xls" && FileExtension.ToLower() != ".xlsx")
            {
                viewModel.ErrorMessage = "Invalid File";
                return View(viewModel);
            }

            string FileName = Path.GetFileNameWithoutExtension(file.FileName);
            string UploadPath = Server.MapPath("~/Documents/UploadPaidInvoicesFiles/");
            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }
            var NgInvoiceImportFileName = FileName;
            var NgInvoiceImportFileNameWithExtension = Path.GetFileName(file.FileName);
            viewModel.FileName = Guid.NewGuid().ToString() + "-" + DateTime.Now.ToString("yyyyMMddhhss") + "-" + FileName.Trim() + FileExtension;
            FileName = UploadPath + viewModel.FileName;


            file.SaveAs(FileName);


            #endregion
            try
            {


                var dt = LALoDep.Custom.UtilityFunctions.GetDataTableFromXlsFile(FileName);


                var columns = new[] { "Document Number", "Vendor Name", "Credit", "Debit", "Order", "Document Header Text", "Item text", "Document Date"
                    , "User Name", "Journal/Invoice #", "Cost Center","WBS element","G/L Account","Fund"};
                foreach (var col in columns)
                {
                    if (dt.Columns[col] == null)
                    {
                        viewModel.ErrorMessage = col + " column not found in uploaded file";
                        break;
                    }
                }
                if (!viewModel.ErrorMessage.IsNullOrEmpty())
                {
                    return View(viewModel);
                }
                var NgInvoiceImportBatchGUID = Guid.NewGuid().ToString();
                var NgInvoiceImportStagedOn = DateTime.Now;
                foreach (DataRow dr in dt.Rows)
                {
                    if (!dr["Document Number"].ToCellString().IsNullOrEmpty() && dr["Document Number"].ToCellString().Length > 0)
                    {

                        UtilityService.ExecStoredProcedureWithoutResults(
                                  "NgInvoiceImportIUD_sp", new LALoDep.Domain.NgInvoice.NgInvoiceImportIUD_spParams()
                                  {
                                      CostCenter = dr["Cost Center"].ToCellString(),
                                      Credit = dr["Credit"].ToCellString(),
                                      Debit = dr["Debit"].ToCellString(),
                                      DocumentDate = dr["Document Date"].ToCellString(),
                                      DocumentHeaderText = dr["Document Header Text"].ToCellString(),
                                      DocumentNumber = dr["Document Number"].ToCellString(),
                                      Fund = dr["Fund"].ToCellString(),
                                      GLAccount = dr["G/L Account"].ToCellString(),
                                      ItemText = dr["Item text"].ToCellString(),
                                      JournalInvoiceNbr = dr["Journal/Invoice #"].ToCellString(),
                                      Order = dr["Order"].ToCellString(),
                                      UserName = dr["User Name"].ToCellString(),
                                      VendorName = dr["Vendor Name"].ToCellString(),
                                      WBSelement = dr["WBS element"].ToCellString(),
                                      IUD = "INSERT",
                                      NgInvoiceImportBatchGUID = NgInvoiceImportBatchGUID,
                                      NgInvoiceImportFileName = NgInvoiceImportFileNameWithExtension,
                                      NgInvoiceImportStagedOn = NgInvoiceImportStagedOn,
                                      UserID = UserManager.UserExtended.UserID,

                                  });
                    }

                }

                viewModel.NgInvoiceImportProcessRecords = UtilityService.ExecStoredProcedureWithResults<NgInvoiceImport_ProcessRecords_spResult>(
                 "NgInvoiceImport_ProcessRecords_sp", new NgInvoiceImport_ProcessRecords_spParams
                 {
                     NgInvoiceImportFileName = NgInvoiceImportFileNameWithExtension,
                     NgInvoiceImportStagedOn = NgInvoiceImportStagedOn,
                     NgInvoiceImportBatchGUID = NgInvoiceImportBatchGUID,
                     UserID = UserManager.UserExtended.UserID
                 }).ToList();

            }
            catch
            {
                viewModel.ErrorMessage = "Invalid File";
                return View(viewModel);
            }
            return View(viewModel);
        }

    }
}