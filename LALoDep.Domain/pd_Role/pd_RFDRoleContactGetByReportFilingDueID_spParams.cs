using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RFDRoleContactGetByReportFilingDueID_spParams
    {
        public int ReportFilingDueID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_RFDRoleContactGetByReportFilingDueID_spResult
    {
        public int ContactTypeID { get; set; }
        public string ContactType { get; set; }
        public int ContactID { get; set; }
        public string ContactDate { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int RFDRoleID { get; set; }
        public string RoleType { get; set; }
        public string ClientDisplay { get; set; }

    }


}
