using System;

namespace LALoDep.Core.NG_sp.com_Report
{
    public class NG_com_ReportParameterDefinitionGetByReportID_spParams
    {
        public int ReportID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
