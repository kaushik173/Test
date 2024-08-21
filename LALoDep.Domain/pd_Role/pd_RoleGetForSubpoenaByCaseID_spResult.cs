
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetForSubpoenaByCaseID_spResult
    {
        public int? RoleID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string RoleTypeCodeShortValue { get; set; }
        public string RoleTypeCodeValue { get; set; }
        public int? PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameMiddle { get; set; }
        public int? PersonAddressID { get; set; }
        public int? PersonAddressTypeCodeID { get; set; }
        public Int16? RecordStateID { get; set; }
        public string PersonAddressTypeCodeValue { get; set; }
        public string PersonAddressTypeCodeShortValue { get; set; }
        public int? AddressID { get; set; }
        public int? AgencyID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public string AddressStateCodeValue { get; set; }
        public string AddressStateCodeShortValue { get; set; }
        public string AddressCountryCodeValue { get; set; }
        public string AddressCountryCodeShortValue { get; set; }
    }
}
