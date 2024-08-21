namespace LALoDep.Domain.qcal
{
    public class qcal_HearingNoteGetPreviousNotes_spParams {
  public int? CaseID { get; set; }
  public int? HearingID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class qcal_HearingNoteGetPreviousNotes_spResult
	{
		public qcal_HearingNoteGetPreviousNotes_spResult()
		{
		}
		public string NoteHeader	{ get; set; }
		public string NoteEntry	{ get; set; }
		public int? NoteID	{ get; set; }
		public string Sort1	{ get; set; }
	}
}
