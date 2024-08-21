using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CaseLoad
{
    public class pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spResult
    {
        public int? TotalCases { get; set; }
        public int? TotalConflictPending { get; set; }
        public int? TotalOpenPetitions { get; set; }
        public int? TotalOpenClients { get; set; }
        public int? PetitionID { get; set; }
        public string CaseNumber { get; set; }
        public int? PersonID { get; set; }
        public string PersonDOB { get; set; }
        public string PersonNameDisplay { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        //public string SortPetitionFileDate { get; set; }
        public string PetitionFileDate { get; set; }
        public string PetitionCloseDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? PetitionTypeCodeID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public string PetitionTypeCodeValue { get; set; }
        public string AgencyName { get; set; }
        public DateTime? HearingDateTime { get; set; }
        public string HearingTypeCodeShortValue { get; set; }
        public string HearingTypeCodeValue { get; set; }
        public int? MyRoleTypeCodeID { get; set; }
        public string Role { get; set; }
        //public string RoleStartDate { get; set; }
        public int? Conflict { get; set; }
        public string PersonNameFirstParameter { get; set; }
        public string PersonNameLastParameter { get; set; }
        //public byte? AllowTransferFlag { get; set; }
        //public byte? CaseloadPetitionFlag { get; set; }
    }
}
