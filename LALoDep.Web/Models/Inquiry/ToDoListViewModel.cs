using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class ToDoListViewModel
    {
        [Display(Name = "Action Type")]
        public int? ActionTypeCodeID{ get; set; }
        [Display(Name="Status")]
        public int ActionStatusCodeID { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name="Date Type")]
        public int DateType { get; set; }
        public string UserName{ get; set; }
        public int? AssignedToPersonID { get; set; }
        public string DeleteIDList { get; set; }
        public string StatusChangeIDList { get; set; }
        public int ActionStatus { get; set; }
        public IEnumerable<KeyValuePair<string, string>> DateTypes
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("1", "Reminder"),
                    new KeyValuePair<string, string>("2", "Due"),
                };
            }
        }
        public IEnumerable<CodeViewModel> ActionTypes { get; set; }
        public IEnumerable<CodeViewModel> Status { get; set; }
    }
}