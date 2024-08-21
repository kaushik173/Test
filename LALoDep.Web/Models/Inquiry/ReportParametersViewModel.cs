using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using LALoDep.Models.Administration;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    public class ReportParametersViewModel
    {
        public int ReportSourceID { get; set; }
        public int ReportID { get; set; }
        public int ExportOption { get; set; }
        public int ReportSourceSequence { get; set; }
        public string ReportSourceDocumentName { get; set; }
        public string ReportSourceStoredProcedureName { get; set; }
        public string ReportDisplayName { get; set; }
        public string CaseNumber { get; set; }
        public string JcatsNumber { get; set; }
        public int BranchID { get; set; }

        //Unique Lists
        public List<ReportSourceList> ReportSourceList { get; set; }
        public List<ReportParameterDefinition> ParameterDefinitionList { get; set; }
        public List<ReportParameterAttorneyListViewModel> AttorneyList { get; set; }
        public List<ReportParameterInvestigatorListViewModel> InvestigatorList { get; set; }
        public List<AgencyModel> AgencyList { get; set; }
        public List<AgencyGroupModel> AgencyGroupList { get; set; }
        public List<AgencyCountyModel> CountyList { get; set; }
        public List<AgencyRegionModel> RegionList { get; set; }
        public List<CodeTypeViewModel> CodeTypeList { get; set; }

        //Code Lists
        public List<CodeViewModel> GenderList { get; set; }
        public List<CodeViewModel> HearingTypeList { get; set; }
        public List<CodeViewModel> DepartmentList { get; set; }
        public List<CodeViewModel> SortBy { get; set; }
        public List<CodeViewModel> ReportTypeList { get; set; }
        public List<CodeViewModel> ServicePlanningAreaList { get; set; }
        public List<CodeViewModel> ClientTypeList { get; set; }
        public List<CodeViewModel> GroupingList { get; set; }
        public List<CodeViewModel> ReferralSourceList { get; set; }
        public List<CodeViewModel> RoleTypeList { get; set; }
        public List<CodeViewModel> AllegationList { get; set; }
        public List<CodeViewModel> AllegationFindingList { get; set; }
        public List<CodeViewModel> ReportCountTypeList { get; set; }
        public List<CodeViewModel> QuarterList { get; set; }
        public List<CodeViewModel> DesignatedDayList { get; set; }
        public List<CodeViewModel> CaseTypeList { get; set; }
        public List<CodeViewModel> ARTypeList { get; set; }
        /*//CheckBox MultiSelects
        public List<CodeSelectedViewModel> SpecialNeedsList { get; set; }
        public List<CodeSelectedViewModel> AddressTypeList { get; set; }
        public List<CodeSelectedViewModel> WorkTaskList { get; set; }
        public List<CodeSelectedViewModel> PhaseList { get; set; }*/

        public string ComboSelectedValue { get; set; }

        public IEnumerable<SelectListItem> ComboItemList { get; set; }
        public ReportParametersViewModel()
        {
            ComboItemList = new List<SelectListItem>();
            ReportSourceList = new List<ReportSourceList>();
            ParameterDefinitionList = new List<ReportParameterDefinition>();
            AttorneyList = new List<ReportParameterAttorneyListViewModel>();
            CodeTypeList = new List<CodeTypeViewModel>();
            ServicePlanningAreaList = new List<CodeViewModel>();
            InvestigatorList = new List<ReportParameterInvestigatorListViewModel>();
            GenderList = new List<CodeViewModel>();
            AgencyList = new List<AgencyModel>();
            CountyList = new List<AgencyCountyModel>();
            RegionList = new List<AgencyRegionModel>();
            AgencyGroupList = new List<AgencyGroupModel>();
            ServicePlanningAreaList = new List<CodeViewModel>();
            ClientTypeList = new List<CodeViewModel>();
            GroupingList = new List<CodeViewModel>();
            ReferralSourceList = new List<CodeViewModel>();
            ReportTypeList = new List<CodeViewModel>();
            HearingTypeList = new List<CodeViewModel>();
            RoleTypeList = new List<CodeViewModel>();
            AllegationList = new List<CodeViewModel>();
            AllegationFindingList = new List<CodeViewModel>();
            ReportCountTypeList = new List<CodeViewModel>();
            DepartmentList = new List<CodeViewModel>();
            QuarterList = new List<CodeViewModel>();
            DesignatedDayList = new List<CodeViewModel>();
            SortBy = new List<CodeViewModel>();
            CaseTypeList = new List<CodeViewModel>();
            /*
            SpecialNeedsList = new List<CodeSelectedViewModel>();
            AddressTypeList = new List<CodeSelectedViewModel>();
            WorkTaskList = new List<CodeSelectedViewModel>();
            PhaseList = new List<CodeSelectedViewModel>();
            */
        }
    }
}