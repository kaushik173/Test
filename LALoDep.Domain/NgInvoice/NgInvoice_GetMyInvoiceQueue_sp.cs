namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoice_GetMyInvoiceQueue_spParams
    {
        public int? PersonID { get; set; }
        public int? YearQuarterID { get; set; }
        public int? InvoiceStatusCodeID { get; set; }
        public string InvoiceNumber { get; set; }
        public int? NotInvoicedFlag { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }
        public string DocNumber { get; set; }

    }


    public class NgInvoice_GetMyInvoiceQueue_spResult
    {
        public NgInvoice_GetMyInvoiceQueue_spResult()
        {
        }
        public string SectionHeader { get; set; }
        public int? YearQuarterID { get; set; }
        public int? ContractorPersonID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string CaseName { get; set; }
        public string CaseNumber { get; set; }
        public string ExpenseTotal { get; set; }
        public string TotalNumberOfHours { get; set; }
        public string TotalHoursAmount { get; set; }
        public string GrandTotal { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceStatsuDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDialog { get; set; }
        public int? NgInvoiceID { get; set; }
        public int? Sorting { get; set; }

        public string EncryptedCaseID { get; set; }

        public string EncryptedInvoiceID { get; set; }

        public string EncryptedYearQuarterID { get; set; }

        public string EncryptedContractorPersonID { get; set; }
        public string PrintInvoiceLabel { get; set; }
    }
}
