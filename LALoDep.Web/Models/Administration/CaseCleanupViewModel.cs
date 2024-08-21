using LALoDep.Domain.qcal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class CaseCleanupViewModel
    {
        [Display(Name = "Agency")]
        public int? AgencyID { get; set; }

        [Display(Name = "Attorney")]
        public int? PersonID { get; set; }

        public IEnumerable<SelectListItem> Agencies { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
    }
    public class PredeterminedAnswersViewModel
    {
        [Display(Name = "Agency")]
        public int? AgencyID { get; set; }
        [Display(Name = "Agency Group")]
        public int? AgencyGroupID { get; set; }

        [Display(Name = "Note Type")]
        public int? NoteTypeID { get; set; }
        [Display(Name = "Hearing Type Group")]
        public int? HearingTypeGroupID { get; set; }
        [Display(Name = "Client Type")]
        public string ClientTypeID { get; set; }

        [Display(Name = "Hearing Type")]
        public int? HearingTypeID { get; set; }
        public IEnumerable<SelectListItem> ClientTypeList { get; set; }

        public IEnumerable<SelectListItem> HearingTypeList { get; set; }

        public IEnumerable<SelectListItem> HearingTypeGroupList { get; set; }

        public IEnumerable<SelectListItem> NoteTypeList { get; set; }

        public IEnumerable<SelectListItem> Agencies { get; set; }
        public IEnumerable<SelectListItem> AgencyGroupList { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
    }
    public class PredeterminedAnswersAddEditModel
    {
        public bool InactiveFlag { get; set; }
        public bool ChildClients { get; set; }
        public bool AdultClients { get; set; }
        public int? Seq { get; set; }
        public string PredeterminedAnswer { get; set; }
        public string ShortValue { get; set; }
        public int? QuickNoteID { get; set; }


        public IEnumerable<PredeterminedAnswersDependent> AddNodeTypes { get; set; }
        public IEnumerable<PredeterminedAnswersDependent> AddAgencies { get; set; }
        public IEnumerable<PredeterminedAnswersDependent> AddHearingTypes { get; set; }

        public IEnumerable<QuickNoteGetHearingTypes_spResult> HearingTypeList { get; set; }

        public IEnumerable<QuickNoteGetNoteTypes_spResult> NoteTypeList { get; set; }

        public IEnumerable<QuickNoteGetAgencies_spResult> Agencies { get; set; }
    }
    public class MergeTemplateViewModel
    {
        [Display(Name = "Agency")]
        public int? AgencyID { get; set; }
       
        public IEnumerable<SelectListItem> Agencies { get; set; }
     }

    public class PredeterminedAnswersDependent
    {
        public int CodeID { get; set; }
        public string IUD { get; set; }

        public int QuickNoteDependentID { get; set; }
    }
    public class CaseCleanupResultViewModel
    {
        public string SearchType { get; set; }
        public List<CaseDetail> CaseList { get; set; }
    }

    public class CaseDetail
    {
        public string ASPActionDisplay { get; set; }
        public string ASPAction { get; set; }
        public string NG_NavigationURL { get; set; }
        public int? SortOrder { get; set; }
        public int CaseID { get; set; }
        public string Client { get; set; }
        public string CaseName { get; set; }
        public string PetitionNumber { get; set; }
        public string CaseNumber { get; set; }
        public string Attorney { get; set; }
        public string InsertedOn { get; set; }
        public string InsertedBy { get; set; }
        public string SortInsetedOn { get; set; }
        public string CustomSort { get; set; }
    }
}