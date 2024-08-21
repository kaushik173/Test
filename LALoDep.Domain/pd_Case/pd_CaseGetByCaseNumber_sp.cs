namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseGetByCaseNumber_spParams
    {
        public string CaseNumber { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_CaseGetByCaseNumber_spResult
    {
      
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string CaseNumber { get; set; }
        public System.Nullable<System.DateTime> CaseAppointmentDate { get; set; }
        public System.Nullable<System.DateTime> CaseClosedDate { get; set; }
        public byte? CasePanelCase { get; set; }
        public short? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
    }
}
