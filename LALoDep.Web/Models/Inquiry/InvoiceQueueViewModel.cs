using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    public class InvoiceQueueViewModel
    {
        //public string InoviceDate { get; set; }
        //public bool ApproveInvoice { get; set; }
        //public string Status { get; set; }
        //public string Client { get; set; }
        //public string PetitionNumber { get; set; }
        //public string Hearing { get; set; }
        //public string Division { get; set; }
        public int? BranchId { get; set; }
        //public string Branch { get; set; }
        public bool OnViewLoad { get; set; }
        public IEnumerable<SelectListItem> BranchList { get; set; }
    }
}