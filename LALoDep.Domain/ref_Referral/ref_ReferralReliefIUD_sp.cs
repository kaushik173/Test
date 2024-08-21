
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.ref_Referral
{

    public class ref_ReferralReliefIUD_spParams {
  public string IUD { get; set; }
  public int? ReferralReliefID { get; set; }
  public int? ReferralID { get; set; }
  public int? ReferralReliefTypeCodeID { get; set; }
  public int? ReferralReliefStatusCodeID { get; set; }
  public DateTime? ReferralReliefStatusDate { get; set; }
  public string ReferralReliefNote { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

        public DateTime? ReferralReliefPriorityDate { get; set; }
    }

}