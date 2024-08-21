using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Staff
{
    public class pd_StaffGetByPersonID_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }

    public class pd_StaffGetByPersonID_spResult
    {
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? RoleID { get; set; }
        public int? AgencyID { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public short? RecordStateID { get; set; }
    }
}
