namespace LALoDep.Domain.Advisement
{
public class Advisement_GetForAttorney_spParams {
  public int? AttorneyPersonID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class Advisement_GetForAttorney_spResult
	{
        public int? MaxRowsToDisplay { get; set; }


        public int? AlertFlag	{ get; set; }
		public string DueDate	{ get; set; }
		public string CaseNumber	{ get; set; }
		public int? CaseID	{ get; set; }
		public int? AgencyID	{ get; set; }
		public string TypeDisplay	{ get; set; }
		public string Client	{ get; set; }
		public string CaseName	{ get; set; }
		public string SortDueDate	{ get; set; }
	}
}
