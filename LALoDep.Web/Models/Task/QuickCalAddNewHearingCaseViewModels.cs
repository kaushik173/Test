using LALoDep.Domain.HearingPrepNote;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.qcal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
   
    public class QuickCalAddNewHearingCaseViewModels
    {
        public List<qa_CaseloadGetForHearingAdd_spResult> HearingList { get; set; }
        public int AttorneyPersonID { get; set; }

        public QuickCalAddNewHearingCaseViewModels()
        {
            HearingList = new List<qa_CaseloadGetForHearingAdd_spResult>();
        }

    }
    public class HearingPreparationHoursViewModels
    {
        public List<HearingPrepNote_HoursGetHistory_spResult> HoursHistory { get; set; }
        public int HearingID { get; set; }

        public HearingPreparationHoursViewModels()
        {
            HoursHistory = new List<HearingPrepNote_HoursGetHistory_spResult>();
        }

    }
    
}