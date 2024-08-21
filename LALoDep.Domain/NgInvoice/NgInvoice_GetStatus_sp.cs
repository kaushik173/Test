namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoice_GetStatus_spParams
    {
        public int? CaseID { get; set; }
        public int? NgInvoiceID { get; set; }
        public int? UserID { get; set; }

    }


    public class NgInvoice_GetStatus_spResult
    {
        public NgInvoice_GetStatus_spResult()
        {
        }
        public string StatusDisplay { get; set; }
        public int? StatusCodeID { get; set; }
        public int? Selected { get; set; }
        public int? DisplayOrder { get; set; }
    }

    public class NgInvoice_GetStatusHistory_spParams
    {
        public int? NgInvoiceID { get; set; }
        public int? UserID { get; set; }

    }


    public class NgInvoice_GetStatusHistory_spResult
    {
        public NgInvoice_GetStatusHistory_spResult()
        {
        }
        public string StatusDate { get; set; }
        public string StatusDisplay { get; set; }
    }
}
