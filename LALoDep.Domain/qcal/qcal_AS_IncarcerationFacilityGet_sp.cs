using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AS_IncarcerationFacilityGet_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class qcal_AS_IncarcerationFacilityGet_spResult
    {
        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }
    }
}
