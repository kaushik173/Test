using System.Collections.Generic;

namespace LALoDep.Models.Inquiry
{
    public class CaseLoadManagementViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AgencyID { get; set; }
        public IEnumerable<AgencyModel> AgencyList { get; set; }
         
    }
}