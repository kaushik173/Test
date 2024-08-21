using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class HearingCaseFileIUD_spParams
    {
        public string IUD { get; set; }
        public int HearingCaseFileID { get; set; }
        public int HearingID { get; set; }
        public int CaseFileID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
