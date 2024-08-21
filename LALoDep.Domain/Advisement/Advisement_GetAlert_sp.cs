namespace LALoDep.Domain.Advisement
{
    public class Advisement_GetAlert_spParams
    {
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }

    }


    public class Advisement_GetAlert_spResult
    {
        
        public string AdvisementAlert { get; set; }
    }
}
