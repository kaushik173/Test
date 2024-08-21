using System;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeInvoiceGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public int? UserID { get; set; }
       



    }


    public class TitleIVeInvoiceGet_spResult
    {
       
        public TitleIVeInvoiceGet_spResult()
        {
        }
        public int? TitleIVeInvoiceID { get; set; }
        public string DisplayInvoiceID { get; set; }
        public int? AgencyGroupID { get; set; }
        public string AgencyGroup { get; set; }
        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public int? AgencyCountyID { get; set; }
        public string AgencyCounty { get; set; }
        public string AgreementNumber { get; set; }
        public int? ContractdirectlyWithCourt { get; set; }
        public int? InvoiceID { get; set; }
        public System.Nullable<System.DateTime> InvoiceDate { get; set; }
        public string InvoicePeriod { get; set; }
        public decimal? Personnel { get; set; }
        public decimal? ProfessionalServices { get; set; }
        public decimal? OperationalExpenses { get; set; }
        public decimal? TravelExpenses { get; set; }
        public decimal? TotalExpenses { get; set; }
        public decimal? CaliforniaEligibilityAmount { get; set; }
        public decimal? IVeReimburseAmount { get; set; }
        public decimal? AmountDue { get; set; }
        public int? ContractorSigningPersonID { get; set; }
        public string ContractorName { get; set; }
        public string ContractTitle { get; set; }
        public System.Nullable<System.DateTime> ContractorSigningDate { get; set; }
        public string CourtContractAgreementNbr { get; set; }
        public string FederalIDNumber { get; set; }
        public string CourtSigningPerson { get; set; }
        public string CourtSigningTitle { get; set; }
        public System.Nullable<System.DateTime> CourtSigningDate { get; set; }
        public int? JCCSigningPersonID { get; set; }
        public string JCCPersonName { get; set; }
        public string JCCTitle { get; set; }
        public System.Nullable<System.DateTime> JCCSigningDate { get; set; }
        public int? ReadOnly { get; set; }
        public int? ShowAgencySignatureButton { get; set; }
        public int? ShowJCCSignatureButton { get; set; }
        public string PersonnelUploadButton { get; set; }
        public string ProfServiceUploadButton { get; set; }
        public string OpExpenseUploadButton { get; set; }
        public string TravelUploadButton { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public decimal? NonAttorneyTrainingAmount { get; set; }
        public decimal? NonAttorneyTrainingEligibilityAmount { get; set; }
        public decimal? NonAttorneyTrainingReimbursement { get; set; }
        public decimal? AttorneyTrainingAmount { get; set; }
        public decimal? AttorneyTrainingEligibilityAmount { get; set; }
        public decimal? AttorneyTrainingReimbursement { get; set; }
        public string StandardAgreementNbr { get; set; }
    }

    public class TitleIVeProfessionalServiceGet_spResult
    {
        public decimal? OverHeadRate { get; set; }
        public int? ReadOnly { get; set; }
        public int? TitleIVeProfessionalServiceID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string InvoiceReferenceNbr { get; set; }
        public int? TypeOfServiceID { get; set; }
        public string SubcontractorBusinessName { get; set; }
        public decimal? NumberOfUnits { get; set; }
        public int? SpecifyUnitID { get; set; }
        public decimal? UnitRate { get; set; }
        public decimal? PercentDependency { get; set; }
        public decimal? PercentCACFunds { get; set; }
        public decimal? PercentFFDRPFunds { get; set; }
        public decimal? CACFundAmount { get; set; }
        public decimal? FFDRPFundAmount { get; set; }
        public decimal? EligibleCost { get; set; }
        public string Note { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public short? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
        public string UploadButton { get; set; }
        public decimal? EligibleCaseSpecific { get; set; }
        public decimal? EligibleAdmin { get; set; }
        public decimal? EligibleAttorneyTraining { get; set; }
        public decimal? EligibleNonAttorneyTraining { get; set; }
        public decimal? EligibleAmountCaseSpecific { get; set; }
        public decimal? EligibleAmountAdmin { get; set; }
        public decimal? EligibleAmountAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonAttorneyTraining { get; set; }

    }
    public class TitleIVeProfessionalServiceGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public int UserID { get; set; }

    }
    
}
