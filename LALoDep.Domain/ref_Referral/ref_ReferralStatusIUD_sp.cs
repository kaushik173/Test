
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.ref_Referral
{

    public class ref_ReferralStatusIUD_spParams
    {
        public string IUD { get; set; }
        public int? ReferralStatusID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralStatusCodeID { get; set; }
        public DateTime? ReferralStatusDate { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}