namespace LALoDep.Domain.RTNC
{

    public class RTNC_Supervisor_spParams {
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class RTNC_Supervisor_spResult
	{
		public RTNC_Supervisor_spResult()
		{
		}
		public string PersonDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
