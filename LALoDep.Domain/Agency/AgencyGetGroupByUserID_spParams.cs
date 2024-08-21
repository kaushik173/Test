using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_TrainingSummary
{
    public class AgencyGetGroupByUserID_spParams
    {
        public string SortOption { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }
}
