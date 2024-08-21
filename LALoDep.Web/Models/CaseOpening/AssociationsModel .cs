using System.Collections.Generic;
using System.Web.Mvc;
using LALoDep.Domain.pd_Allegation;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.CaseOpening
{
    public class AssociationsModel
    {

        public int? PersonID { get; set; }
        public IEnumerable<SelectListItem> PersonList { get; set; }
        public int? AssociationTypeID { get; set; }
        public string SelectedRelatedPersonIds { get; set; }
        public IEnumerable<SelectListItem> AssociationTypeList { get; set; }

        public IEnumerable<pd_RoleGetByCaseIDForAssociationRelatedTo_spResults> RelatedPersonList { get; set; }


        public IEnumerable<pd_AssociationGetByCaseID_spResult> AssociationsInCase { get; set; }

        public IEnumerable<pd_AssociationSuggestByCaseID_spResult> AssociationSuggestions { get; set; }
        public IEnumerable<AssociateSuggestionModel> SelectedAssociationSuggestions { get; set; }
        public AssociationsModel()
        {
            AssociationsInCase = new List<pd_AssociationGetByCaseID_spResult>();
            RelatedPersonList = new List<pd_RoleGetByCaseIDForAssociationRelatedTo_spResults>();
            SelectedAssociationSuggestions = new List<AssociateSuggestionModel>();
            AssociationSuggestions = new List<pd_AssociationSuggestByCaseID_spResult>();
        }
    }

    public class AssociateSuggestionModel
    {
      

        public int PersonID1 { get; set; }
     
        public string StartDate { get; set; }
        public int PersonID2 { get; set; }
        public int AssociationTypeID { get; set; }
    }
}