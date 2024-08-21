using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using DataTables.Mvc;
using LALoDep.Domain.pd_ARSummary;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Core.Custom.Extensions;

namespace LALoDep.Controllers
{
    public partial class ARSummaryController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public ARSummaryController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.ARSummary, PageSecurityItemID = SecurityToken.ARSummary)]
        public virtual ActionResult Search()
        {
            var viewModel = new ARSummarySearchViewModel()
            {
                IncludeAll = true,
                Mode = "ByRequestee",
                OnViewLoad = true
            };
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Search(ARSummarySearchViewModel model)
        {
            byte includeAllInt = model.IncludeAll ? (byte)1 : (byte)0;
            var result =
                UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetSummary_spResult>(
                    "pd_HearingReportFilingDueGetSummary_sp",
                    new pd_HearingReportFilingDueGetSummary_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        IncludeAll = includeAllInt,
                        Mode = model.Mode,
                        UnderAgeFiveOnlyFlag = model.UnderAge5Only ? 1 : 0
                    }).ToList();

            var summaryModel = result.Select(x => new pd_HearingReportFilingDueGetSummary_spResult()
            {
                RoleType = x.RoleType,
                PersonNameFirst = x.PersonNameLast + ", " + x.PersonNameFirst,
                TotalDue = x.TotalDue,
                PastDue = x.PastDue,
                DuePrior7Days = x.DuePrior7Days,
                DuePrior30Days = x.DuePrior30Days,
                DuePrior60Days = x.DuePrior60Days,
                DuePrior90Days = x.DuePrior90Days,
                DuePrior180Days = x.DuePrior180Days,
                DueAfter180Days = x.DueAfter180Days,
                PersonID = x.PersonID
            }).ToList();

            return PartialView("_arSummurySearchResult", summaryModel);

        }
    }
}