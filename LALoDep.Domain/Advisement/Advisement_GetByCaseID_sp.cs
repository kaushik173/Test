namespace LALoDep.Domain.Advisement
{
    public class Advisement_GetByCaseID_spParams
    {
        public int? CaseID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class Advisement_GetByCaseID_spResult
    {
        public string ClientDisplay { get; set; }
        public int? ClientRoleID { get; set; }
        public int? GroupDisplayOrder { get; set; }
        public string GroupDisplay { get; set; }
        public int? GroupDisplayHearingInfo { get; set; }
        public int? AdvisementDisplayOrder { get; set; }
        public int? AdvisementID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int? AdvisementCodeID { get; set; }
        public string AdvisementDisplay { get; set; }
        public System.Nullable<System.DateTime> AdvisementDateTime { get; set; }
        public int? HearingID { get; set; }
        public string HearingDisplay { get; set; }
        public System.Nullable<System.DateTime> FileDueDate { get; set; }
        public int? StatusCodeID { get; set; }
        public System.Nullable<System.DateTime> StatusDate { get; set; }
        public int? AddAnotherFlag { get; set; }
    }
}
