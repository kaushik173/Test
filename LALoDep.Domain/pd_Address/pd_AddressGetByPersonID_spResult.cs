using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Address
{
  public  class pd_AddressGetByPersonID_spResult
    {
        public int? PersonID { get; set; }
        public int? PersonAddressTypeCodeID { get; set; }
        public string PersonAddressWorkPhone { get; set; }
        public DateTime? PersonAddressStartDate { get; set; }
        public DateTime? PersonAddressEndDate { get; set; }
        public int? AddressID { get; set; }
        public int? AgencyID { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressHomePhone { get; set; }
        public int? PlacementAgencyCodeID { get; set; }
        public string State { get; set; }
        public string AddressType { get; set; }
        public string Country { get; set; }
        public string PlacementAgency { get; set; }
        public string PersonName { get; set; }
    }
}
