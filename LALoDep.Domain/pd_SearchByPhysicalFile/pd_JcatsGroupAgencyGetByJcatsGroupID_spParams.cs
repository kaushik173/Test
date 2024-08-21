using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_SearchByPhysicalFile
{
    public class pd_JcatsGroupAgencyGetByJcatsGroupID_spParams
    {
        public int JcatsGroupID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
