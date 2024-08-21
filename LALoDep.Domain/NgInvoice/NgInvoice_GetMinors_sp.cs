namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetMinors_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetMinors_spResult
	{
		public NgInvoice_GetMinors_spResult()
		{
		}
		public int? NgInvoiceMinorID	{ get; set; }
		public int? NgInvoiceID	{ get; set; }
		public int? RoleID	{ get; set; }
		public string MinorName	{ get; set; }
		public int? TypeCodeID	{ get; set; }
		public int? StateCodeID	{ get; set; }
		public int? AgencyCountyID	{ get; set; }
        public string DOBAge { get; set; }
    }
}
