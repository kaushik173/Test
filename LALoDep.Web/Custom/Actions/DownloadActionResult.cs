using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Custom.Actions
{
    public class DownloadActionResult : ActionResult
    {
        string file;
        public DownloadActionResult(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("File Name is null or empty");
            }
            file = fileName;
        }
        public override void ExecuteResult(ControllerContext context)
        {

            string fullPath = UtilityFunctions.GetDocumentDownloadFolderPath() + file;
            if (!System.IO.File.Exists(fullPath))
            {
                context.HttpContext.Response.Write("File not found");
                return;
            }

            if (UserEnvironment.UserManager.UserExtended.PrintDocumentOn == "NewWindow" && Path.GetExtension(fullPath).Contains("pdf"))
            {
                //context.HttpContext.Response.AppendHeader("Content-Disposition", "inline; filename=" + file);

                //var bytes = File.ReadAllBytes(fullPath);
                //context.HttpContext.Response.ContentType = "application/pdf";
                //context.HttpContext.Response.BinaryWrite(bytes);
                context.HttpContext.Response.Redirect("~/Inquiry/Download?file=" + file);
                return;
            }


            context.HttpContext.Response.AppendHeader("Content-Disposition", "attachment; filename=" + file);

            context.HttpContext.Response.ContentType = "application/force-download";
            context.HttpContext.Response.TransmitFile(fullPath);





        }
    }
}