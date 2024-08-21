namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralImmigrationGetByReferralID_spParams {
  public int? CaseID { get; set; }
  public int? CaseAgencyID { get; set; }
  public int? ReferralID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class ref_ReferralImmigrationGetByReferralID_spResult
	{
		public ref_ReferralImmigrationGetByReferralID_spResult()
		{
		}
		public int? ReferralImmigrationID	{ get; set; }
		public int? ReferralID	{ get; set; }
		public int? ImmigrationAgencyCodeID	{ get; set; }
		public int? ImmigrationAttorneyPersonID	{ get; set; }
		public System.Nullable<System.DateTime> ImmigrationStartDate	{ get; set; }
		public System.Nullable<System.DateTime> ImmigrationEndDate	{ get; set; }
		public string SortDate	{ get; set; }
        public int? Immigration317eCodeID { get; set; }

    }
}
