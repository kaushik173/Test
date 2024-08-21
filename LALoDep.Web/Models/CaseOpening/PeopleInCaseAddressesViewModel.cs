namespace LALoDep.Models.CaseOpening
{
    public class PeopleInCaseAddressesViewModel
    {
        public int? PersonAddressID { get; set; }
        public int? PersonAddressTypeCodeID { get; set; }
        public string PersonAddressStartDate { get; set; }
        public string PersonAddressEndDate { get; set; }
        public int? PersonAddressTypeDefault { get; set; }
        public string PersonAddressTypeCodeValue { get; set; }
        public string PersonAddressTypeCodeShortValue { get; set; }

        public int? RoleID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string RoleTypeCodeValue { get; set; }
        public string RoleTypeCodeShortValue { get; set; }
        public int? PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameMiddle { get; set; }

        public int? PersonAddressGlobalDefault { get; set; }
        public int? PersonAddressUserDefault { get; set; }
        public byte? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public string PersonAddressWorkPhone { get; set; }
        public int? PersonAddressConfidential { get; set; }
        
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
        
        public int? PersonAttributeID { get; set; }

        public string OrganizationName { get; set; }
        public int? SystemValueSequence { get; set; }
      
    
    }



}