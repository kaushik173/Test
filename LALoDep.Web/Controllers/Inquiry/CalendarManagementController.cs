using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Inquiry;
using CrystalDecisions.CrystalReports.Engine;
using LALoDep.Domain.com_Report;
using LALoDep.Custom;
using System.IO;
using LALoDep.Core.Custom.Utility;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class InquiryController : Controller
    {

        // GET: CalendarManagement
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewCalendarManagement)]
        public virtual ActionResult CalendarManagement()
        {
            var model = new CalendarManagementViewModel();
            /*
            if (Session["CalendarStartDate"] != null)
                model.StartDate = Session["CalendarStartDate"].ToString();
            else
                Session["CalendarStartDate"] = model.StartDate = DateTime.Now.ToString("MM/dd/yyyy");

            if (Session["CalendarEndDate"] != null)
                model.EndDate = Session["CalendarEndDate"].ToString();
            else
                Session["CalendarEndDate"] = model.EndDate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");
            */
            TempData["StartDate"] = model.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            TempData["EndDate"] = model.EndDate = DateTime.Now.ToString("MM/dd/yyyy");

            return View(model);

        }
        /// <summary>
        /// Last Edit on : 19 Feb,2013
        /// Last Edit By: Humair Ahmed
        /// Reason:ID Encryption 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewCalendarManagement)]
        [HttpPost]
        public virtual PartialViewResult CalendarManagement(CalendarManagementViewModel model)
        {
            //Session["CalendarStartDate"] = model.StartDate;
            //Session["CalendarEndDate"] = model.EndDate;

            TempData["StartDate"] = model.StartDate;
            TempData["EndDate"] = model.EndDate;

            var spParams = new pd_CalendarGetSummaryByStaffStartDateEndDate_spParams()
            {
                StartDate = DateTime.Parse(model.StartDate),
                EndDate = DateTime.Parse(model.EndDate),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };

            var data = UtilityService.ExecStoredProcedureWithResults<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult>(
                                                "pd_CalendarGetSummaryByStaffStartDateEndDate_sp", spParams).ToList();

            return PartialView("_calendarSearchResult", data);
        }
        [HttpPost]
        public virtual ActionResult PrintCalendarManagement(CalendarManagementViewModel viewModel)
        {



            var fileName = Guid.NewGuid() + ".pdf";
            var rpt = new ReportClass
            {
                FileName = HttpContext.Server.MapPath("~/Reports/IndividualCalendar.rpt")
            };
            try
            {
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTableADO("rpt_IndividualCalendar_sp",
                     new Dictionary<string, object>()
                {
                  {"@PersonID", null},
                  {"@StartDate", viewModel.StartDate.ToDateTime()},
                  {"@EndDate", viewModel.EndDate.ToDateTime()},
                  {"@HearingTypeID", null},
                  {"@UserID", UserManager.UserExtended.UserID},
                  {"@BatchLogJobID", Guid.NewGuid()}
                });

                rpt.SetDataSource(table);


               

                //if file already exists then delete it
                if (System.IO.File.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\" + fileName))
                    System.IO.File.Delete(Server.MapPath("~") + "MergeTemplate\\Download\\" + fileName);

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {


                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + fileName;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);



                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();

                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + fileName) });
                }
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
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
          

            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);

        }
    }
}