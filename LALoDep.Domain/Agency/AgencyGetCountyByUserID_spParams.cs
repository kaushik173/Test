using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Agency
{
    public class AgencyGetCountyByUserID_spParams
    {
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class AgencyGetCountyByUserID_spResult
    {
        public int AgencyCountyID { get; set; }
        public string AgencyCounty { get; set; }
        public string AgencyCountyAbbreviation { get; set; }
    }
}
