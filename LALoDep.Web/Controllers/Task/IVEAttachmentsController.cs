using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.IVEActvityLog;
using LALoDep.Domain.IVeActivity;
using Newtonsoft.Json;
using System.Collections.Generic;
using LALoDep.Domain.TitleIVe;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models.Task;
using DataTables.Mvc;
using System.IO;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {

        public virtual ActionResult IVEAttachments(string DocType, string TitleIVeItemID, string TitleIVeInvoiceID)
        {
            var model = new IVEAttachmentsViewModel();
            model.DocumentType = DocType;
            model.TitleIVeItemID = TitleIVeItemID;
            model.TitleIVeInvoiceID = TitleIVeInvoiceID;
            model.AttachFileTypes = "gif|jpe?g|png|pdf|txt|docx|doc|xls|xlsx|csv|ppt|pptx";


            model.AttachFileMaxSize = "15000000";


            return View(model);
        }
        [HttpPost]
        public virtual JsonResult IVEAttachmentsFiles(string DocType, string TitleIVeItemID, string TitleIVeInvoiceID)
        {
            var returnPathParams = string.Format("&DocType={0}&TitleIVeItemID={1}&TitleIVeInvoiceID={2}", DocType, TitleIVeItemID, TitleIVeInvoiceID);

            var caseFileGetByCaseList =
               UtilityService.ExecStoredProcedureWithResults<TitleIVeInvoiceSupportingDocGet_spResult>(
                   "TitleIVeInvoiceSupportingDocGet_sp",
                   new TitleIVeInvoiceSupportingDocGet_spParams
                   {
                       DocumentType = DocType,
                       TitleIVeItemID = TitleIVeItemID.ToDecrypt().ToInt(),
                       InvoiceID = TitleIVeInvoiceID.ToDecrypt().ToInt(),

                       UserID = UserManager.UserExtended.UserID,

                   }).ToList();



            for (var i = 0; i < caseFileGetByCaseList.Count; i++)
            {
                var item = caseFileGetByCaseList[i];

                item.Path = "/Task/IVEDownloadFile?path=" + item.Path.ToEncrypt() + "&fileName=" + item.DocumentName + returnPathParams;
                caseFileGetByCaseList[i] = item;


            }


            var total = caseFileGetByCaseList.Count;

            return new JsonResult()
            {
                Data = new DataTablesResponse(0, caseFileGetByCaseList, total, total),

                MaxJsonLength = Int32.MaxValue
            };

        }

        [HttpPost]
        public virtual JsonResult IVEDeleteFile(int id)
        {

            UtilityService.ExecStoredProcedureWithoutResultADO("TitleIVeInvoiceSupportingDocInsertDelete_sp", new TitleIVeInvoiceSupportingDocInsertDelete_spParams()
            {

                UserID = UserManager.UserExtended.UserID,
                TitleIVeInvoiceSupportingDocID = id,
                Delete = "Yes"


            });
            return Json(new { Status = "done" });
        }


        [HttpPost]
        public virtual JsonResult IVEAttachFileUpload(string DocType, string TitleIVeItemID, string TitleIVeInvoiceID)
        {
            if (Request.Files.Count == 0)
            {

                Response.ContentType = "text/plain";
                Response.Write("No files received.");

            }
            else
            {

                var userId = UserManager.UserExtended.UserID;
                var uploadedfile = Request.Files[0];

                var fileName = uploadedfile.FileName.ToRemoveSplChars();
                var fileType = uploadedfile.ContentType;
                var fileSize = uploadedfile.ContentLength;


                #region Get FullPath and SubRootPath
                var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];


                var fileSavePath = rootPath;
                var subRootPath = @"TitleIVe\{0}\{1}\{2}\";



                var headerData = UtilityService.ExecStoredProcedureWithResults<TitleIVeExpenseHeader_spResult>("TitleIVeExpenseHeader_sp", new TitleIVeExpenseHeader_spParams
                {
                    InvoiceID = TitleIVeInvoiceID.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
                if (headerData != null)
                {
                    var invDate = headerData.InvoiceDate.ToDateTime();
                    subRootPath = string.Format(subRootPath, invDate.Year, invDate.Month, TitleIVeInvoiceID.ToDecrypt().ToInt());
                }
                fileSavePath += "\\" + subRootPath;
                if (!Directory.Exists(fileSavePath))
                    Directory.CreateDirectory(fileSavePath);

                #endregion

                #region Rename file if duplicate

                var fileCount = -1;
                var extension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                var fileNameWithOutExtension = fileName.Substring(0,
                    fileName.LastIndexOf(".", StringComparison.Ordinal));
                do
                {
                    fileCount++;
                } while (
                    System.IO.File.Exists(fileSavePath + "\\" + fileNameWithOutExtension +
                                          (fileCount > 0 ? "[" + fileCount + "]" : "") + extension));

                fileName = fileNameWithOutExtension + (fileCount > 0 ? "[" + fileCount + "]" : "") + extension;

                #endregion



                uploadedfile.SaveAs(fileSavePath + "\\" + fileName);

                //rollback
                //  System.IO.File.SetAttributes(fileSavePath + "\\" + fileName, FileAttributes.ReadOnly);


                var description = Request.Form["description[]"];


                var insertFileResult = UtilityService.ExecStoredProcedureScalar("TitleIVeInvoiceSupportingDocInsertDelete_sp",
                          new TitleIVeInvoiceSupportingDocInsertDelete_spParams()
                          {

                              UserID = UserManager.UserExtended.UserID,
                              DocumentName = fileName,
                              Path = subRootPath + fileName,
                              DocumentType = DocType,
                              TitleIVeItemID = TitleIVeItemID.ToDecrypt().ToInt()==0?(int?)null: TitleIVeItemID.ToDecrypt().ToInt(),
                              InvoiceID = TitleIVeInvoiceID.ToDecrypt().ToInt(),
                              Note = description

                          });


                Response.ContentType = "text/plain";
                Response.Write("{\"name\":\"" + fileName + "\",\"type\":\"" + fileType + "\",\"size\":\"" + fileSize + "\",\"description\":\"" + description + "\"}");

            }


            return Json(new { Status = "Done" });
        }

        public virtual ActionResult IVEDownloadFile(string path, string fileName, string DocType, string TitleIVeItemID, string TitleIVeInvoiceID)
        {


            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];
            var fileSavePath = rootPath + "\\" + path.ToDecrypt();




            if (!System.IO.File.Exists(fileSavePath))
            {
                TempData["ErrorMessage"] = "File doesn't exist";
                return RedirectToAction("IVEAttachments", new { DocType, TitleIVeItemID, TitleIVeInvoiceID });
            }


            //   byte[] fileBytes = System.IO.File.ReadAllBytes(fileSavePath);

            return File(fileSavePath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
    }


}