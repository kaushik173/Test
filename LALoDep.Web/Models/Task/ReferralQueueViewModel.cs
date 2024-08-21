using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class ReferralQueueViewModel
    {
        public int? ReferralPersonID { get; set; }
        public string PersonName { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public int? DateRangeTypeCodeID { get; set; }
        public bool IncludeCompletedReferrals { get; set; }
        public IEnumerable<SelectListItem> DateRangeTypes { get;set;}

        public ReferralQueueViewModel()
        {
            DateRangeTypes = new List<SelectListItem>();
        }
    }
}