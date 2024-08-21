using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetByCaseIDClient_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? ReferralID { get; set; }
    }

    public class pd_RoleGetByCaseIDClient_spResult
    {
        public int? RoleID { get; set; }
        public int? CaseID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public int? AgencyID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string Role { get; set; }
        public byte? RoleClient { get; set; }
        public int? AllMainPetitionsClosedFlag { get; set; }
        public string AllMainPetitionsDisplay { get; set; }
    }





   
}
