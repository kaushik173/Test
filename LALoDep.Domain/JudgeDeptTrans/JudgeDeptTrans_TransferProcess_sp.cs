
namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_TransferProcess_spParams {
  public string TransferGUID { get; set; }
  public int? UpdateToJudgePersonID { get; set; }
  public int? UpdateToDeptCodeID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class JudgeDeptTrans_TransferProcess_spResult
	{
		public JudgeDeptTrans_TransferProcess_spResult()
		{
		}
		public string Status	{ get; set; }
	}
}
