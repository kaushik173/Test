namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetCounselHistory_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetCounselHistory_spResult
	{
		public NgInvoice_GetCounselHistory_spResult()
		{
		}
		public int? NgInvoiceCounselID	{ get; set; }
		public int? NgInvoiceID	{ get; set; }
		public int? NgInvoiceCounselTypeCodeID	{ get; set; }
		public int? NgInvoiceCounselPersonID	{ get; set; }
	}
}
