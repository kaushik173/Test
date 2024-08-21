using System;

namespace LALoDep.Domain.qcal
{
    public class qcal_ToDo_History_spParams
    {

        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }


    public class qcal_ToDo_History_spResult
    {

        public int? PDActionID { get; set; }
        public int? AgencyID { get; set; }
        public int? BranchID { get; set; }
        public int? CaseID { get; set; }
        public int? AssignedToPersonID { get; set; }
        public int? ActionTypeCodeID { get; set; }
        public string ActionNote { get; set; }
        public int? ActionStatusCodeID { get; set; }
        public string ActionStatusDate { get; set; }
        public string ActionDueDate { get; set; }
        public string ActionReminderDate { get; set; }
        public int? ActionAssociatedToEntityID { get; set; }
        public int? ActionAssociatedToEntityTypeCodeID { get; set; }
        public byte? RecordStateID { get; set; }
        public decimal RecordTimeStamp { get; set; }
        public string ActionType { get; set; }
        public string CaseNumber { get; set; }
        public int? Completed { get; set; }
        public string CaseName { get; set; }
        public string CaseFileDisplay { get; set; }
        public string CaseFilePath { get; set; }
        public string AssignedTo { get; set; }
    }
}
