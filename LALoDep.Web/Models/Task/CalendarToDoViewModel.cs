using LALoDep.Domain.PD_PDAction;
using LALoDep.Domain.qcal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class CalendarToDoViewModel
    {
        public pd_PDActionGet_spResults PdActionOldModel { get; set; }
        public string Heading { set; get; }
        public List<SelectListItem> ActionTypeList { get; set; }
        public List<qcal_ToDo_History_spResult> ToDoList { get; set; }
        public int? ActionTypeID { set; get; }
        public string ActionNote { set; get; }

        public string DueDate { set; get; }

        public string ReminderDate { set; get; }

        public int? PDActionID { set; get; }
        public int HearingID { get; set; }
        public List<SelectListItem> AssignToStaffList { get; set; }
        public int? AssignToStaffID { set; get; }
        public CalendarToDoViewModel()
        {
            ActionTypeList = new List<SelectListItem>();
            AssignToStaffList = new List<SelectListItem>();
        }

        public string PDActionCompletedIds { get; set; }
    }


}