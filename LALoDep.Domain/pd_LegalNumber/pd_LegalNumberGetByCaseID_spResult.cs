using System;

namespace LALoDep.Domain.pd_LegalNumber
{
    public class pd_LegalNumberGetByCaseID_spParams
    {

        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_LegalNumberGetByCaseID_spResult
    {
        public int LegalNumberID { get; set; }
        public int? LegalNumberTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public string LegalNumberEntry { get; set; }
        public string LegalNumberTypeCodeValue { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string RoleTypeCodeValue { get; set; }
        public byte? RoleClient { get; set; }
    }
}