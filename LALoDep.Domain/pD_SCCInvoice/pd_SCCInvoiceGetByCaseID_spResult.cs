using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
    public class pd_SCCInvoiceGetByCaseID_spResult
    {
        public int? SCCInvoiceID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? SCCInvoiceSubmittedByPersonID { get; set; }
        public string SCCInvoiceDateSubmitted { get; set; }
        public int? SCCInvoiceRateID { get; set; }
        public int? SCCInvoiceStatusCodeID { get; set; }
        public string SCCInvoiceStatusDate { get; set; }
        public int? SCCInvoiceStatusByPersonID { get; set; }
        public int? SCCInvoiceDepartmentCodeID { get; set; }
        public string SCCInvoiceNextHearingDate { get; set; }
        public string SCCInvoicePetitionFileDate { get; set; }
        public string SCCInvoiceAppointmentDate { get; set; }
        public string SCCInvoiceServiceHearingDate { get; set; }
        public string SCCInvoiceFirstRPPDate { get; set; }
        public string SCCInvoiceReliefDate { get; set; }
        public string SCCInvoicePaidDate { get; set; }
        public string SCCInvoiceNote { get; set; }
        public string CourtNumber { get; set; }
        public int? ReferralSourceCodeID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public string AttorneyFirstName { get; set; }
        public string AttorneyLastName { get; set; }
        public string AttorneyPhoneNumber { get; set; }
        public string AttorneyBarNumber { get; set; }
        public string InvoiceRateType { get; set; }
        public string InvoiceStatus { get; set; }
        public string ClientList { get; set; }
        public string InvoiceAmount { get; set; }
        public string AttorneyName { get; set; }
    }
}
