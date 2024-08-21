namespace LALoDep.Domain.qcal
{
    public class qcal_ToDo_ActionType_spParams
    {
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? PDActionID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_ToDo_ActionType_spResult
    {
        public qcal_ToDo_ActionType_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
    }
}
