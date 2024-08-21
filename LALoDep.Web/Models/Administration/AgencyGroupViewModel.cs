using LALoDep.Domain.TitleIVe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class AgencyGroupViewModel: TitleIVeAgencyGroupGet_spResult
    {
        public AgencyGroupViewModel()
        {
            AgencyGroupList = new List<SelectListItem>();
            CountyList = new List<SelectListItem>(); 
             AgencyGroupCountyList = new List<TitleIVeAgencyGroupGet_spResult>();
        }        
        public IEnumerable<SelectListItem> AgencyGroupList { get; set; }
        public bool IsUseWorkHoursForActivityLog { get; set; }
        public IEnumerable<TitleIVeAgencyGroupGet_spResult> AgencyGroupCountyList { get; set; }

        public int? AgencyGroupID { get; set; }
        public IEnumerable<SelectListItem> CountyList { get; set; }

    }

    public class AgencyGroupAllocationViewModel
    {
        public List<TitleIVeAgencyGroupCountyAllocationGet_spResult> TitleIVeAgencyGroupCountyAllocationList { get; set; }
        public AgencyGroupAllocationViewModel()
        {
            TitleIVeAgencyGroupCountyAllocationList = new List<TitleIVeAgencyGroupCountyAllocationGet_spResult>();
        }
        
        public int? AgencyGroupID { get; set; }

    }
}