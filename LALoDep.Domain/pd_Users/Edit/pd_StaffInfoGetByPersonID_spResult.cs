using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users.Edit
{
    public class pd_StaffInfoGetByPersonID_spResult
    {
        public int StaffInfoID { get; set; }
        public int PersonID { get; set; }
        public int AlternateContactPersonID { get; set; }
        public string StaffInfoBarNumber { get; set; }
        public string StaffInfoComment { get; set; }
        public string EmailPrimary { get; set; }
        public string EmailSecondary { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string StaffInfoBarAdmittedDate { get; set; }
        public string StaffInfoEligibilityEffectiveDate { get; set; }
        public string StaffInfoEligibilityEndingDate { get; set; }
        public int EmailToPrimaryPersonContactID { get; set; }
        public int EmailToSecondaryPersonContactID { get; set; }
        public int EmailToAlternatePersonContactFlag { get; set; }
        public int? StaffInfoEmployeeStatusCodeID { get; set; }
        public string StaffInfoEmployeeID { get; set; }

        public int? FaxPersonContactID { get; set; }
        public int? MobilePhonePersonContactID { get; set; }
        public int? WorkPhonePersonContactID { get; set; }
        public int? EmailPrimaryPersonContactID { get; set; }
        public int? EmailSecondaryPersonContactID { get; set; }


    }
}
