namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralGet602Petitions_spParams {
  public int? CaseID { get; set; }
  public int? ReferralID { get; set; }
  public int? RoleID { get; set; }
  public int? ReferralTypeCodeID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class ref_ReferralGet602Petitions_spResult
	{
		public ref_ReferralGet602Petitions_spResult()
		{
		}
		public string FileDate	{ get; set; }
		public string CloseDate	{ get; set; }
		public string MostRecentHearing	{ get; set; }
		public string NextHearing	{ get; set; }
		public string SortDate	{ get; set; }
	}
}
