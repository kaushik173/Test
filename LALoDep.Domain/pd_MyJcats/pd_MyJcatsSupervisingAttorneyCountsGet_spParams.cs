using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MyJcats
{
    public class pd_MyJcatsSupervisingAttorneyCountsGet_spParams
    {
        public int StaffID { get; set; }
        public int PersonID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }



    }
}
