namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetCounty_spParams {
  public int? CaseID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetCounty_spResult
	{
		public NgInvoice_GetCounty_spResult()
		{
		}
		public string AgencyCounty	{ get; set; }
		public int? AgencyCountyID	{ get; set; }
	}
}
