using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceHearingUpdate_spParams
    {
        public int? InvoiceHearingID { get; set; }
        public int? AgencyID { get; set; }
        public int? InvoiceID { get; set; }
        public int? HearingID { get; set; }
        public DateTime? InvoiceHearingStartDate { get; set; }
        public decimal? InvoiceHearingAmount { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
