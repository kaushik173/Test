using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_TrainingSummary;

namespace LALoDep.Models.Administration
{
    public class TrainingViewModel
    {
        public int? VenueID { get; set; }
        public int PersonID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool OnViewLoad { get; set; }
        public pd_PersonGet_spResult Person { get; set; }
        public List<pd_TrainingGetSummaryByPersonIDStartDateEndDate_spResult> TrainingSummaryList{get; set; } 
    }
}