
namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_GetAttorney_spParams {
  public int? AgencyGroupID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class JudgeDeptTrans_GetAttorney_spResult
	{
		public JudgeDeptTrans_GetAttorney_spResult()
		{
		}
		public string AttorneyDisplay	{ get; set; }
		public int? AttorneyPersonID	{ get; set; }
	}
}
