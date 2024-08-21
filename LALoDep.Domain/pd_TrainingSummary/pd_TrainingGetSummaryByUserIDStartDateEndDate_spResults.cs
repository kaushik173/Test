using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingGetSummaryByUserIDStartDateEndDate_spResults
    {
        public int ID { get; set; }
        public string RoleType { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int PersonID { get; set; }
        public decimal P { get; set; }
        public decimal NP { get; set; }
        public decimal Total { get; set; }
    }
}
