using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECAddNew_spParams
    {
        public int ChildRoleID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

}
