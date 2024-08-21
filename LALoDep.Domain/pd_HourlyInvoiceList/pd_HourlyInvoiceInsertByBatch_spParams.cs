using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HourlyInvoiceList
{
    public class pd_HourlyInvoiceInsertByBatch_spParams
    {   
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? HourlyInvoiceStatusCodeID { get; set; }
        public decimal? HourlyInvoiceCourtApprovalAmount { get; set; }
        public DateTime? HourlyInvoiceCourtApprovalDate { get; set; }
        public Guid BatchGUID { get; set; }
        public string WorkIDList { get; set; }
        public string HourlyExpenseIDList { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
}
