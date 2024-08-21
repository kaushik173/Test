namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralGetImmigrationAttorneyList_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? ReferralID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class ref_ReferralGetImmigrationAttorneyList_spResult
	{
		public ref_ReferralGetImmigrationAttorneyList_spResult()
		{
		}
		public string NameDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
		public string Phone	{ get; set; }
		public string Email	{ get; set; }
	}
}
