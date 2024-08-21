using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralEventCalendarSearch_spParams
    {
        public System.DateTime? StartDate { get; set; }
        public System.DateTime? EndDate { get; set; }
        public int? AgencyID { get; set; }
        public int? ReferralTypeCodeID { get; set; }
        public int? AppearingPersonID { get; set; }
        public int? EventTypeCodeID { get; set; }
        public int? EventLocationCodeID { get; set; }
        public string LoadOption { get; set; }
        public int UserID { get; set; }
    }
    public class ref_ReferralEventCalendarSearch_spResult
    {
        public ref_ReferralEventCalendarSearch_spResult()
        {
        }
        public string GroupID { get; set; }
        public string GroupDisplay { get; set; }
        public string EventTime { get; set; }
        public string ReferralType { get; set; }
        public string EventType { get; set; }
        public string EventLocation { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string ClientPresent { get; set; }
        public string AppearingStaffAtty { get; set; }
        public string SortDate { get; set; }
        public int? ReferralEventID { get; set; }
        public int? ReferralID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
    }
}
