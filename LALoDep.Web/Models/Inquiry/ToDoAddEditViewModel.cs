using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class ToDoAddEditViewModel
    {
        public int PDActionID { get; set; }
        [Display(Name = "Action Type")]
        public int ActionTypeCodeID { get; set; }

        [Display(Name = "Action")]
        public string ActionNote { get; set; }

        [Display(Name = "Due Date")]
        public string ActionDueDate { get; set; }

        [Display(Name = "Reminder Date")]
        public string ActionReminderDate { get; set; }

        [Display(Name = "Case Name")]
        public int? CaseID { get; set; }

        [Display(Name = "Assign To Staff")]
        public int? AssignedToPersonID { get; set; }

        public string ActionStatusDate { get; set; }
        public int AgencyID { get; set; }
        public int? BranchID { get; set; }
        public int ActionStatusCodeID { get; set; }
        public int? ActionAssociatedToEntityID { get; set; }
        public int? ActionAssociatedToEntityTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public string ClientWithCaseNumber { get; set; }

        public List<CodeViewModel> ActionTypeList { get; set; }
        public List<CodeViewModel> CaseList { get; set; }
        public List<PersonViewModel> AssignedToPersonList { get; set; }
        public byte RecordStateID { get; set; }
        public ToDoAddEditViewModel()
        {
            ActionTypeList = new List<CodeViewModel>();
            CaseList = new List<CodeViewModel>();
            AssignedToPersonList = new List<PersonViewModel>();
        }
    }
}