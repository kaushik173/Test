namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetCounselTypes_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetCounselTypes_spResult
	{
		public NgInvoice_GetCounselTypes_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
	}
}
