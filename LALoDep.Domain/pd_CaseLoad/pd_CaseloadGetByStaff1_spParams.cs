using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CaseLoad
{
    public class pd_CaseloadGetByStaff1_spParams
    {
        public int? RoleTypeCodeID { get; set; }
        public int? AgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
