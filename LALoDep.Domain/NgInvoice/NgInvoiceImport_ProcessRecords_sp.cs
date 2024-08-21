using System;

namespace LALoDep.Domain.NgInvoice
{
    public class NgInvoiceImport_ProcessRecords_spParams
    {
        public string NgInvoiceImportBatchGUID { get; set; }
        public string NgInvoiceImportFileName { get; set; }
        public DateTime? NgInvoiceImportStagedOn { get; set; }
        public int? UserID { get; set; }
        public int? DebugOnly { get; set; }

    }


    public class NgInvoiceImport_ProcessRecords_spResult
    {
        public NgInvoiceImport_ProcessRecords_spResult()
        {
        }
        public int? Total { get; set; }
        public string Description { get; set; }
    }
}
