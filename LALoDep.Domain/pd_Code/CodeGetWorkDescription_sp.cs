  namespace LALoDep.Domain.pd_Code
{
public class CodeGetWorkDescription_spParams {
  public int? AgencyID { get; set; }
  public int? WorkID { get; set; }
  public int? WorkDescriptionCodeID { get; set; }
  public string SortOption { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }
         
        public int? ReferralID { get; set; }
    }

	
	public class CodeGetWorkDescription_spResult
	{
		public CodeGetWorkDescription_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
		public int? DefaultIVeEligibleCodeID	{ get; set; }
        public int? DefaultCanChangeFlag { get; set; } 
        public int? AttorneyDefaultIVeEligibleCodeID { get; set; }
        public int? AttorneyDefaultCanChangeFlag { get; set; }
        public int? SupervisorDefaultIVeEligibleCodeID { get; set; }
        public int? SupervisorDefaultCanChangeFlag { get; set; }
        public int? UseWorkTimeFlag { get; set; }
        public int? ZipCodeRequiredFlag { get; set; }

    }
}
