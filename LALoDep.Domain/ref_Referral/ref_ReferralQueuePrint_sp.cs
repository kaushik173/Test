using System;

namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralQueuePrint_spParams
    {
        public int? PersonID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DateRangeTypeCodeID { get; set; }
        public string SortOption { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int? IncludeCompletedReferrals { get; set; }

    }


     
}
