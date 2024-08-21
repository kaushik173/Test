using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.com_Report
{
    public class com_ReportSourceGetByReportID_spResult
    {
        public int ReportSourceID { get; set; }
        public int ReportID { get; set; }
        public int ReportSourceSequence { get; set; }
        public string ReportSourceDocumentName { get; set; }
        public string ReportSourceStoredProcedureName { get; set; }
        public string ReportDisplayName { get; set; }
        public string AgencyMergeTemplatePath { get; set; }
        public byte? UseMasterFlag { get; set; }
    }
}
  