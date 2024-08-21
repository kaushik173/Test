using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Profile
{
    public class pd_ProfileGetList_spResult
    {
        public int? ReportID { get; set; }
        public string ReportValue { get; set; }
        public int? RoleID { get; set; }
        public int? HearingReportFilingDueID { get; set; }
        public int? ProfileTypeCodeID { get; set; }
        public int? ProfileID { get; set; }
        public int? PersonID { get; set; }
        public string ProfileDate { get; set; }
        public string ProfilePerson { get; set; }
        public string ProfileDisplay { get; set; }
        public string SortDate { get; set; }
        public string ProfilePersonDisplay { get; set; }
    }
}
