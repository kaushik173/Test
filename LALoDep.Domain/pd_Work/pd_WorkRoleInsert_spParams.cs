using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkRoleInsert_spParams
    {
        public int? AgencyID { get; set; }
        public int WorkID { get; set; }
        public int RoleID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
