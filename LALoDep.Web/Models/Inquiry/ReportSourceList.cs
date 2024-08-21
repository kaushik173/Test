using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Inquiry
{
    public class ReportSourceList
    {
        public int ReportSourceSequence { get; set; }
        public string ReportSourceDocumentName { get; set; }
        public string ReportSourceStoredProcedureName { get; set; }
        public string ReportDisplayName { get; set; }
    }
}