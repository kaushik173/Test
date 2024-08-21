using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CountyCounselList
{
    public class pd_AttorneyListSearchGet_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameFirst { get;set; }
        public string PersonNameLast { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public string BarNumber { get; set; }
    }
}
