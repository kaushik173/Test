namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoice_GetTotals_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? YearQuarterID { get; set; }
        public int? ContractorPersonID { get; set; }
        public int? NgInvoiceID { get; set; }
        public int? UserID { get; set; }

    }


    public class NgInvoice_GetTotals_spResult
    {
       
        public string RecordTimeAmount { get; set; }
        public string ExpenseAmount { get; set; }
        public string TotalMiles { get; set; }
        public string MileageAmount { get; set; }
        public string TotalBilled { get; set; }
        public string BarFeeAmount { get; set; }
        public string TotalDue { get; set; }
    }
}
