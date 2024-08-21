using LALoDep.Domain.ref_Referral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    public class ReferralEventCalendarViewModel
    {
        public int? PersonID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? AgencyID { get; set; }
        public int? ReferralTypeID { get; set; }
        public int? AppearingPersonID { get; set; }
        public int? EventTypeID { get; set; }
        public int? EventLocationID { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<SelectListItem> ReferralTypeList { get; set; }
        public IEnumerable<ref_ReferralEventCalendarGetCodes_spResult> EventTypeList { get; set; }
        public IEnumerable<ref_ReferralEventCalendarGetCodes_spResult> EventLocationList { get; set; }
        public IEnumerable<ref_ReferralEventCalendarGetAppearingStaffAtty_spResult> AppearingStaffAttyList { get; set; }
    }
}