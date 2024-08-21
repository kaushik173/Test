using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.AddEditCountyCounsel
{
    public class pd_UserRoleUpdate_spParams
    {
        public int RoleID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public int AgencyID { get; set; }
        public string RoleStartDate { get; set; }
        public string RoleEndDate { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
