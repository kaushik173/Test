using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceGetUnsentByInvoiceIDParent_spParams
    {
        public int UserID { get; set; }
        public int InvoiceID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_InvoiceGetUnsentByInvoiceIDParent_spResult
    {

        public int InvoiceID { get; set; }
        public int? ClientID { get; set; }
        public int? PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string Role { get; set; }
        public DateTime? DOB { get; set; }
        public string Mailstreet { get; set; }
        public string Mailcity { get; set; }
        public string Mailstate { get; set; }
        public string MailzipCode { get; set; }
        public string MailCountry { get; set; }
        public string Homestreet { get; set; }
        public string Homecity { get; set; }
        public string Homestate { get; set; }
        public string HomezipCode { get; set; }
        public string HomeCountry { get; set; }
        public string ssn { get; set; }
    }
}
