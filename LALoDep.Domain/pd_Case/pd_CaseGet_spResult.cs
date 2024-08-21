using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseGet_spResult
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? CaseAppointmentDate { get; set; }
        public DateTime? CaseClosedDate { get; set; }
        public byte? CasePanelCase { get; set; }
        public int? DepartmentID { get; set; }
        public Int16? RecordStateID { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedByUserName { get; set; }
        public string LeadAttorney { get; set; }
        public int? LeadAttorneyPersonID { get; set; }
        public int? LeadAttorneyRoleID { get; set; }
        public int? LeadAttorneyRoleTypeCodeID { get; set; }
        public int? CaseNameRoleID { get; set; }
        public string CaseName { get; set; }
        public string CaseNumberTitle { get; set; }
        public string PetitionNumber { get; set; }
        public int? AdminReviewFlag { get; set; }
        public int? ReferralSourceCodeID { get; set; }
        public DateTime? NextHearingDate { get; set; }
    }
}
