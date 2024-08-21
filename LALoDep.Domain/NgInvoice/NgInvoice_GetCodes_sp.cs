namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetCodes_spParams {
  public int? CodeTypeCodeID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? AgencyID { get; set; }
  public int? CaseID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetCodes_spResult
	{
		public NgInvoice_GetCodes_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
		public int? DisplayOrder	{ get; set; }
	}
}
