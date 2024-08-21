using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class ReferralSummaryViewModel
    {
        public bool IncludeInActiveStaff { get; set; }
        public int? ModeCodeID { get; set; }
        public IEnumerable<SelectListItem> ReferralSummaryModes { get;set;}
        public ReferralSummaryViewModel()
        {
            ReferralSummaryModes = new List<SelectListItem>();
        }
    }
}