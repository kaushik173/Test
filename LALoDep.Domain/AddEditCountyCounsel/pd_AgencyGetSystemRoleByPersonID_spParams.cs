using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.AddEditCountyCounsel
{
    public class pd_AgencyGetSystemRoleByPersonID_spParams
    {
        public int? PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int SortFlag { get; set; }
    }
}
