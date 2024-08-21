namespace LALoDep.Domain.Advisement
{
    public class Advisement_GetAttorneyList_spParams
    {
        public int? CaseID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class Advisement_GetAttorneyList_spResult
    {
        public Advisement_GetAttorneyList_spResult()
        {
        }
        public string AttorneyDisplay { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int? DefaultFlag { get; set; }
    }
}
