namespace LALoDep.Domain.Advisement
{
public class Advisement_GetForSupervisor_spParams {
  public int? SupervisorPersonID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class Advisement_GetForSupervisor_spResult
	{
		public Advisement_GetForSupervisor_spResult()
		{
		}
		public string Attorney	{ get; set; }
		public int? AttorneyPersonID	{ get; set; }
		public int? Total	{ get; set; }
		public int? Due3To5Days	{ get; set; }
		public int? Due1To2Days	{ get; set; }
		public int? DueToday	{ get; set; }
		public int? PastDue	{ get; set; }
	}
}
