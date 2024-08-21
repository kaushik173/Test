using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.pd_Association;
using System.ComponentModel.DataAnnotations;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.Case
{
    public class MoreInfoOnPersonViewModel
    {
        public List<pd_RoleGetForPersonMoreInfo_spResult> PersonRoles { get; set; }
        public List<pd_AddressGetForPersonMoreInfo_spResult> Addresses { get; set; }
        public List<pd_PersonContactGetByPersonID_spResult> Contacts { get; set; }
        public List<pd_LegalNumberGetByPersonID_spResult> LegalNumbers { get; set; }        
        public List<pd_AssociationGetForPersonMoreInfo_spResult> Associations { get; set; }
        public MoreInfoOnPersonViewModel()
        {
            PersonRoles = new List<pd_RoleGetForPersonMoreInfo_spResult>();
            Addresses = new List<pd_AddressGetForPersonMoreInfo_spResult>();
            Associations = new List<pd_AssociationGetForPersonMoreInfo_spResult>();
            
        }
    }
}