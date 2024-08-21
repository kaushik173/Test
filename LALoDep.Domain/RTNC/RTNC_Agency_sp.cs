namespace LALoDep.Domain.RTNC
{

    public class RTNC_Agency_spParams {
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class RTNC_Agency_spResult
	{
		public RTNC_Agency_spResult()
		{
		}
		public string AgencyDisplay	{ get; set; }
		public int? AgencyID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
