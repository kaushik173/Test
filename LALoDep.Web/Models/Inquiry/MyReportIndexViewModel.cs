using System.Collections.Generic;
namespace LALoDep.Models.Inquiry
{
    public class MyReportIndexViewModel
    {
        
            public string FullName { get; set; }
            public string ReturnUrl { get; set; }
            public string saveReportIds { get; set; }
            public string oldSavedReportIds { get; set; }
            public List<ReportViewModel> Reports { get; set; }
            public bool AddButtonAccess { get; set; }
            public MyReportIndexViewModel()
            {
                Reports = new List<ReportViewModel>();
            }
    }
    public class ReportViewModel
    {
        public int ReportID { get; set; }
        public int? ReportPersonID { get; set; }
        public string ReportValue { get; set; }
        public string ReportDescription { get; set; }
        public bool Selected { get; set; }
    }
}