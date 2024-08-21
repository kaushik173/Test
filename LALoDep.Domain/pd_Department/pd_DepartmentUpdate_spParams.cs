using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Department
{
    public class pd_DepartmentUpdate_spParams
    {
        public int? DepartmentID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public DateTime? DepartmentStartDate { get; set; }
        public DateTime? DepartmentEndDate { get; set; }
        public int? DepartmentCodeID { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
}
