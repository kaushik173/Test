namespace LALoDep.Domain.qcal
{
    public class qcal_AS_HearingAttendanceGetAdditionalAttendee_spParams
    {
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AS_HearingAttendanceGetAdditionalAttendee_spResult
    {
        
        public string PersonNameDisplay { get; set; }
        public int? RoleID { get; set; }
    }
}
