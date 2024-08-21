namespace LALoDep.Domain.pd_Report
{
    public class pd_ReportPersonGetByPersonID_spResult
    {
        public int ReportPersonID { get; set; }
        public int ReportID { get; set; }
        public int PersonID { get; set; }
        public int ReportPersonDisplayOrder { get; set; }
  
        public string JcatsReportName { get; set; }
        public string JcatsReportSampleURL { get; set; }
    }
}