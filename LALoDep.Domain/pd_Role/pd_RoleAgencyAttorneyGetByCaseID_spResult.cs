using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleAgencyAttorneyGetByCaseID_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? Current { get; set; }
        public string StartDate { get; set; }
        public DateTime? JcatsUserEndDate { get; set; }
        
    }
}
