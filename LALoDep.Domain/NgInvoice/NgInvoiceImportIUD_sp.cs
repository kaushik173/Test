
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.NgInvoice
{

    public class NgInvoiceImportIUD_spParams
    {
        public string IUD { get; set; }
        public int? NgInvoiceImportID { get; set; }
        public string NgInvoiceImportBatchGUID { get; set; }
        public string NgInvoiceImportFileName { get; set; }
        public DateTime? NgInvoiceImportStagedOn { get; set; }
        public DateTime? NgInvoiceImportProcessedOn { get; set; }
        public int? UserID { get; set; }
        public string DocumentNumber { get; set; }
        public string VendorName { get; set; }
        public string Credit { get; set; }
        public string Debit { get; set; }
        public string Order { get; set; }
        public string DocumentHeaderText { get; set; }
        public string ItemText { get; set; }
        public string DocumentDate { get; set; }
        public string UserName { get; set; }
        public string JournalInvoiceNbr { get; set; }
        public string CostCenter { get; set; }
        public string WBSelement { get; set; }
        public string GLAccount { get; set; }
        public string Fund { get; set; }

    }

}