using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_EmployeeRoster
{
    public class pd_EmployeeRosterSearchGet_spResults
    {
        public int PersonID { get; set; }
        public string StaffName { get; set; }
        public string StaffPosition { get; set; }
        public string WorkContact { get; set; }
        public string MobileContact { get; set; }
        public string EmailContact { get; set; }
    }
}
