using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralSummary_spParams
    {
        public int? IncludeInactiveStaffFlag { get; set; }
        public int? ModeCodeID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

    public class ref_ReferralSummary_spResult
    {
        public string ReferralCategory { get; set; }
        public int? ReferralCategoryCodeID { get; set; }
        public int? ReferralPersonID { get; set; }
        public string NameDisplay { get; set; }
        public int? TotalPending { get; set; }
        public int? NoDueDate { get; set; }
        public int? PastDue { get; set; }
        public int? DueWithin7Days { get; set; }
        public int? DueWithin15Days { get; set; }
        public int? DueOver15Days { get; set; }

    }
}
