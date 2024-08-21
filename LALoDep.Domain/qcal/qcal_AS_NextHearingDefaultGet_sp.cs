using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AS_NextHearingDefaultGet_spParams
    {
        public int? CaseID { get; set; }
        public int? PreviousHearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AS_NextHearingDefaultGet_spResult
    {
        public qcal_AS_NextHearingDefaultGet_spResult()
        {
        }
        public string DefaultNote { get; set; }
    }
}
