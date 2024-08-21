using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    public class SearchByPhysicalFileViewModel
    {
        public string FileName1 { get; set; }
        public string FileName2 { get; set; }
        public string FileName3 { get; set; }
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public string CaseNumber { get; set; }
        public int? AgencyID { get; set; }
        public bool OnViewLoad { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
    }
}