namespace LALoDep.Domain.pd_Training
{
    public class pd_TrainingGetSupervisorList_spParams {
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class pd_TrainingGetSupervisorList_spResult
	{
		public pd_TrainingGetSupervisorList_spResult()
		{
		}
		public string PersonDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
	}
}
