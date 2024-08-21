using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceGetUnsentByInvoiceIDPetition_spParams
    {
        public int UserID { get; set; }
        public int InvoiceID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_InvoiceGetUnsentByInvoiceIDPetition_spResult
    {
        public int ClientID { get; set; }
        public int InvoiceID { get; set; }
        public string DocketNumber { get; set; }
    }
}
