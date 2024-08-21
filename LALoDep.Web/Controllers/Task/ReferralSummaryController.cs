using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Models.Task;
using LALoDep.Custom;
using LALoDep.Domain.ref_Referral;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        //[ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.ViewActionRequest)]
        public virtual ActionResult ReferralSummary()
        {
            var modes = UtilityService.ExecStoredProcedureWithResults<ref_ReferralSummaryModes_spResult>("ref_ReferralSummaryModes_sp",
                     new ref_ReferralSummaryModes_spParams()
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,

                     });

            var viewModel = new ReferralSummaryViewModel
            {
                IncludeInActiveStaff = true,
                ReferralSummaryModes = modes.Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() }),
                ModeCodeID = modes.FirstOrDefault(x => x.Selected == 1)?.CodeID
            };

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult ReferralSummarySearch(ReferralSummaryViewModel viewModel)
        {
            var data = UtilityService.ExecStoredProcedureWithResults<ref_ReferralSummary_spResult>("ref_ReferralSummary_sp",
                     new ref_ReferralSummary_spParams()
                     {
                         IncludeInactiveStaffFlag = viewModel.IncludeInActiveStaff ? 1 : 0,
                         ModeCodeID = viewModel.ModeCodeID,
                         BatchLogJobID = Guid.NewGuid(),
                         UserID = UserManager.UserExtended.UserID,
                     });

            return PartialView(data);
        }
    }
}