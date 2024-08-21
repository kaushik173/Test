using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_UserGroups
{
    public class pd_SecurityInsert_spParams
    {
        public int? JcatsGroupID { get; set; }
        public int SecurityItemID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
