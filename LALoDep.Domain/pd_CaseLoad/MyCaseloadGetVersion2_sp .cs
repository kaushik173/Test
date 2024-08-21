using System;

namespace LALoDep.Domain.pd_CaseLoad
{
    public class MyCaseloadGetVersion2_spParams
    {
        public int? CaseloadPersonID { get; set; }
        public int? SortByCodeID { get; set; }
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public string PetitionNumber { get; set; }
        public int? AgencyCountyID { get; set; }
        public string ClientType { get; set; }
        public int? ClientStatusCodeID { get; set; }
        public DateTime? HearingStartDate { get; set; }
        public DateTime? HearingEndDate { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public int? NoFutureARFlag { get; set; }
        public int? NoCompletedAR6MFlag { get; set; }
        public int? AgeStartRange { get; set; }
        public int? AgeEndRange { get; set; }
        public int? OutOfStateFlag { get; set; }
        public int? AddressTypeCodeID { get; set; }
        public int? ClassificationCodeID { get; set; }
        public int? MedicationCurrentCodeID { get; set; }
        public int? MedicationEverCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int? ParmCaseID { get; set; }
        public int?  ReportID { get; set; }
        public int? NoRaceFlag { get; set; }
    }


    public class MyCaseloadGetVersion2_spResult
    {
        public string ResultHeader { get; set; }
        public int? TotalCases { get; set; }
        public int? TotalClients { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? PetitionID { get; set; }
        public string PetitionType { get; set; }
        public string PetitionFileDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string ClientName { get; set; }
        public int? ClientRoleID { get; set; }
        public int? ClientPersonID { get; set; }
        public int? ClientAge { get; set; }
        public string ClientDOB { get; set; }
        public string ClientDOBSort { get; set; }
        public string ClientSex { get; set; }
        public string ClientStatus { get; set; }
        public int? LatestPetitionID { get; set; }
        public string LatestContactDate { get; set; }
        public string AREndDate { get; set; }
        public string NextCourtDate { get; set; }
        public string NextCourtType { get; set; }
    }
}
