using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MasterCalendar
{
   public class pd_MasterCalendarGet_spParams
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? AgencyID { get; set; }
        public int? DepartmentID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public int? DesignatedDayCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
