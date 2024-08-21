using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeOperatingExpensesGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public int UserID { get; set; }

    }
    public class TitleIVeOperatingExpensesGet_spResult
    {
        public string UploadButton { get; set; }
        public int? ReadOnly { get; set; }
        public int? TitleIVeOperatingExpenseID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string InvoiceReferenceNbr { get; set; }
        public string ExpenseProvider { get; set; }
        public decimal? ExpenseAmount { get; set; }
        public bool ForIndividual { get; set; }
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
        public string ExpenseDescription { get; set; }
        public decimal? EligibleNonTraining { get; set; }
        public decimal? EligibleAttorneyTraining { get; set; }
        public decimal? EligibleNonAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonTraining { get; set; }
        public decimal? EligibleAmountAttorneyTraining { get; set; }
        public decimal? EligibleAmountNonAttorneyTraining { get; set; }
        public decimal? OverHeadRate { get; set; }
    }
}
