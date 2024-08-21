using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_ProfileGetList_spParams
    {

        public int CaseID { get; set; }
        public int RoleID { get; set; }
        public int RFDID { get; set; }
        public int ProfileTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}

