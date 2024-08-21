namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetStatusCodes_spParams {
  public int? PersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? AgencyID { get; set; }
  public int? CaseID { get; set; }
  public string LoadOption { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetStatusCodes_spResult
	{
		public NgInvoice_GetStatusCodes_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
		public int? Selected	{ get; set; }
		public string CodeEnumName	{ get; set; }
	}
}
