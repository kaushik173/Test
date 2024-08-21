using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeTravelExpensesGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public int UserID { get; set; }

    }
    public class TitleIVeTravelExpensesGet_spResult
    {
        public int? ReadOnly { get; set; }
        public int? TitleIVeTravelExpenseID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public DateTime? TravelDate { get; set; }
        public string CaseNbr { get; set; }
        public string TravelerName { get; set; }
        public string PurposeOfTravel { get; set; }
        public string TypeOfTravelExpense { get; set; }
        public string SubcontractorBusinessName { get; set; }
        public decimal? NumberOfUnits { get; set; }
        public int? SpecifyUnit { get; set; }
        public decimal? UnitRate { get; set; }
        public decimal? PercentDependency { get; set; }
        public decimal? PercentCACFunds { get; set; }
        public decimal? PercentFFDRPFunds { get; set; }
        public decimal? CACFundAmount { get; set; }
        public decimal? FFDRPFundAmount { get; set; }
        public decimal? EligibleCost { get; set; }
        public string Note { get; set; }
        public int? InsertedByUserID { get; set; }
        public DateTime? InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public short? RecordStateID { get; set; }
        public string UploadButton { get; set; }
        public string FromZipcode   { get; set; }
        public string ToZipcode { get; set; }
        public decimal? EligibleNonTraining { get; set; }
        public decimal? EligibleAttorneyTraining { get; set; }
        public decimal? EligibleNonAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonTraining { get; set; }
        public decimal? EligibleAmountAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonAttorneyTraining { get; set; }
    }   
}
