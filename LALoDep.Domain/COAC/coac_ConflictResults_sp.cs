namespace Jcats.SD.Domain.COAC
{
    public class coac_ConflictResults_spParams
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? UserID { get; set; }

    }


    public class coac_ConflictResults_spResult
    {
        public coac_ConflictResults_spResult()
        {
        }
        public int? KeySequence { get; set; }
        public int? CaseID { get; set; }
        public string CaseNumber { get; set; }
        public string ClosedDate { get; set; }
        public string Agency { get; set; }
        public int? PersonID { get; set; }
        public int? PersonNameID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string DOB { get; set; }
        public string Sex { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? RoleID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string Role { get; set; }
        public int? RoleClient { get; set; }
        public string LeadAttorney { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalCases { get; set; }
    }
}
