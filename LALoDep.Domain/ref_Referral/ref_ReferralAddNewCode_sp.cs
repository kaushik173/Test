
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.ref_Referral
{

    public class ref_ReferralAddNewCode_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public string CodeType { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int? UserID { get; set; }

    }
    public class ref_ReferralAddNewCode_spResult
    {
        public int? CodeID { get; set; }
        public string CodeDisplay { get; set; }
    }

}