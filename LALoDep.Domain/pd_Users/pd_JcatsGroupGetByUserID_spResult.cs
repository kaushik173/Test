using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users
{
    public class pd_JcatsGroupGetByUserID_spResult
    {
        public int CurrentUserGroupFlag { get; set; }
        public int JcatsGroupID { get; set; }
        public string JcatsGroupName { get; set; }
        public int JcatsGroupUserFlag { get; set; }
        public int? DefaultNavigationID { get; set; }
        public string NavigationDisplayName { get; set; }
        public string NavigationURL { get; set; }
        public int UserCount { get; set; }
    }
}
