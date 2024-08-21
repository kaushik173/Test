using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleAgencyInvestigatorGetByUserGroup_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleAgencyInvestigatorGetByUserGroup_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameDisplay { get; set; }
    } 
}
