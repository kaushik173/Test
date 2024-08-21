using LALoDep.Domain.NgInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Administration
{
    public class ImportPaidInvoicesViewModel
    {
        public string ErrorMessage { get; set; }
        public string FileName { get; set; }
        public List<NgInvoiceImport_ProcessRecords_spResult> NgInvoiceImportProcessRecords { get; set; }
        public List<NgInvoiceImportIUD_spParams> NgInvoiceImportIUDList { get; set; }

        

        public ImportPaidInvoicesViewModel()
        {
            NgInvoiceImportProcessRecords = new List<NgInvoiceImport_ProcessRecords_spResult>();
            NgInvoiceImportIUDList = new List<NgInvoiceImportIUD_spParams>();
        }
    }
}