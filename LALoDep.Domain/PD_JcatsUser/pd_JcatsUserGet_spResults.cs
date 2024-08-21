using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_JcatsUser
{
    public class pd_JcatsUserGet_spResults
    {
        public int JcatsUserID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsUserPassword { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string JcatsEmailPersonContactInfo { get; set; }
        public Int16 RecordStateID { get; set; }
        public int JcatsEmailPersonContactID { get; set; }
        public int JcatsEmailPersonAttributeID { get; set; }
        public int JcatsEmailPersonContactTypeCodeID { get; set; }
        public int StaffID { get; set; }
        public byte SystemAdminFlag { get; set; }
        public int JcatsUserGroupID { get; set; }
        public int JcatsGroupID { get; set; }
        public string JcatsUserStartDate { get; set; }
        public string Email { get; set; }

        public string JcatsUserEndDate { get; set; }

        public int? JcatsUserTimeOut { get; set; }
    }
}
