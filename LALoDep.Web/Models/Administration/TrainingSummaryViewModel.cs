using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class TrainingSummaryViewModel
    {
        public IEnumerable<SelectListItem> VenueList { get; set; }
        public IEnumerable<SelectListItem> SupervisorList { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<SelectListItem> AgencyGroupList { get; set; }
        public bool OnViewLoad { get; set; }
        public int AgencyGroupID { get; set; }
        public int AgencyID { get; set; }
        public int VenueID { get; set; }
        public int SupervisorID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int IncludeAllActiveStuff { get; set; }
        public bool IncludeAllActiveStuffBool { get; set; }
    }
}