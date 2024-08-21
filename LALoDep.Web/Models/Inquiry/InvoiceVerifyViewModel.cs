using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Inquiry
{
    public class InvoiceVerifyViewModel
    {
        public int InvoiceID { get; set; }

        public string AgencyName { get; set; }
        public string To { get; set; }
        public string From  { get; set; }
        public string Refrence { get; set; }
        public string HHSANumber { get; set; }
        public string JCATSNumber { get; set; }
        public string CaseNumber { get; set; }
        public string Client { get; set; }
        public string Address { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public List<ParentViewModel> ParentList { get; set; }

        //public string ServiceStartDate { get; set; }
        //public string ServiceEndDate { get; set; }
        //public string ServiceType { get; set; }
        //public string Amount { get; set; }

        public List<InvoiceDetails> InvoiceDetails { get; set; }
        public string NoteEntry { get; set; }


    }

    public class ParentViewModel
    {
        public string Parent { get; set; }
        public string Address { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
    }

    public class InvoiceDetails
    {
        public int? HearingID { get; set; }
        public decimal? Amount { get; set; }
        public string ServiceStartDate { get; set; }
        public string ServiceEndDate { get; set; }
        public string ServiceType { get; set; }
    }
}