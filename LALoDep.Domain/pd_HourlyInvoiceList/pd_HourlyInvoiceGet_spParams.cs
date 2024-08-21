using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HourlyInvoiceList
{
    public class pd_HourlyInvoiceGet_spParams
    {
        public int HourlyInvoiceID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }

    public class pd_HourlyInvoiceGet_spResult
    {
        public int HourlyInvoiceID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? HourlyInvoiceStatusCodeID { get; set; }
        public DateTime? HourlyInvoiceStatusDate { get; set; }
        public int? HourlyInvoiceStatusByPersonID { get; set; }
        public decimal? HourlyInvoiceCourtApprovalAmount { get; set; }
        public DateTime? HourlyInvoiceCourtApprovalDate { get; set; }
        public int? HourlyInvoiceCourtApprovalByPersonID { get; set; }        
        public short? RecordStateID { get; set; }        
        public string HourlyInvoicePersonName { get; set; }
        public string HourlyInvoiceStatus { get; set; }
        public string HourlyInvoiceStatusPersonName { get; set; }
        public decimal? TotalInvoiceAmount { get; set; }

    }
}
