using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_PDActionGetAllCasesByUserID_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
