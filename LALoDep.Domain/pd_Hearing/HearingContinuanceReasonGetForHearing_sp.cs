
namespace LALoDep.Domain.pd_Hearing
{
    public class HearingContinuanceReasonGetForHearing_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? HearingID { get; set; }
  public int? UserID { get; set; }

}

	
	public class HearingContinuanceReasonGetForHearing_spResult
	{
		public HearingContinuanceReasonGetForHearing_spResult()
		{
		}
		public int? CodeID	{ get; set; }
		public string CodeDisplay	{ get; set; }
		public int? CommentRequired	{ get; set; }
		public int? Selected	{ get; set; }
		public int? HearingContinuanceReasonID	{ get; set; }
		public string Comment	{ get; set; }
	}
}
