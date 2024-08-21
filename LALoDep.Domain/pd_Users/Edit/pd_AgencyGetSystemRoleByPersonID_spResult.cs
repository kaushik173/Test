using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users.Edit
{
    public class pd_AgencyGetSystemRoleByPersonID_spResult
    {
        public string AgencyName { get; set; }
        public int AgencyID { get; set; }
        public int Selected { get; set; }
        public int? RoleID { get; set; }
        public string RoleStartDate { get; set; }
        public string RoleEndDate { get; set; }
        public short? RecordStateID { get; set; }
        public int? RoleTypeCodeID { get; set; }
    }
}
