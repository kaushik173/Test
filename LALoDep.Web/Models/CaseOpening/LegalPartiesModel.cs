using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.CaseOpening
{
    public class LegalPartiesSelectedRoleModel
    {


        public int RoleTypeCodeID { get; set; }
        public int PersonID { get; set; }
        public  DateTime StartDate { get; set; }
        public string SelectedAttorneyAssociate { get; set; }
    }

    public class LegalPartiesModel
    {

        public bool AllowTransfer { get; set; }

        public int? NewRoleID { get; set; }
        public IEnumerable<SelectListItem> NewRoleList { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string RoleStartDate { get; set; }
        public int AddressTypeID { get; set; }
        public IEnumerable<SelectListItem> AddressTypeList { get; set; }

        public string AddrStartDate { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int StateID { get; set; }
        public IEnumerable<CodeViewModel> StateList { get; set; }
        public string ZipCode { get; set; }
        public string AddressPhone { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }

        public string AssociationForNewRoleSelectedIds { get; set; }


        public IEnumerable<pd_RoleGetLegalAll_spResults> LegalPartiesList { get; set; }
        public int AssociationForNewRoleAssociationTypeID { get; set; }
        public IEnumerable<SelectListItem> AssociationForNewRoleAssociationTypeList { get; set; }
        public IEnumerable<pd_RoleGetByCaseIDForAssociationAttorney_spResults> AssociationAttorneyList { get; set; }
        public IEnumerable<pd_RoleGetByCaseIDForAssociationRelatedTo_spResults> AssociationRelatedToList { get; set; }
        public IEnumerable<pd_RoleGetLegalSpecific_spResult> RoleGetLegalSpecificList { get; set; }

        public IEnumerable<pd_RoleGetLegalStatus_spResult> RoleLegalStatusList { get; set; }
        public IEnumerable<LegalPartiesSelectedRoleModel> LegalPartiesSelectedRoleList { get; set; }

        
        public LegalPartiesModel()
        {
            StateList = new List<CodeViewModel>();
            AddressTypeList = new List<SelectListItem>();
            NewRoleList = new List<SelectListItem>();//pd_RoleGetLegalFreeFormGeneral_spResults
            LegalPartiesList = new List<pd_RoleGetLegalAll_spResults>();
            AssociationForNewRoleAssociationTypeList = new List<SelectListItem>();
            AssociationAttorneyList = new List<pd_RoleGetByCaseIDForAssociationAttorney_spResults>();
            AssociationRelatedToList = new List<pd_RoleGetByCaseIDForAssociationRelatedTo_spResults>();
            RoleGetLegalSpecificList = new List<pd_RoleGetLegalSpecific_spResult>();

            LegalPartiesSelectedRoleList = new List<LegalPartiesSelectedRoleModel>();
        }

    }
}