using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseUpdateCaseName_spParams
    {
        public int CaseID { get; set; }
        public int CaseNameRoleID { get; set; }


        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}


