
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.ref_Referral
{

    public class ref_ReferralImmigrationIUD_spParams {
  public string IUD { get; set; }
  public int? ReferralImmigrationID { get; set; }
  public int? ReferralID { get; set; }
  public int? ImmigrationAgencyCodeID { get; set; }
  public int? ImmigrationAttorneyPersonID { get; set; }
  public DateTime? ImmigrationStartDate { get; set; }
  public DateTime? ImmigrationEndDate { get; set; }
  public int? UserID { get; set; }
  public System.Guid BatchLogJobID { get; set; }

        public int? Immigration317eCodeID { get; set; }
    }

}