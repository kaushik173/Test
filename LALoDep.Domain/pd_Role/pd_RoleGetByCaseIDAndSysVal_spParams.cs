using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetByCaseIDAndSysVal_spParams
    {
        public int CaseID { get; set; }
        public int SysVal { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_RoleGetByCaseIDAndSysVal_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameDisplay { get; set; }
    }
}
