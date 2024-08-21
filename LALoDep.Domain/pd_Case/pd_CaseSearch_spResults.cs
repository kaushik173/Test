namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseSearch_spResults
    {
        public int wtCaseSearchSequence { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? PetitionID { get; set; }
        public int? RoleID { get; set; }
        public int? PersonID { get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string HHSANumber { get; set; }
        public string CaseNumber { get; set; }
        public int? RoleClient { get; set; }
        public int CaseID { get; set; }
        public string DOB { get; set; }
        public string ClosedDate { get; set; }
        public int TotalRecords { get; set; }
        public System.Guid? wtCaseSearchGUID { get; set; }
        public string LeadAttorney { get; set; }
        public int? TotalCases { get; set; }
        public int? ConflictFlag { get; set; }
        public string PetitionType { get; set; }
        public string PetitionCloseDate { get; set; }
    }
}
