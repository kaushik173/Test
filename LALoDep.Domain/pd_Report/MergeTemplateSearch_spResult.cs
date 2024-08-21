using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Report
{
   public class MergeTemplateSearch_spResult
    {
        public int ReportID { get; set; }

        public string DocumentName { get; set; }
        public string DataSource { get; set; }
        public string TemplatePath { get; set; }
    }
    public class MergeTemplateSearch_spParams
    {
        public int? AgencyID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }
    public class MergeTemplateCopyProcess_spParams
    {
        public int FromReportID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }

    
}
