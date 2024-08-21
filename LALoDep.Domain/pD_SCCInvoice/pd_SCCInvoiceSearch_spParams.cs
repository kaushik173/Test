using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
    public class pd_SCCInvoiceSearch_spParams
    {
        public int PersonID { get; set; }
        public int AgencyCountyID { get; set; }
        public int? SCCInvoiceID { get; set; }
        public int SCCInvoiceStatusCodeID { get; set; }
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public int? CaseID { get; set; }
        public string CourtNumber { get; set; }
        public string SCCInvoicePaidDateStart { get; set; }
        public string SCCInvoicePaidDateEnd { get; set; }
        public int ReferralSourceCodeID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
