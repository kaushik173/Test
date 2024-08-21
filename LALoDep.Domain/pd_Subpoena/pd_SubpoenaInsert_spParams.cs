using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Subpoena
{
    public class pd_SubpoenaInsert_spParams
    {
        public int AgencyID { get; set; }
        public int HearingID { get; set; }
        public int SubpoenaServedToRoleID { get; set; }
        public string SubpoenaServedDate { get; set; }
        public Int16 RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
