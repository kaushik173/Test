namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetForCaseFilter_spParams {
  public int? CaseID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetForCaseFilter_spResult
	{
		public NgInvoice_GetForCaseFilter_spResult()
		{
		}
		public string FilterByDisplay	{ get; set; }
		public string FilterByEnumName	{ get; set; }
		public int? Selected	{ get; set; }
		public int? SortOrder	{ get; set; }
	}
}
