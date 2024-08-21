using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_TrainingSummary;

namespace LALoDep.Models.Administration
{
    public class TrainingEditViewModel
    {
        public IEnumerable<SelectListItem> VenueList { get; set; }
        public int? VenueID { get; set; }
        public int AgencyID { get; set; }
        public int? TrainingID { get; set; }
        public string CourseTitle { get; set; }
        public string TrainingProvider { get; set; }
        public string SubjectMatter { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Participatory { get; set; }
        public decimal? Hours { get; set; }
        public int? CreditTypeID { get; set; }
        public Int16 RecordStateID { get; set; }
        public pd_TrainingGet_spResult Training { get; set; }
        public pd_PersonGet_spResult Person { get; set; }
        public IEnumerable<SelectListItem> CreditTypeList { get; set; }
        public int? TrainingIVeEligibleCodeID { get; set; }
        public IEnumerable<SelectListItem> TrainingIVeEligibleCodeList { get; set; }
        
    }
}