namespace LALoDep.Domain.TrainingImport
{
public class TrainingImport_GetPersonList_spParams {
  public int? AgencyGroupID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class TrainingImport_GetPersonList_spResult
	{
		public TrainingImport_GetPersonList_spResult()
		{
		}
		public string PersonDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
	}
}
