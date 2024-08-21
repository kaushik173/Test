namespace LALoDep.Domain.HearingPrepNote
{
    public class HearingPrepNoteGet_spParams
    {
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class HearingPrepNoteGet_spResult
    {
        public HearingPrepNoteGet_spResult()
        {
        }
        public int? NoteID { get; set; }
        public string NoteEntry { get; set; }
        public string TotalPrepHoursLink { get; set; }
    }
}
