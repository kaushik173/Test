using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.com_Report
{
    public class com_ReportParameterValueInsert_spParams
    {
        public int? ReportParameterValueID { get; set; }
        public int ReportID { get; set; }
        public int Sequence { get; set; }
        public string Value { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }  
    }
}
