using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Agency
{
    public class AgencyGetRegionByUserID_spParams
    {
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class AgencyGetRegionByUserID_spResult
    {
        public int AgencyRegionID { get; set; }
        public string AgencyRegion { get; set; }
        public string AgencyRegionAbbreviation { get; set; }
    }
}
