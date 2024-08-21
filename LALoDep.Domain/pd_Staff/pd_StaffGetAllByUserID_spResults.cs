using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Staff
{
    public class pd_StaffGetAllByUserID_spResults
    {
        public string FullName { get; set; }
        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
    }
}
