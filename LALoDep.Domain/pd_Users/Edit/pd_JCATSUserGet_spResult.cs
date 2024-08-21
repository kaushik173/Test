using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users.Edit
{
    public class pd_JCATSUserGet_spResult
    {
        public int JcatsUserID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public int JcatsGroupID { get; set; }
        public string JcatsUserPassword { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsUserStartDate { get; set; }
        public string JcatsUserEndDate { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? PersonNameSuffixCodeID { get; set; }
        public int? PersonNameSalutationCodeID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? JcatsUserLevelCodeID { get; set; }
        public int? PersonNameID { get; set; }
       
        public string JcatsUserLevelCodeEnumName { get; set; }

    }
}
