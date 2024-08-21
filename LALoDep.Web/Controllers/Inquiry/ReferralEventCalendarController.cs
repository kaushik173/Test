using LALoDep.Domain.ref_Referral;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Controllers
{
    public partial class InquiryController
    {
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ReferralEventCalendar)]
        public virtual ActionResult ReferralEventCalendar()
        {
            var viewModel = new ReferralEventCalendarViewModel
            {
                StartDate = DateTime.Now.ToShortDateString(),
                EndDate = DateTime.Now.ToShortDateString(),
                PersonID = UserManager.UserExtended.PersonID,
            };

            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventCalendarGetAgency_spResult>("ref_ReferralEventCalendarGetAgency_sp", new ref_ReferralEventCalendarGetAgency_spParams() { LoadOption = "ReferralEventCalendar", UserID = UserManager.UserExtended.UserID })
                                                                 .Select(e => new SelectListItem() { Value = e.AgencyID.ToString(), Text = e.AgencyDisplay }).ToList();
            viewModel.ReferralTypeList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventCalendarGetCodes_spResult>("ref_ReferralEventCalendarGetCodes_sp", new ref_ReferralEventCalendarGetCodes_spParams() { CodeType = "ReferralType", LoadOption = "ReferralEventCalendar", UserID = UserManager.UserExtended.UserID })
                                                                 .Select(e => new SelectListItem() { Value = e.CodeID.ToString(), Text = e.CodeDisplay }).ToList();

            viewModel.EventTypeList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventCalendarGetCodes_spResult>("ref_ReferralEventCalendarGetCodes_sp", new ref_ReferralEventCalendarGetCodes_spParams() { CodeType = "EventType", LoadOption = "ReferralEventCalendar", UserID = UserManager.UserExtended.UserID });
            viewModel.EventLocationList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventCalendarGetCodes_spResult>("ref_ReferralEventCalendarGetCodes_sp", new ref_ReferralEventCalendarGetCodes_spParams() { CodeType = "EventLocation", LoadOption = "ReferralEventCalendar", UserID = UserManager.UserExtended.UserID });
            viewModel.AppearingStaffAttyList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventCalendarGetAppearingStaffAtty_spResult>("ref_ReferralEventCalendarGetAppearingStaffAtty_sp", new ref_ReferralEventCalendarGetAppearingStaffAtty_spParams() { LoadOption = "ReferralEventCalendar", UserID = UserManager.UserExtended.UserID });
            return View(viewModel);
        }
        [HttpPost]
        public virtual PartialViewResult ReferralEventCalendar(ReferralEventCalendarViewModel viewModel)
        {
            var ref_ReferralEventCalendarSearch_spParams = new ref_ReferralEventCalendarSearch_spParams()
            {
                StartDate = viewModel.StartDate?.ToDateTimeValue(),
                EndDate = viewModel.EndDate?.ToDateTime(),
                AgencyID = viewModel.AgencyID,
                ReferralTypeCodeID = viewModel.ReferralTypeID,
                AppearingPersonID = viewModel.AppearingPersonID,
                EventTypeCodeID = viewModel.EventTypeID,
                EventLocationCodeID = viewModel.EventLocationID,
                LoadOption = "ReferralEventCalendar",
                UserID = UserManager.UserExtended.UserID,
            };
            var data = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventCalendarSearch_spResult>("ref_ReferralEventCalendarSearch_sp", ref_ReferralEventCalendarSearch_spParams).ToList();
            return PartialView("_referralEventCalendarPartial", data);
        }
        [HttpPost]
        public virtual JsonResult UpdateReferralCase(int? refID,int? caseID,int? evtID)
        {
            if(caseID > 0)
               UserManager.UpdateCaseStatusBar(caseID.ToInt());

            return Json(new { isSuccess = true, URL = "/Case/ReferralEventAddEdit/" + (refID.HasValue ? refID.Value.ToEncrypt() : "") + "?eventId=" + evtID.Value.ToEncrypt() + "&pID=1" });
        }
    }
}