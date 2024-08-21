namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetRecordTime_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetRecordTime_spResult
	{
		public NgInvoice_GetRecordTime_spResult()
		{
		}
        public int? NgInvoiceDetailID { get; set; }
        public int? NgInvoiceID { get; set; }
        public int? WorkID { get; set; }
        public decimal? Hours { get; set; }
        public decimal? HourlyRate { get; set; }
        public int? CanEditHourlyRate { get; set; }
        public decimal? Amount { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Phase { get; set; }
        public string InCourtTime { get; set; }
        public string IVe { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }
        public System.Nullable<System.DateTime> SortDate { get; set; }
        public decimal? OrigHours { get; set; }
    }
}
