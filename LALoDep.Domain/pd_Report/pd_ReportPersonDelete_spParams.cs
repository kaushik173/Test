using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Report
{
    public class pd_ReportPersonDelete_spParams
    {
        public int? ReportPersonID { get; set; }
        public decimal? RecordTimeStamp{ get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }
        public int RecordStateID{ get; set; }
    }
}
