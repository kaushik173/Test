using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.com_Report
{
    public class com_ReportSourceGetByReportID_spParams
    {
        public int ReportID { get; set; }
        public int UserID { get; set; }
        public string BatchLogJobID { get; set; }
    }
}
