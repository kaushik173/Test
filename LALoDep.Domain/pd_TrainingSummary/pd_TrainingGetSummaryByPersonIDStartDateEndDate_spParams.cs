using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class pd_TrainingGetSummaryByPersonIDStartDateEndDate_spParams
    {
        public int PersonID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; } 
          public int? TrainingVenueCodeID { get; set; }
    }
}
