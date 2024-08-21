
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeInvoiceInsertRemoveAgencySignature_Params
    {


        public int? ErrorID { get; set; }
        public int? UserID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
    }

    public class TitleIVeInvoiceInsertUpdate_spParams
    {


        public int? ErrorID { get; set; }
        public int? UserID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public int? AgencyCountyID { get; set; }
        public string AgreementNumber { get; set; }
        public int? ContractdirectlyWithCourt { get; set; }
        public string InvoiceID { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? Personnel { get; set; }
        public decimal? ProfessionalServices { get; set; }
        public decimal? OperationalExpenses { get; set; }
        public decimal? TravelExpenses { get; set; }
        public decimal? TotalExpenses { get; set; }
        public decimal? CaliforniaEligibilityAmount { get; set; }
        public decimal? IVeReimburseAmount { get; set; }
        public decimal? AmountDue { get; set; }
        public int? ContractorSigningPersonID { get; set; }
        public DateTime? ContractorSigningDate { get; set; }
        public string CourtContractAgreementNbr { get; set; }
        public string CourtSigningPerson { get; set; }
        public string CourtSigningTitle { get; set; }
        public DateTime? CourtSigningDate { get; set; }
        public int? JCCSigningPersonID { get; set; }
        public DateTime? JCCSigningDate { get; set; }
    }
    public class TitleIVeInvoiceSignatureUpdate_spParams
    {
        public int? ErrorID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public string SignatureType { get; set; }
        public string CourtSigningPersonName { get; set; }
        public DateTime? CourtSigningDate { get; set; }
        public string CourtSigningTitle { get; set; }
        public int? JCCSigningPersonID { get; set; }
        public DateTime? JCCSigningDate { get; set; }
        public int? UserID { get; set; }

    }
}