namespace LALoDep.Domain.TrainingImport
{
public class TrainingImport_ProcessPendingRecords_spParams {
  public string FileName { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class TrainingImport_ProcessPendingRecords_spResult
	{
		public TrainingImport_ProcessPendingRecords_spResult()
		{
		}
		public string MessageDisplay	{ get; set; }
	}
}
