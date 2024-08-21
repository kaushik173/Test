using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.com_Report
{
    public class com_ReportParameterHeaderInsert_spParams
    {
        public int? ReportParameterHeaderID { get; set; }
        public int ReportID { get; set; }
        public int JcatsUserID { get; set; }
        public string ReportParameterHeaderName { get; set; }
        public string ReportParameterHeaderValue { get; set; }
        public int? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; } 

    }
}
