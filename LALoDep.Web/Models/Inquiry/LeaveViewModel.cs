using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    public class LeaveViewModel
    {
        public int? LeaveID { get; set; }
        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public int? LeaveTypeCodeID { get; set; }
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }

        public string RecordType { get; set; }
        public IEnumerable<SelectListItem> RecordTypeList { get; set; }

        public List<LeaveListViewModel> LeaveList { get; set; }

        public LeaveViewModel()
        {
            LeaveTypes = new List<SelectListItem>();
            RecordTypeList = new List<SelectListItem>();
            LeaveList = new List<LeaveListViewModel>();
        }
    }

    public class LeaveListViewModel
    {
        public int? LeaveID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveDateTimeDisplay { get; set; }
    }
}