namespace LALoDep.Domain.TrainingImport
{
public class TrainingImport_GetVenueList_spParams {
  public int? AgencyGroupID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class TrainingImport_GetVenueList_spResult
	{
		public TrainingImport_GetVenueList_spResult()
		{
		}
		public string Venue	{ get; set; }
		public int? VenueCodeID	{ get; set; }
	}
}
