namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralClientCategoryGetByReferralID_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralTypeCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class ref_ReferralClientCategoryGetByReferralID_spResult
    {
        
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public int? ReferralClientCategoryID { get; set; }

        //this for model page
        public bool IsChecked { get; set; }
    }
}
