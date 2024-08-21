using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    public class JudgeTransferViewModel
    {
        public int? PersonID { get; set; }
        public string PersonName { get; set; }
        public int? TransferFromPersonID { get; set; }
        public int? TransferToPersonID { get; set; }

        public IEnumerable<SelectListItem> HearingOfficerList { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }

    }
}