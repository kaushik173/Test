
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.ref_Referral
{

    public class ref_ReferralEventIUD_spParams
    {
        public string IUD { get; set; }
        public int? ReferralEventID { get; set; }
        public int? ReferralID { get; set; }
        public DateTime? ReferralEventDateTime { get; set; }
        public int? ReferralEventTypeCodeID { get; set; }
        public int? ReferralEventLocationCodeID { get; set; }
        public int? ReferralEventAppearingPersonID { get; set; }
        public short? ReferralEventClientPresentFlag { get; set; }
        public string ReferralEventNote { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}