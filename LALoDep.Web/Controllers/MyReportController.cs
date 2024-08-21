using LALoDep.Domain.pd_JcatsReport;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Jcats;
using LALoDep.Domain.NG_com;
using LALoDep.Domain.pd_Report;
using LALoDep.Models.Inquiry;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class MyReportController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public MyReportController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        public virtual ActionResult Index(string returnURL)
        {
            var viewModel = new MyReportIndexViewModel();
            if (string.IsNullOrEmpty(returnURL))
                returnURL = MVC.Inquiry.ActionNames.SupvAtty;

            var pd_ReportPersonGetAllByPersonID_spParams = new pd_ReportPersonGetAllByPersonID_spParams
            {
                PersonID = UserManager.UserExtended.PersonID,
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            };
            
            viewModel.Reports = UtilityService.ExecStoredProcedureWithResults<pd_ReportPersonGetAllByPersonID_spResult>("pd_ReportPersonGetAllByPersonID_sp", pd_ReportPersonGetAllByPersonID_spParams).Select(x => new ReportViewModel()
            {
                ReportID = x.JcatsReportID,
                ReportPersonID = x.ReportPersonID,
                ReportValue = x.JcatsReportName,
                ReportDescription = x.JcatsReportDescription,
                Selected = x.MyReportFlag == 1
            }).ToList();
            viewModel.ReturnUrl = "/" + MVC.Inquiry.Name + "/" + returnURL;
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult MyReportSave(MyReportIndexViewModel viewmodel)
        {
           
                var pd_ReportPersonGetAllByPersonID_spParams = new pd_ReportPersonGetAllByPersonID_spParams
                {
                    PersonID = UserManager.UserExtended.PersonID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                };

                var data = UtilityService.ExecStoredProcedureWithResults<pd_ReportPersonGetAllByPersonID_spResult>("pd_ReportPersonGetAllByPersonID_sp", pd_ReportPersonGetAllByPersonID_spParams).ToList();

                if (viewmodel.oldSavedReportIds.CompareTo(viewmodel.saveReportIds) != 0)
                {
                    List<string> oldSavedReportArray = new List<string>(viewmodel.oldSavedReportIds.Split(';'));

                    List<string> newAddReportArray = new List<string>(viewmodel.saveReportIds.Split(';'));

                    List<string> deleteList = oldSavedReportArray.Except(newAddReportArray).ToList();

                    List<string> addList = newAddReportArray.Except(oldSavedReportArray).ToList();

                    foreach (var reportId in deleteList)
                    {
                        if (reportId != "")
                        {
                            var pd_ReportPersonDelete_spParams = new pd_ReportPersonDelete_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                ReportPersonID = data.First(o => o.JcatsReportID == Convert.ToInt32(reportId)).ReportPersonID,
                                LoadOption = "ReportPerson",
                                RecordStateID=10,
                            };
                            List<object> d = UtilityService.ExecStoredProcedureWithResults<object>("pd_ReportPersonDelete_sp", pd_ReportPersonDelete_spParams).ToList();
                        }
                    }

                    foreach (var reportId in addList)
                    {
                        if (reportId != "")
                        {
                            var pd_ReportPersonInsert_spParams = new pd_ReportPersonInsert_spParams
                            {
                                PersonID = UserManager.UserExtended.PersonID,
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                ReportID = Convert.ToInt32(reportId),
                                ReportPersonDisplayOrder = 1,
                                RecordStateID = 1,
                            };

                            var d = UtilityService.ExecStoredProcedureWithResults<int>("pd_ReportPersonInsert_sp", pd_ReportPersonInsert_spParams).FirstOrDefault();
                        }
                    }
                }
            return Json(new { URL = viewmodel.ReturnUrl }, JsonRequestBehavior.AllowGet);
        }
    }
}