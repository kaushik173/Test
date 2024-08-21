using System.ComponentModel.DataAnnotations;

namespace LALoDep.Areas.Mobile.Models
{
    public class MyCalendarViewModel
    {
        [Display(Name="Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public string AttorneyPersonName { get; set; }
    }
}