using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Report
{
    public class pd_ReportPersonInsert_spParams
    {
        public int ReportID { get; set; }
        public int PersonID { get; set; }
        public int ReportPersonDisplayOrder { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
