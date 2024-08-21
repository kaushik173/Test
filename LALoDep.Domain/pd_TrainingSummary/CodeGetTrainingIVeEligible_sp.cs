namespace LALoDep.Domain.pd_TrainingSummary
{
public class CodeGetTrainingIVeEligible_spParams {
  public int? AgencyID { get; set; }
  public int? TrainingID { get; set; }
  public int? TrainingIVeEligibleCodeID { get; set; }
  public string SortOption { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class CodeGetTrainingIVeEligible_spResult
	{
		public CodeGetTrainingIVeEligible_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
	}
}
