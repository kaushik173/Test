using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? WorkerPersonID { get; set; }
        public int? ClientPersonID { get; set; }
        public int? ReferralID { get; set; }
        public string FilterByStaffType { get; set; }

    }
}
