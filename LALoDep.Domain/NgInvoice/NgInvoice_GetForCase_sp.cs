namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoice_GetForCase_spParams
    {
        public int? CaseID { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }

    }


    public class NgInvoice_GetForCase_spResult
    {
        public NgInvoice_GetForCase_spResult()
        {
        }
        public string ButtonLabel { get; set; }
        public int? NgInvoiceID { get; set; }
        public string GroupingBy { get; set; }
        public string YearQuarter { get; set; }
        public int? YearQuarterID { get; set; }
        public int? ContractorPersonID { get; set; }
        public string ContractorDisplay { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceStatusDate { get; set; }
        public System.Nullable<System.DateTime> Date { get; set; }
        public string Phase { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string InCourtTime { get; set; }
        public int? TypeAlertFlag { get; set; }
        public string Hours { get; set; }
        public string HourlyRate { get; set; }
        public decimal? Amount { get; set; }
        public string IVe { get; set; }
        public decimal? PreviousAmount { get; set; }
        public string Status { get; set; }
        public string StatusDate { get; set; }
        public string Note { get; set; }
        public string AdminNote { get; set; }
        public string PrintInvoiceLabel { get; set; }
    }
}
