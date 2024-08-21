namespace LALoDep.Domain.ref_Referral
{
public class ref_ReferralGetCodes_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? ReferralID { get; set; }
  public string LoadOption { get; set; }
  public int? UserID { get; set; }

}

	
	public class ref_ReferralGetCodes_spResult
	{
		public ref_ReferralGetCodes_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
