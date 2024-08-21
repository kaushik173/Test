using LALoDep.Domain.TitleIVe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class IVETravelExpensesViewModel
    {
        public int InvoiceID { get; set; }
        public string ContractorName { get; set; }
        public string ContractorStreetAddress { get; set; }
        public string ContractorCityStateZipCode { get; set; }
        public string ContractorTelephone { get; set; }
        public string CourtSystem { get; set; }
        public string InvoicePeriod { get; set; }
        public string InvoiceDate { get; set; }
        public decimal? TotalCACFundAmount { get; set; }
        public decimal? TotalFFDRPFundAmount { get; set; }
        public decimal? TotalEligibleCost { get; set; }
        public List<TitleIVeTravelExpensesGet_spResult> TitleIVeTravelExpensesList { get; set; }
        public List<SelectListItem> SpecifyUnitList { get; set; }
        public string HeaderInvoiceID { get; set; }

        public IVETravelExpensesViewModel()
        {
            TitleIVeTravelExpensesList = new List<TitleIVeTravelExpensesGet_spResult>();
        }

    }
    public class TitleIVETravelExpensesAddEditModel
    {
        public int? TitleIVeTravelExpenseID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public DateTime TravelDate { get; set; }
        public string CaseNbr { get; set; }
        public string TravelerName { get; set; }
    public string PurposeOfTravel { get; set; }
        public string TypeOfTravelExpense { get; set; }
        public string SubcontractorBusinessName { get; set; }
        public decimal NumberOfUnits { get; set; }
        public int? SpecifyUnitID { get; set; }
        public decimal UnitRate { get; set; }
       // public decimal PercentDependency { get; set; }
        public decimal PercentCACFunds { get; set; }
        public decimal PercentFFDRPFunds { get; set; }
        public decimal CACFundAmount { get; set; }
        public decimal FFDRPFundAmount { get; set; }
       // public decimal EligibleCost { get; set; }
        public string Note { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public short? RecordStateID { get; set; }
        public string FromZipcode { get; set; }
        public string ToZipcode { get; set; }
        public decimal? EligibleNonTraining { get; set; }
        public decimal? EligibleAttorneyTraining { get; set; }
        public decimal? EligibleNonAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonTraining { get; set; }
        public decimal? EligibleAmountAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonAttorneyTraining { get; set; }
    }
}