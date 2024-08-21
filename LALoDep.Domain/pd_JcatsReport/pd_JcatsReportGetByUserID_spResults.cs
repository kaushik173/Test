using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_JcatsReport
{
    public class pd_JcatsReportGetByUserID_spResults
    {

        public int JcatsReportID { get; set; }
        public string JcatsReportName { get; set; }
        public int? ReportTypeCodeID { get; set; }
        public int? ReportCategoryCodeID { get; set; }
        public int? ReportDocumentTypeCodeID { get; set; }
        public string JcatsReportDescription { get; set; }
        public string ReportCategory { get; set; }
        public string JcatsReportType { get; set; }
        public string ReportDocumentType { get; set; }
        public string JcatsReportSampleURL { get; set; }
        public int SelectedFlag { get; set; }
        public int? ParmCount { get; set; }
    }
}
