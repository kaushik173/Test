using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_QuickRFDClientGet_spParams
    {
        public int CaseID { get; set; }
        public int ReportFilingDueID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_QuickRFDClientGet_spResult
    {
        public string ClientName { get; set; }
        public int? ClientRoleID { get; set; }
        public int? ClientPersonID { get; set; }
        public int? HasNon602Flag { get; set; }
        public int? Has602Flag { get; set; }
        public string LastSeen { get; set; }
        public int? RFDRoleContactID { get; set; }
        public string RFDRoleContactDate { get; set; }
        public int? RFDRoleID { get; set; }
        public int? RFDRoleContactTypeCodeID { get; set; }
    }
}









