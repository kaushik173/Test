using LALoDep.Domain.TitleIVe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class IVEInvoiceViewModel : TitleIVeInvoiceGet_spResult
    {

        public new string ContractorName { get; set; }

        public string ContractorStreetAddress { get; set; }

        public string ContractorCityStateZipCode { get; set; }

        public string ContractorTelephone { get; set; }

        public string SignatureContractor { get; set; }
        public string SignatureContractorTitle { get; set; }
        public string SignatureContractorDate { get; set; }
        public string SignatureContractorFederalIDNumber { get; set; }

        public string SignatureCourt { get; set; }
        public string SignatureCourtTitle { get; set; }
        public string SignatureCourtDate { get; set; }
        public string SignatureCourtJCC { get; set; }
        public string SignatureCourtJCCTitle { get; set; }
        public string SignatureCourtJCCDate { get; set; }

        public DateTime ActivityMonth { get; set; }
        public bool IsUserSignatureExists { get; set; }


        public string AgencySignaturePath { get; set; }
        public string JccSignaturePath { get; set; }
        public string AgencyCountyEncryptedID { get; set; }
        public IEnumerable<SelectListItem> AgencyCountyList { get; set; }



        public string SignatureType { get; set; }
        public decimal CAEligibilityRate { get; set; }
    }
    public class IVEProfessionalServicesViewModel
    {
        public int InvoiceID { get; set; }
        public string ContractorName { get; set; }

        public string ContractorStreetAddress { get; set; }

        public string ContractorCityStateZipCode { get; set; }

        public string ContractorTelephone { get; set; }
        public string HeaderInvoiceID { get; set; }

        public string CourtSystem { get; set; }
        public string InvoicePeriod { get; set; }
        public string InvoiceDate { get; set; }
        public List<TitleIVeProfessionalServiceGet_spResult> TitleIVeProfessionalServiceList { get; set; }
        public List<SelectListItem> TypeOfServiceList { get; set; }
        public List<SelectListItem> SpecifyUnitList { get; set; }
        public decimal? TotalCACFundAmount { get; set; }
        public decimal? TotalFFDRPFundAmount { get; set; }
        public decimal? TotalEligibleCost { get; set; }
        public IVEProfessionalServicesViewModel()
        {
            TitleIVeProfessionalServiceList = new List<TitleIVeProfessionalServiceGet_spResult>();
            SpecifyUnitList = new List<SelectListItem>();
            TypeOfServiceList = new List<SelectListItem>();
        }

        public decimal? ProviderOverheadRate { get; set; }
    }

    public class TitleIVeProfessionalServiceAddEditModel
    {

        public int? TitleIVeProfessionalServiceID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public DateTime ServiceDate { get; set; }
        public string InvoiceReferenceNbr { get; set; }
        public int? TypeOfServiceID { get; set; }
        public string SubcontractorBusinessName { get; set; }
        public decimal NumberOfUnits { get; set; }
        public int? SpecifyUnitID { get; set; }
        public decimal UnitRate { get; set; }
        public decimal PercentDependency { get; set; }
        public decimal PercentCACFunds { get; set; }
        public decimal PercentFFDRPFunds { get; set; }
        public decimal CACFundAmount { get; set; }
        public decimal FFDRPFundAmount { get; set; }
        public decimal EligibleCost { get; set; }
        public string Note { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public short? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }

        public decimal? EligibleCaseSpecific { get; set; }
        public decimal? EligibleAdmin { get; set; }
        public decimal? EligibleAttorneyTraining { get; set; }
        public decimal? EligibleNonAttorneyTraining { get; set; }
        public decimal? EligibleAmountCaseSpecific { get; set; }
        public decimal? EligibleAmountAdmin { get; set; }
        public decimal? EligibleAmountAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonAttorneyTraining { get; set; }

    }
    public class IVEPersonnelViewModel
    {
        public decimal? ProviderOverheadRate { get; set; }
        public int InvoiceID { get; set; }
        public string HeaderInvoiceID { get; set; }

        public string MessageToDisplay { get; set; }
        public string CourtSystem { get; set; }
        public string InvoicePeriod { get; set; }
        public string InvoiceDate { get; set; }
        public List<TitleIVePersonnelGetListViewModel> TitleIVePersonnelList { get; set; }
        public List<SelectListItem> OHCodeList { get; set; }

        public IVEPersonnelViewModel()
        {
            TitleIVePersonnelList = new List<TitleIVePersonnelGetListViewModel>();
        }

    }
    public class TitleIVePersonnelGetListViewModel : TitleIVePersonnelGet_spResult
    {

        public List<SelectListItem> OHCodeList { get; set; }



    }

    public class IVEPersonnelAddEditModel
    {
        public int? TitleIVePersonnelID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public int? EmployeePersonID { get; set; }
        public decimal MonthlySalaryAndBenefits { get; set; }
        public decimal PercentDependency { get; set; }
        public decimal PercentCACFunds { get; set; }
        public decimal PercentFFDRPFunds { get; set; }
        public decimal CACFundAmount { get; set; }
        public decimal FFDRPFundAmount { get; set; }
        public decimal EligibleCost { get; set; }
        public string Note { get; set; }
        public int? OHCode { get; set; }
        public int? FromTitleIveActivityLogID { get; set; }

        public string EmployeeName { get; set; }
        public string EmployeeTitle { get; set; }
        public decimal? CountyProgramPercent { get; set; }
        public decimal? EligibleCaseSpecific { get; set; }
        public decimal? EligibleAdmin { get; set; }
        public decimal? EligibleAttorneyTraining { get; set; }
        public decimal? EligibleNonAttorneyTraining { get; set; }
        public decimal? EligibleAmountCaseSpecific { get; set; }
        public decimal? EligibleAmountAdmin { get; set; }
        public decimal? EligibleAmountAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonAttorneyTraining { get; set; }
        public int? DeleteFlag { get; set; }
    }
    public class IVEClaimFormViewModel
    {
        public int InvoiceID { get; set; }
        public string HeaderInvoiceID { get; set; }

        public string CourtSystem { get; set; }
        public string InvoicePeriod { get; set; }
        public string InvoiceDate { get; set; }
        public TitleIVeClaimFormGet_spResult TitleIVeClaimForm { get; set; }

        public string StandardAgreementNumber { get; set; }
        public IVEClaimFormViewModel()
        {
            TitleIVeClaimForm = new TitleIVeClaimFormGet_spResult();
        }

    }
    public class IVEAttachmentsViewModel
    {

        public IVEAttachmentsViewModel()
        {

        }
        public string TitleIVeInvoiceID { get; set; }

        public string DocumentType { get; set; }
        public string TitleIVeItemID { get; set; }

        public string AttachFileMaxSize { get; set; }


        public string AttachFileTypes { get; set; }



    }
    public class IVeInvoiceStatusViewModel
    {
        public int InvoiceID { get; set; }
        public string HeaderInvoiceID { get; set; }

        public string CourtSystem { get; set; }
        public string InvoicePeriod { get; set; }
        public string InvoiceDate { get; set; }

        public string AgencyCountyEncryptedID { get; set; }
        public IEnumerable<SelectListItem> AgencyCountyList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<TitleIVePendingInvoiceGet_spResult> TitleIVeInvoiceStatusList { get; set; }

        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public int? AgencyCountyID { get; set; }
        public string AgencyCounty { get; set; }
        public DateTime ActivityMonth { get; set; }
        public string EnumStatusCode { get; set; }
        public IVeInvoiceStatusViewModel()
        {
            AgencyCountyList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>();
            TitleIVeInvoiceStatusList = new List<TitleIVePendingInvoiceGet_spResult>();
        }

    }
}