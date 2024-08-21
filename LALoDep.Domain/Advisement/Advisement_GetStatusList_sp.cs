namespace LALoDep.Domain.Advisement
{
public class Advisement_GetStatusList_spParams {
  public int? CaseID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class Advisement_GetStatusList_spResult
	{
		public Advisement_GetStatusList_spResult()
		{
		}
		public string StatusDisplay	{ get; set; }
		public int? StatusCodeID	{ get; set; }
	}
}
