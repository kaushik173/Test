using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.rpt_Print
{
    public class rpt_RecordTimePrintableVersion_spParams
    {
        public int? CaseID { get; set; }
        public string WorkIDList { get; set; }
        public int? WorkerPersonID { get; set; }
        public int? ClientPersonID { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
}
