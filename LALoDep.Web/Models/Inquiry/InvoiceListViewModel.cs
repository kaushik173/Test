using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Inquiry
{
    public class InvoiceListViewModel
    {
        public int InvoiceID { get; set; }
        public string InvoiceDate { get; set; }
        public string Status { get; set; }
        public int InvoiceStatusCodeID { get; set; }
        public string ClientDisplayName { get; set; }
        public string OtherParties { get; set; }
        public int Void { get; set; }
        public int? NoteID { get; set; }


        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public decimal InvoiceHearingAmount { get; set; }
        public int? HearingID { get; set; }

    }
}