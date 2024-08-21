namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralGetRequestBy_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? ReferralID { get; set; }
  public int? ReferralTypeCodeID { get; set; }
  public int? ReferralRequestedByPersonID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}
   

    public class ref_ReferralGetRequestBy_spResult
	{
		public ref_ReferralGetRequestBy_spResult()
		{
		}
		public string DisplayName	{ get; set; }
		public int? PersonID	{ get; set; }
		public int? Current	{ get; set; }
	}
    public class ref_ReferralGetRequestFor_spResult
    {
      
        public string DisplayName { get; set; }
        public int? PersonID { get; set; }
        public int? Current { get; set; }
    }
    public class ref_ReferralGetRequestFor_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralTypeCodeID { get; set; }
        public int? ReferralRequestedForPersonID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
}
