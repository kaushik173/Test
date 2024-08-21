using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
    public class pd_SCCInvoiceGet_spParams
    {
        public int SCCInvoiceID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class pd_SCCInvoiceUpdateOrInsert_spParams
    {
        public int SCCInvoiceID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }

        public int SCCInvoiceSubmittedByPersonID { get; set; }
        public DateTime? SCCInvoiceDateSubmitted { get; set; }
        public int? SCCInvoiceRateID { get; set; }
        public int? SCCInvoiceStatusCodeID { get; set; }
        public DateTime? SCCInvoiceStatusDate { get; set; }
        public int? SCCInvoiceStatusByPersonID { get; set; }
        public int? SCCInvoiceDepartmentCodeID { get; set; }
        public DateTime? SCCInvoiceNextHearingDate { get; set; }
        public DateTime? SCCInvoicePetitionFileDate { get; set; }
        public DateTime? SCCInvoiceAppointmentDate { get; set; }
        public DateTime? SCCInvoiceServiceHearingDate { get; set; }
        public DateTime? SCCInvoiceFirstRPPDate { get; set; }
        public DateTime? SCCInvoiceReliefDate { get; set; }
        public DateTime? SCCInvoicePaidDate { get; set; }
        public string SCCInvoiceNote { get; set; }
        public string CourtNumber { get; set; }
        public int? ReferralSourceCodeID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public string AttorneyFirstName { get; set; }
        public string AttorneyLastName { get; set; }
        public string AttorneyPhoneNumber { get; set; }
        public string AttorneySSNTaxID { get; set; }
        public string AttorneyBarNumber { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_SCCInvoiceClientDelete_spParams
    {
        public int ID { get; set; }
        public decimal? RecordTimeStamp { get; set; }

        public short RecordStateID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_SCCInvoiceClientInsert_spParams
    {
        public int SCCInvoiceClientID { get; set; }
        public int SCCInvoiceID { get; set; }
        public int  SCCInvoiceClientRoleID { get; set; }
        public int  SCCInvoiceClientRoleTypeCodeID { get; set; }
        public string  SCCInvoiceClientName { get; set; }

        public decimal? RecordTimeStamp { get; set; }

        public int RecordStateID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }


}
