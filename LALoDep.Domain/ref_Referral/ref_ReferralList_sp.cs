namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralList_spParams
    {
        public int? CaseID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

    public class ref_ReferralList_spResult
    {
        public int? ReferralID { get; set; }
        public string Client { get; set; }
        public string ReferralType { get; set; }
        public string NG_NavigationURL { get; set; }
        public string RequestDate { get; set; }
        public string CompleteDate { get; set; }
        public string AssignedTo { get; set; }
        public string Eligibility { get; set; }
        public string AttachedFilesDisplay { get; set; }
        public int? CanEditFlag { get; set; }
        public int? CanDeleteFlag { get; set; }
        public string CopiedToDisplay { get; set; }
        public int? CopiedToCaseID { get; set; }
        public int? CopiedToReferralID { get; set; }



    }
}
