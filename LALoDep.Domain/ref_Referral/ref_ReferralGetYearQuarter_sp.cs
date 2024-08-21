namespace LALoDep.Domain.ref_Referral
{
public class ref_ReferralGetYearQuarter_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? ReferralID { get; set; }
  public string LoadOption { get; set; }
  public int? UserID { get; set; }

}

	
	public class ref_ReferralGetYearQuarter_spResult
	{
		public ref_ReferralGetYearQuarter_spResult()
		{
		}
		public string YearQuarter	{ get; set; }
		public int? YearQuarterID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
