using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceGetUnsentByInvoiceID_spParams
    {
        public int UserID { get; set; }
        public int InvoiceID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_InvoiceGetUnsentByInvoiceID_spResult
    {

        public int? InvoiceID { get; set; }
        public int? ClientID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public DateTime? PersonDOB { get; set; }
        public int? CaseID { get; set; }
        public string CaseNumber { get; set; }        
        public string SSN { get; set; }
        public string HHSA { get; set; }        
        public string AgencyName { get; set; }
        public DateTime? InvoiceDateTime { get; set; }
        public int? InvoiceStatusCodeID { get; set; }

        public int? DepartmentID { get; set; }
        public DateTime? InvoiceSentDate { get; set; }
        public short? RecordStateID { get; set; }

        public string HomeStreet { get; set; }
        public string HomeCity { get; set; }
        public string HomeState { get; set; }
        public string HomeZipCode { get; set; }
        public string HomeCountry { get; set; }


        public string MailStreet { get; set; }
        public string MailCity { get; set; }
        public string MailState { get; set; }
        public string MailZipCode { get; set; }
        public string MailCountry { get; set; }

    }
}
