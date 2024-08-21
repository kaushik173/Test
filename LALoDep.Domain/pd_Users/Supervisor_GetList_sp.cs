namespace LALoDep.Domain.pd_Users
{
    public class Supervisor_GetList_spParams
    {
        public int? StaffPersonID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class Supervisor_GetList_spResult
    {
        public string AgencyName { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public string SupervisorDisplay { get; set; }
        public int? Selected { get; set; }

    }
}