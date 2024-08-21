using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_PDActionGetAllCasesByUserID_spResults
    {
        
        public string FullName { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? PersonID { get; set; }
        public int? caseid { get; set; }
        public string casenumber { get; set; }
        
    }
}
