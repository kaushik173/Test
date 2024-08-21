namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoice_GetInvoiceSummary_spParams
    {
        public int? YearQuarterID { get; set; }
        public int? ContractorPersonID { get; set; }
        public int? PendingInvoicesOnly { get; set; }
        public string LoadOption { get; set; }
        public string SortOption { get; set; }
        public int? UserID { get; set; }
        public int? StatusCodeID { get; set; }
    }


    public class NgInvoice_GetInvoiceSummary_spResult
    {
        public NgInvoice_GetInvoiceSummary_spResult()
        {
        }
        public string YearQuarter { get; set; }
        public string Contractor { get; set; }
        public int? TotalInvoices { get; set; }
        public int? ProcessedInvoices { get; set; }
        public int? PendingInvoices { get; set; }
        public string ExpenseTotal { get; set; }
        public string TotalNumberOfHours { get; set; }
        public string TotalHoursAmount { get; set; }
        public string GrandTotal { get; set; }
        public string AvgHoursPerCase { get; set; }
        public string AvgAmountPerCase { get; set; }
        public string CasesNotInvoiced { get; set; }
        public int? YearQuarterID { get; set; }
        public int? ContractorPersonID { get; set; }
        public string PostInvoiceLabel { get; set; }
        public int? PostInvoiceYearQuarterID { get; set; }
        public int? PostInvoiceStatusCodeID { get; set; }

        public string EncryptedYearQuarterID { get; set; }
        public string EncryptedPostInvoiceYearQuarterID { get; set; }

        public string EncryptedContractorPersonID { get; set; }

    }
}
