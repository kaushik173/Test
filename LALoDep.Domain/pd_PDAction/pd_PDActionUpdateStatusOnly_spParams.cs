using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_PDActionUpdateStatusOnly_spParams
    {
        public string PDActionIDList { get; set; }
        public int ActionStatusCodeID { get; set; }
        public string ActionStatusDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
