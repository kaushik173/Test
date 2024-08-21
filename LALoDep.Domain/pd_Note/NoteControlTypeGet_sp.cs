namespace LALoDep.Domain.pd_Note
{
    public class NoteControlTypeGet_spParams
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? NoteID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public string NG_NavigationURL { get; set; }

    }


    public class NoteControlTypeGet_spResult
    {
        public NoteControlTypeGet_spResult()
        {
        }
        public string ControlType { get; set; }
    }
}
