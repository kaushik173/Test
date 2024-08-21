using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECGetSummary_spResult
    {
        public string GroupDisplay { get; set; }
        public int? GroupID { get; set; }
        public string StaffMember { get; set; }
        public int? StaffPersonID { get; set; }
        public int? Completed { get; set; }
        public int? MarkedIncomplete { get; set; }
        public int? PastDue { get; set; }
        public string XColumn { get; set; }
        public int? XValue { get; set; }
        public string YColumn { get; set; }
        public int? YValue { get; set; }
    }
    public class CSECGetSummary_spParams
    {
        public int AgencyID { get; set; }
        public int CSECAssignedToRoleTypeCodeID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DateRangeType { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class CSECGetAgency_spResult
    {
        public string AgencyDisplay { get; set; }
        public int AgencyID { get; set; }
        public int HomeAgencyFlag { get; set; }
    }
    public class CSECGetAgency_spParams
    {

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class CSECGetStaffType_spResult
    {
        public string CodeDisplay { get; set; }
        public int CodeID { get; set; }

    }
    public class CSECGetStaffType_spParams
    {

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class CSECGetMyQueue_spResult
    {
        public int CSECID { get; set; }
        public string Child { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public short? ScoreNumeric { get; set; }
        public string ScoreLiteral { get; set; }
        public int? CanEditFlag { get; set; }
        public string SortBy { get; set; }


        public string StatusDate { get; set; }
        public string StatusBy { get; set; }
        public string SortDueDate { get; set; }
        public string SortCompletionDate { get; set; }
        public string SortStatusDate { get; set; }
        public int? CanMarkAsIncompleteFlag { get; set; }
        public int? CanRestoreFlag { get; set; }
    }
    public class CSECGetMyQueue_spParams
    {
        public int CSECAssignedToPersonID { get; set; }
        public int IncludeCompleted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DateRangeType { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int ShowIncompletes { get; set; }
    }
    public class CSECUpdateStatus_spParams
    {
        public int? CSECID { get; set; }
        public string UpdateMode { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
}
 