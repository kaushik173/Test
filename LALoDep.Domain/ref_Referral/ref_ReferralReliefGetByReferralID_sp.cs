namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralReliefGetByReferralID_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class ref_ReferralReliefGetByReferralID_spResult
    {
        
        public int? ReferralReliefID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralReliefTypeCodeID { get; set; }
        public int? ReferralReliefStatusCodeID { get; set; }
        public string ReferralReliefStatusDate { get; set; }
        public string ReferralReliefNote { get; set; }
        public string SortDate { get; set; }
        public string ReferralReliefPriorityDate { get; set; }
}
}
