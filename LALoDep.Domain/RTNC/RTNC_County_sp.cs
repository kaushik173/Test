namespace LALoDep.Domain.RTNC
{
public class RTNC_County_spParams {
  public int? WorkID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class RTNC_County_spResult
	{
		public RTNC_County_spResult()
		{
		}
		public string AgencyCounty	{ get; set; }
		public int? AgencyCountyID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
