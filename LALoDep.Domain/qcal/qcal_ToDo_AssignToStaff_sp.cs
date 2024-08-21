namespace LALoDep.Domain.qcal
{
    public class qcal_ToDo_AssignToStaff_spParams
    {
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? PDActionID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_ToDo_AssignToStaff_spResult
    {
       
        public string FullName { get; set; }
        public int? PersonID { get; set; }
        public int? DefaultFlag { get; set; }
    }
}
