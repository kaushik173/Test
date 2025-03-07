namespace LALoDep.Domain.ref_Referral
{
public class  ref_ReferralEventGetAppearingAttorneys_spParams {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralEventID { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
    }

	
	public class  ref_ReferralEventGetAppearingAttorneys_spResult
	{
		public  ref_ReferralEventGetAppearingAttorneys_spResult()
		{
		}
		public string NameDisplay	{ get; set; }
		public int? PersonID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
