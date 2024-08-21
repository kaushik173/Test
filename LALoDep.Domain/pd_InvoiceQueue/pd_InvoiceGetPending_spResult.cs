using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceGetPending_spResult
    {
        public int ID { get; set; }
        public int JcatsUserID { get; set; }
        public DateTime InsertedOn { get; set; }
        public int CaseID { get; set; }
        public int InvoiceID { get; set; }
        public string SortInvoiceDate { get; set; }
        public string InvoiceDate { get; set; }
        public string Client { get; set; }
        public int BranchCodeID { get; set; }
        public string Branch { get; set; }
        public string Status { get; set; }
        public int AgencyID { get; set; }
        public string Division { get; set; }
        public string Petitions { get; set; }
        public string Hearings { get; set; }
    }
}
