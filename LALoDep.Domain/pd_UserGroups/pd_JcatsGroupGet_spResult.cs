using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_UserGroups
{
    public class pd_JcatsGroupGet_spResult
    {
        public int JcatsGroupID { get; set; }
        public string JcatsGroupName { get; set; }
        public Int16 RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
    }
}
