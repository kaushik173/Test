using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.AddEditCountyCounsel
{
    public class pd_StaffGetAttorneyByPersonID_spResult
    {
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int RoleID { get; set; }
        public int AgencyID { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public string LegalNumberID { get; set; }
        public string BarNumber { get; set; }
        public Int16 RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
    }
}
