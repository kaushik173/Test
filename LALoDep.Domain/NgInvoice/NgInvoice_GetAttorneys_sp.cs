namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetAttorneys_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetAttorneys_spResult
	{
		public NgInvoice_GetAttorneys_spResult()
		{
		}
		public string PersonDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
	}
}
