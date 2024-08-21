using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Calendar;
using System.ComponentModel.DataAnnotations;
using LALoDep.Domain.NG_com;

namespace LALoDep.Models.Case
{
    public class MoreInfoOnPetitionViewModel
    {
        public int PetitionID { get; set; }
        [Display (Name="File Date")]
        public string PetitionFileDate { get; set; }
        [Display (Name="Close Date")]
        public string CloseDate { get; set; }
        [Display (Name="Case #")]
        public string PetitionDocketNumber { get; set; }
        [Display(Name = "Type")]
        public string PetitionTypeCodeValue { get; set; }
        [Display(Name = "Child")]
        public string Child { get; set; }
        public List<PetitionCalendarListViewModel> PetitionList { get; set; }

        public MoreInfoOnPetitionViewModel()
        {
            PetitionList = new List<PetitionCalendarListViewModel>();
        }
    }

    public class GoToViewModel
    {
       
        public int HearingID { get; set; }
        public List<NG_GoToNavigation_spResult> NavigationList { get; set; }

        public GoToViewModel()
        {
            NavigationList = new List<NG_GoToNavigation_spResult>();
        }
    }
}