using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users
{
    public class pd_CodeGetBySysValAndUserID_spParams
    {
        public string SystemValueIDList { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? IncludeCodeID { get; set; }

        
    }
}
