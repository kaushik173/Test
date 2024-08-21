
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.ref_Referral
{

    public class ref_ReferralAddNewStaff_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public string StaffType { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? UserID { get; set; }

    }
    public class ref_ReferralAddNewStaff_spResult
    {
        public int? PersonID { get; set; }
        public string NameDisplay { get; set; }
      
    }
}