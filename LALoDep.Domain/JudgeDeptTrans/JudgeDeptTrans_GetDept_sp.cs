
namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_GetDept_spParams {
  public int? AgencyGroupID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class JudgeDeptTrans_GetDept_spResult
	{
		public JudgeDeptTrans_GetDept_spResult()
		{
		}
		public string DeptDisplay	{ get; set; }
		public int? DeptCodeID	{ get; set; }
	}
}
