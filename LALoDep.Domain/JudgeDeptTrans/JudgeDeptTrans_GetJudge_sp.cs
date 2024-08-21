
namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_GetJudge_spParams {
  public int? AgencyGroupID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class JudgeDeptTrans_GetJudge_spResult
	{
		public JudgeDeptTrans_GetJudge_spResult()
		{
		}
		public string JudgeDisplay	{ get; set; }
		public int? JudgePersonID	{ get; set; }
	}
}
