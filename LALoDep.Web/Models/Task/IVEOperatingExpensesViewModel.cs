using LALoDep.Domain.TitleIVe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Task
{
    public class IVEOperatingExpensesViewModel
    {
        public decimal? ProviderOverheadRate { get; set; }
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
        public decimal? TotalEligibleAmountNonTraining { get; set; }
        public decimal? TotalEligibleAmountAttorneyTraining { get; set; }
        public decimal? TotalEligibleAmountNonAttorneyTraining { get; set; }
        public decimal? TotalOperatingExpensesAmount { get; set; }
        public List<TitleIVeOperatingExpensesGet_spResult> TitleIVeOperatingExpensesList { get; set; }
        public string HeaderInvoiceID { get; set; }
        public string NonTrainingExpenses { get; set; }

        public IVEOperatingExpensesViewModel()
        {
            TitleIVeOperatingExpensesList = new List<TitleIVeOperatingExpensesGet_spResult>();
        }

    }
}