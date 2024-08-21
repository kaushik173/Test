using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Agency
{
    public class AgencyGetByUserID_spParams
    {
        public string SortOption { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class AgencyGetByUserID_spResult
    {
        public int AgencyID { get; set; }
        public string AgencyName { get; set; }
        public byte UniquePetitionNumbersFlag { get; set; }

    }
    public class AgencyGetGroupByUserID_sppParams
    {
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class AgencyGetGroupByUserID_spResult
    {
        public int AgencyGroupID { get; set; }
        public string AgencyGroup { get; set; }
        public string AgencyGroupAbbreviation { get; set; }

    }
}
