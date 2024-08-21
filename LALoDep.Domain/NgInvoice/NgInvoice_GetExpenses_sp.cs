namespace LALoDep.Domain.NgInvoice
{
public class NgInvoice_GetExpenses_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? UserID { get; set; }

}

	
	public class NgInvoice_GetExpenses_spResult
	{
		public NgInvoice_GetExpenses_spResult()
		{
		}
        public int? NgInvoiceDetailID { get; set; }
        public int? NgInvoiceID { get; set; }
        public int? ExpenseID { get; set; }
        public int? ExpenseStatusID { get; set; }
        public int? ExpenseStatusCodeID { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public int? CanNotProceedFlag { get; set; }
        public string StatusDate { get; set; }
        public decimal? Amount { get; set; }
        public string IVe { get; set; }
        public decimal? PreviousAmount { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }
        public System.Nullable<System.DateTime> SortDate { get; set; }
    }
}
