
namespace LALoDep.Domain.pd_Hearing
{
    public class HearingContinuanceRequestedByGetForHearing_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? HearingID { get; set; }
  public int? UserID { get; set; }

}

	
	public class HearingContinuanceRequestedByGetForHearing_spResult
	{
		public HearingContinuanceRequestedByGetForHearing_spResult()
		{
		}
		public int? CodeID	{ get; set; }
		public string CodeDisplay	{ get; set; }
		public int? Selected	{ get; set; }
		public int? HearingContinuanceRequestedByID	{ get; set; }
	}
}
