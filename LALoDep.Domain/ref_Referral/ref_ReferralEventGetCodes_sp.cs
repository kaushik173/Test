namespace LALoDep.Domain.ref_Referral
{
public class  ref_ReferralEventGetCodes_spParams {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralEventID { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class  ref_ReferralEventGetCodes_spResult
	{
		public  ref_ReferralEventGetCodes_spResult()
		{
		}
		public string CodeDisplay	{ get; set; }
		public int? CodeID	{ get; set; }
		public int? Selected	{ get; set; }
	}
}
