using System;

namespace LALoDep.Domain.RTNC
{

    public class RTNC_Search_spParams {
  public DateTime? StartDate { get; set; }
  public DateTime? EndDate { get; set; }
  public int? StaffPersonID { get; set; }
  public int? SupervisorPersonID { get; set; }
  public int? AgencyID { get; set; }
  public int? AgencyGroupID { get; set; }
  public int? WorkDescriptionCodeID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class RTNC_Search_spResult
	{
		public RTNC_Search_spResult()
		{
		}
		public string WorkDate	{ get; set; }
		public string StaffMember	{ get; set; }
		public int? StaffPersonID	{ get; set; }
		public string WorkDescription	{ get; set; }
		public int? WorkDescriptionCodeID	{ get; set; }
		public decimal? WorkHours	{ get; set; }
		public string WorkNote	{ get; set; }
		public int? WorkID	{ get; set; }
		public string SortDate	{ get; set; }
		public int? AgencyID	{ get; set; }
		public int? AgencyGroupID	{ get; set; }
	}
}
