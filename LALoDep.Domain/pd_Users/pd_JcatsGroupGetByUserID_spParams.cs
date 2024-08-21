using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users
{
    public class pd_JcatsGroupGetByUserID_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? IncludeJcatsGroupID { get; set; }
    }
}
