
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.TitleIVe
{

    public class TitleIVeInvoiceSupportingDocInsertDelete_spParams
    {
        public int? ErrorID { get; set; }
        public int? TitleIVeInvoiceSupportingDocID { get; set; }
        public string DocumentName { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
        public int? InvoiceID { get; set; }
        public string DocumentType { get; set; }
        public int? TitleIVeItemID { get; set; }
        public string Delete { get; set; }
        public int? UserID { get; set; }

    }

}