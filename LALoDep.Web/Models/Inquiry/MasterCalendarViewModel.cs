using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Inquiry
{
    public class MasterCalendarViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? AgencyID { get; set; }
        public int? DepartmentID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public int? DesignatedDayCodeID { get; set; }
        public IEnumerable<AgencyModel> AgencyList { get; set; }
        public IEnumerable<CodeViewModel> DepartmentList { get; set; }
        public IEnumerable<CodeViewModel> DayOfWeekList { get; set; }
        public IEnumerable<CodeViewModel> HearingTypeList { get; set; }
    }
}