namespace LALoDep.Domain.qcal
{
    public class qcal_HearingNoteGetMain_spParams {
  public int? CaseID { get; set; }
  public int? HearingID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class qcal_HearingNoteGetMain_spResult
	{
		public qcal_HearingNoteGetMain_spResult()
		{
		}
		public string NoteEntry	{ get; set; }
		public int? NoteID	{ get; set; }
		public int? NoteTypeCodeID	{ get; set; }
	}
}
