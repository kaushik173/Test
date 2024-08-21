namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralHeader_spParams {
  public int? CaseID { get; set; }
  public int? ReferralID { get; set; }
  public int? RoleID { get; set; }
  public int? ReferralTypeCodeID { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

}

	
	public class ref_ReferralHeader_spResult
	{
		public ref_ReferralHeader_spResult()
		{
		}
		public string ReferralHeader	{ get; set; }
		public string FirmDept	{ get; set; }
		public string ClientInfo	{ get; set; }
		public string ClientAddress	{ get; set; }
		public string NextCourtDates	{ get; set; }
		public string AttachedFile	{ get; set; }
		public string ActivityLog	{ get; set; }
        public int? ANumber_PersonID { get; set; }
        public int? ANumber_LegalNumberID { get; set; }
        public string NG_NavigationURL { get; set; }
        public string ANumber { get; set; }
	}
}
