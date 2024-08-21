using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.AddEditCountyCounsel
{
    public class pd_UserRoleInsert_spParams
    {
        public int? RoleID { get; set; }
        public int PersonID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public int RoleAgencyID { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public int RecordStateID { get; set; }
        public int? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
