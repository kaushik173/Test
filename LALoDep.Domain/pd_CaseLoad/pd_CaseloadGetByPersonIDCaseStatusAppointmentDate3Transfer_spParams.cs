using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CaseLoad
{
    public class pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3Transfer_spParams
    {
        public int PersonID { get; set; }
        public string CaseStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PetitionNumber { get; set; }
        public string RoleType { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string SortOption { get; set; }
        public string SortDirection { get; set; }
        public int? ParmCaseID { get; set; }
    }

    public class pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3Transfer_spResult
    {
        public int? CaseID { get; set; }
        public string CaseNumber { get; set; }
        public string Clients { get; set; }
        public string Department { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string PetitionNumber { get; set; }
        public string CaseName { get; set; }
        public string AttorneyRoleStartDate { get; set; }
        public string SortAttorneyRoleStartDate { get; set; }
        public int? TotalCases { get; set; }

    }
}
