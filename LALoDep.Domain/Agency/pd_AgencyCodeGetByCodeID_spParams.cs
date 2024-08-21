using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Agency
{
    public class pd_AgencyCodeGetByCodeID_spParams
    {
        public int CodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_AgencyCodeGetByCodeID_spResult
    {
        public string AgencyName { get; set; }
        public int? AgencyID { get; set; }
        public int? Selected { get; set; }
        public int? InAgency { get; set; }
        public int? AgencyCodeID { get; set; }
    }

}
