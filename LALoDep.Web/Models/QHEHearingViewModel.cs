using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Petition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models
{
    public class QHEHearingViewModel
    {
        public string ControlType { get; set; }

        public int? HearingID { get; set; }
        public int HearingTypeID { get; set; }
        public IEnumerable<SelectListItem> HearingTypeList { get; set; }
        public string HearingDate { get; set; }
        public string HearingTime { get; set; }
        public int HearingOfficerID { get; set; }
        public IEnumerable<SelectListItem> HearingOfficerList { get; set; }
        public int DepartmentID { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public string AppearingAttorneyID { get; set; }
        public IEnumerable<SelectListItem> AppearingAttorneyList { get; set; }

        public List<pd_PetitionGetByCaseID_spResult> PetitionList { get; set; }
        public List<pd_HearingGetByCaseID_spResults> HearingList { get; set; }

        public bool UpdateCaseDepartment { get; set; }
        
    }

    public class QHENextHearingSaveViewModel
    {
        public List<QHEHearingViewModel> Hearings { get; set; }
        public List<pd_PetitionGetByCaseID_spResult> PetitionList { get; set; }

        public int? HearingID { get; set; }
        public int buttonID { get; set; }
        public QHENextHearingSaveViewModel()
        {
            Hearings = new List<QHEHearingViewModel>();
            PetitionList = new List<pd_PetitionGetByCaseID_spResult>();            
        }
    }
}