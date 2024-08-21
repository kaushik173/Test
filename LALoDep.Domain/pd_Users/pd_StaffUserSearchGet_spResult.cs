using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users
{
    public class pd_StaffUserSearchGet_spResult
    {
        public int? JcatsUserID { get; set; }
        public int? PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsGroupName { get; set; }
        public int? JcatsGroupID { get; set; }
        public string RoleType { get; set; }
        public string AgencyName { get; set; }
        public string UserLevel { get; set; }
        public string LastLoginDate { get; set; }
        public string UserEndDate { get; set; }
    }
}
