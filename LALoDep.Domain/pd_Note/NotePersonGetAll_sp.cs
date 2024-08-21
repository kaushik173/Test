namespace LALoDep.Domain.pd_Note
{
    public class NotePersonGetAll_spParams
    {
        public int? CaseID { get; set; }
        public int? NoteID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class NotePersonGetAll_spResult
    {
         
        public string PersonDisplay { get; set; }
        public int? PersonID { get; set; }
        public int? NotePersonID { get; set; }
    }
}
