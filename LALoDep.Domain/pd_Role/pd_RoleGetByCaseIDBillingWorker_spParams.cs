using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetByCaseIDBillingWorker_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_RoleGetByCaseIDBillingWorker_spResult
    {
        public int RoleTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public string DisplayName { get; set; }
        public int? IsCurrentUserFlag { get; set; }
        public int? IsAttorneyFlag { get; set; }
        public int? IsSupervisorFlag { get; set; }


    }
}
