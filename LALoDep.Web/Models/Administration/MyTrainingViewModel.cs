using LALoDep.Domain.pd_TrainingSummary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class MyTrainingViewModel
    {
        public int? PersonID { get; set; }
        public string PersonName { get; set; }
        [Display (Name="Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        public List<pd_TrainingGetSummaryByPersonIDStartDateEndDate_spResult> TrainingSummary { get; set; }
        public List<pd_TrainingGetByPersonIDStartDateEndDate_spResult> TrainingByPerson { get; set; }
        public IEnumerable<SelectListItem> VenueList { get; set; }
        public int? VenueID { get; set; }
        public MyTrainingViewModel()
        {
            TrainingSummary = new List<pd_TrainingGetSummaryByPersonIDStartDateEndDate_spResult>();

            TrainingByPerson = new List<pd_TrainingGetByPersonIDStartDateEndDate_spResult>();
        }
    }
}