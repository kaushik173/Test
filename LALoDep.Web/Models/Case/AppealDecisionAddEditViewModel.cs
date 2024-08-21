using LALoDep.Domain.pd_Appeals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class AppealDecisionAddEditViewModel
    {
        public IEnumerable<SelectListItem> Decisions { get; set; }
        public List<pd_DecisionGetByAppealID_spResult> DecisionList { get; set; }
        [Display(Name = "Decision")]
        public int? DecisionCodeID { get; set; }
        public int DecisionID { get; set; }

        [Display(Name = "Date")]
        public string DecisionDate { get; set; }
        public string EncryptedAppleaID { get; set; }
        
        public int? AgencyID { get; set; }
        public int? RecordStateID { get; set; }
        public AppealDecisionAddEditViewModel()
        {
            DecisionList = new List<pd_DecisionGetByAppealID_spResult>();
        }
    }
}