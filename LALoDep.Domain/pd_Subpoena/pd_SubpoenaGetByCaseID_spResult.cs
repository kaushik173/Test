using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Subpoena
{
    public class pd_SubpoenaGetByCaseID_spResult
    {
        public int? SubpoenaID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public int? SubpoenaServedToRoleID { get; set; }
        public string SubpoenaServedDate { get; set; }
        public byte? RecordStateID { get; set; }
        public string SubpoenaServedToPersonName { get; set; }
        public string CreationDate { get; set; }
        public string HearingDate { get; set; }
        public string HearingTime { get; set; }
        public string HearingDepartment { get; set; }
        
    }
}
