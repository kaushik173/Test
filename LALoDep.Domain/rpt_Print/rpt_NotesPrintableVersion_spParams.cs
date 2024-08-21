using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.rpt_Print
{
    public class rpt_NotesPrintableVersion_spParams
    {
        public int CaseID{ get; set; }
        public string NoteIDList { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
