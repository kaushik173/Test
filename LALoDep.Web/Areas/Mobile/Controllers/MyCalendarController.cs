using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Mobile;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using Jcats.SD.UI.Areas.Mobile.Custom.Attribute;
using LALoDep.Areas.Mobile.Models;
using LALoDep.Controllers;
using LALoDep.Custom;

namespace LALoDep.Areas.Mobile.Controllers
{
    public partial class MyCalendarController : BaseController
    {
        private IUtilityService _utilityService;
        private UserManager _userManager;
        public MyCalendarController(UserManager userManager, IUtilityService utilityService)
        {
            _userManager = userManager;
            _utilityService = utilityService;
        }
        // GET: Mobile/MyCalendar
        public virtual ActionResult Index()
        {
            var ViewModel = new LALoDep.Areas.Mobile.Models.MyCalendarViewModel
            {
                StartDate = DateTime.Now.ToShortDateString(),
                EndDate = DateTime.Now.AddDays(7).ToShortDateString(),
                AttorneyPersonName = _userManager.UserExtended.FullName
            };
            return View(ViewModel);

        }
        [HttpPost]
 
        public virtual JsonResult Index(MyCalendarViewModel viewModel)
        {
            var spParams = new MobileIndividualCalendar_spParams
            {
                PersonID = _userManager.UserExtended.PersonID,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var calResult = _utilityService.ExecStoredProcedureWithResults<MobileIndividualCalendar_spResult>("MobileIndividualCalendar_sp", spParams)
                                            .Select(x => new
                                            {
                                                HearingID = x.HearingID.ToEncrypt(),
                                                SortEventDateTime = x.SortEventDateTime,
                                                EventDate = x.EventDate,
                                                EventTime = x.EventTime,
                                                HearingType = x.HearingType,
                                                Clients = x.Clients,
                                                CaseID = x.CaseID.ToEncrypt()
                                            }).ToList();


            var total = calResult.Count;
            return Json(new DataTablesResponse(0, calResult, total, total));

        }
    }
}