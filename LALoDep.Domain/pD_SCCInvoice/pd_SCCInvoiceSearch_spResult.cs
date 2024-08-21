using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
    public class pd_SCCInvoiceSearch_spResult
    {
        public string Attorney { get; set; }
        public string Client { get; set; }
        public string CaseNumber { get; set; }
        public string InvoiceRateType { get; set; }
        public string InvoiceAmount { get; set; }
        public string InvoiceStatus { get; set; }
        public string InvoiceDate { get; set; }
        public int InvoiceNumber { get; set; }
        public string CourtNumber { get; set; }
        public string NextHearingDate { get; set; }
        public string InvoicePaidDate { get; set; }
        public int ReferralSourceCodeID { get; set; }
        public string ReferralSource{ get; set; }
        public int SCCInvoiceID { get; set; }
        public int AgencyCountyID { get; set; }
        public int AttorneyPersonID { get; set; }
        public int CaseID { get; set; }
        public int SortID { get; set; }
    }
}
