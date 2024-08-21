namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralQueue_spParams
    {
        public int? PersonID { get; set; }
        public System.DateTime? StartDate { get; set; }
        public System.DateTime? EndDate { get; set; }
        public int? DateRangeTypeCodeID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int?  IncludeCompletedReferrals { get; set; }
    }

    public class ref_ReferralQueue_spResult
    {
        public string CurrentAttorney { get; set; }
        public string RequestedFor { get; set; }
        public string ReferralType { get; set; }
        public string NG_NavigationURL { get; set; }
        public string RequestDate { get; set; }
        public string Eligibity { get; set; }
        public string DueDate { get; set; }
        public string CompleteDate { get; set; }
        public string Client { get; set; }
        public string SortDate { get; set; }
        public int? ReferralID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string SubmittedCaseNbr { get; set; }


    }
}
