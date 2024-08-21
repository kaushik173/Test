using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceUpdate_spParams
    {
        public int? InvoiceID { get; set; }
        public int? AgencyID { get; set; }
        public int? ClientRoleID { get; set; }
        public DateTime? InvoiceDateTime { get; set; }
        public int? InvoiceStatusCodeID { get; set; }
        public int? DepartmentID { get; set; }
        public DateTime? InvoiceSentDate { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
