
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{

    public class TitleIVeClaimFormUpdate_spParams
    {
        public int? ErrorID { get; set; }
        public int? UserID { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public string PreparedByName { get; set; }
        public string PreparedByTitle { get; set; }
        public string PreparedByPhone { get; set; }
        public string PreparedByEmail { get; set; }
        public string Note { get; set; }
        public decimal?  PercentageDependency { get; set; }
        public int?  NbrChildrenCasesWorked { get; set; }

        public decimal? TrainingEligibleAmount { get; set; }
        public decimal? TrainingEligibleRateCA { get; set; }
        public decimal? TrainingEligibleAmtCA { get; set; }
        public decimal? TrainingReimbursementRateFed { get; set; }
        public decimal? TrainingReimbursementAmt { get; set; }
    }
}