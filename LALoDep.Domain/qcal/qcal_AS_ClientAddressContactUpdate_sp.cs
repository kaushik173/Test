
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.qcal
{

    public class qcal_AS_ClientAddressContactUpdate_spParams
    {
        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? PersonAddressID { get; set; }
        public int? AddressID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int? StateCodeID { get; set; }
        public string ZipCode { get; set; }
        public string HomePhone { get; set; }
        public int? MobilePhonePersonContactID { get; set; }
        public string MobilePhone { get; set; }
        public int? WorkPhonePersonContactID { get; set; }
        public string WorkPhone { get; set; }
        public int? EmailAddressPersonContactID { get; set; }
        public string EmailAddress { get; set; }
        public int? PreferenceID { get; set; }
        public int? PreferenceCodeID { get; set; }
        public string PreferenceComment { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}