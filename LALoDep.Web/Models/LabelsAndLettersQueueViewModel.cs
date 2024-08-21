using System.Collections.Generic;

namespace Jcats.SD.UI.ViewModels
{
    public class MyCalendarModel
    {
        public string CalendarView { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public int? HearingTypeId { get; set; }

        public List<HearingTypeViewModel> HearingType { get; set; }

        public bool PendingHearingsOnly { get; set; }
        public int AttorneyPersonId { get; set; }
        public string AttorneyPersonName { get; set; }

        public MyCalendarModel()
        {

            HearingType = new List<HearingTypeViewModel>();

        }
    }

    public class HearingTypeViewModel
    {
        public int HearingTypeId { get; set; }
        public string HearingTypeName { get; set; }
    }

    public class CalendarModel
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string url { get; set; }

        public string borderColor { get; set; }
        public string description { get; set; }


    }


}