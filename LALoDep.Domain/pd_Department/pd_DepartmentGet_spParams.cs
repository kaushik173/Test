using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Department
{
    public class pd_DepartmentGet_spParams
    {
        public int DepartmentID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }

    public class pd_DepartmentGet_spResult
    {
        public int? DepartmentID { get; set; }
        public int? PersonID { get; set; }
        public string DepartmentStartDate { get; set; }
        public string DepartmentEndDate { get; set; }
        public int? DepartmentCodeID { get; set; }
        public int? AgencyID { get; set; }
        public short? RecordStateID { get; set; }
        public string DepartmentCodeValue { get; set; }

    }
}
