using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeActivityLogMonthlySummaryGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? ActivityLogID { get; set; }
        public int? UserID { get; set; }
    }

    public class TitleIVeActivityLogMonthlySummaryGet_spResult
    {
        public decimal? FFDRPWorked { get; set; }
        public decimal? FFDRPPaidTimeOff { get; set; }
        public decimal? TotalFFDRPEligible { get; set; }
        public decimal? TotalFFDRPIneligible { get; set; }





        public decimal? PercentFFDRPIneligible { get; set; }
        public decimal? PercentTraining { get; set; }
        public decimal? PercentCaseSpecific { get; set; }
        public decimal? PercentAdministrative { get; set; }
    }
}
