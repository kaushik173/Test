using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingGetSummaryByPersonIDStartDateEndDate_spResult
    {
        public string CreditType { get; set; }
        public decimal P { get; set; }
        public decimal NP { get; set; }
        public decimal Total { get; set; }
        public string AgencyName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StaffName { get; set; }
    }
}
