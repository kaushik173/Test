using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Subpoena
{
    public class pd_SubpoenaAddressInsert_spParams
    {
        public int AgencyID { get; set; }
        public int SubpoenaID { get; set; }
        public int PersonAddressID { get; set; }
        public Int16 RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
