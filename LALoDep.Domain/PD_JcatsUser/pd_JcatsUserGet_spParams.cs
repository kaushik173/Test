using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_JcatsUser
{//EXECUTE dbo.pd_JcatsUserGetByPersonID_sp  24662816,60001700,NULL
    public class pd_JcatsUserGet_spParams
    {
        public int JcatsUserID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_JcatsUserGetByPersonID_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_JcatsUserGetByPersonID_spResult
    {
        public int? JcatsUserID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? JcatsGroupID { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsUserPassword { get; set; }
        public string JcatsUserLoginKey { get; set; }
        public string JcatsUserEMail { get; set; }
        public string JcatsUserStartDate { get; set; }
        public string JcatsUserEndDate { get; set; }
        public System.DateTime? JcatsUserAccessDateTime { get; set; }
        public short? RecordStateID { get; set; }

        public int? PersonNameID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameMiddle { get; set; }
        public int? PersonNameSalutationCodeID { get; set; }
        public int? PersonNameSuffixCodeID { get; set; }
        public System.DateTime? CurrentDateTime { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string AgencyName { get; set; }
        public string Email { get; set; }
        public int? PersonNameTypeCodeID { get; set; }
        public short? PersonNameRecordStateID { get; set; }
        public decimal? PersonNameRecordTimeStamp { get; set; }
        public byte? SystemAdminFlag { get; set; }
        public int? JcatsUserLevelCodeID { get; set; }
        public string JcatsUserLevelCodeEnumName { get; set; }
        public int? JcatsUserLoginAttemptCount { get; set; }
        public int? MaxLoginAttemptFlag { get; set; }
    }
}
