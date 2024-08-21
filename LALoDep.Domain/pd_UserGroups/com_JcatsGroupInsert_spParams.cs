using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_UserGroups
{
    public class com_JcatsGroupInsert_spParams
    {
        public string JcatsGroupName { get; set; }
        public string JcatsGroupDescription { get; set; }
        public int? JcatsGroupDisplayOrder { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int CopySecurityOfJcatsGroupID { get; set; }
    }
}
