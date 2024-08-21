using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingGetSummaryByUserIDStartDateEndDate_spParams
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AgencyGroupID { get; set; }
        public int AgencyID { get; set; }
        public int IncludeAllActiveStaff { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
