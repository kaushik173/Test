using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingReportFilingDueGet_spResult
    {
        public int HearingReportFilingDueID { get; set; }

       
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }

        public int? HearingReportFilingDueTypeCodeID { get; set; }

        public DateTime? HearingReportFilingDueOrderDate { get; set; }

        public DateTime? HearingReportFilingDueDate { get; set; }

        public DateTime? HearingReportFilingDueEndDate { get; set; }
        public int? RequestedByPersonID { get; set; }
        public int? RequestedForPersonID { get; set; }
        public int? HearingReportFilingDueLegalResearchTypeCodeID { get; set; }
        public string HearingReportFilingDueTypeCodeValue { get; set; }
        public DateTime? HearingDateTime { get; set; }
        public string HearingType { get; set; }
         
        public string RequestFor { get; set; }
        public string RequestBy { get; set; }
        public string RFDHeader { get; set; }
        public string LegalResearchType { get; set; }

    }
}










