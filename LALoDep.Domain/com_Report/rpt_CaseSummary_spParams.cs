using System;

namespace LALoDep.Domain.com_Report
{
    public class rpt_CaseSummary_spParams
    {
        public string CaseNumber { get; set; }
        public int? CaseID { get; set; }
        public int PetitionID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class rpt_CaseSummaryNextHearings_spParams
    {
        public string CaseNumber { get; set; }
        public int? ParmCaseID { get; set; }
        public int? PetitionID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? BranchID { get; set; }
        public int  UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}