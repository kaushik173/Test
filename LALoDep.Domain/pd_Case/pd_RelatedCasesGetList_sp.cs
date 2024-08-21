namespace LALoDep.Domain.pd_Case
{

    public class pd_RelatedCasesGetList_spParams {
  public int? CaseID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class pd_RelatedCasesGetList_spResult
	{
		public pd_RelatedCasesGetList_spResult()
		{
		}
		public string HasRelatedCasesInfo	{ get; set; }
		public string ThisCaseRole	{ get; set; }
		public int? RelatedJcatsNumber	{ get; set; }
		public string RelatedCaseNumber	{ get; set; }
		public string RelatedCaseRole	{ get; set; }
		public string Comment	{ get; set; }
        public string Attorney { get; set; }
        public string Status { get; set; }
    }
}
