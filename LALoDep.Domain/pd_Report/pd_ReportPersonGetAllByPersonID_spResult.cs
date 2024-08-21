using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Report
{
    public class pd_ReportPersonGetAllByPersonID_spResult
    {
        public int JcatsReportID { get; set; }
        public int? ReportPersonID { get; set; }
        public string ReportValue { get; set; }
        public string JcatsReportDescription { get; set; }
        public string JcatsReportName{ get; set; }
        public int MyReportFlag { get; set; }
        public int? ReportPersonDisplayOrder { get; set; }
    }
}
