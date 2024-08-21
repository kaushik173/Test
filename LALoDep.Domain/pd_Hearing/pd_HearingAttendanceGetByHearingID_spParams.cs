using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingAttendanceGetByHearingID_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }
    }
    public class pd_HearingAttendanceGetByHearingID_spResult
    {
        public string PersonNameDisplay { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? RoleID { get; set; }
        public int? RoleTypeID { get; set; }
        public string RoleType { get; set; }
        public int? RoleClient { get; set; }
        public int? AttendanceID { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Fillin { get; set; }
        public int? Editable { get; set; }
        public int? AttendedFlag { get; set; }
        public int? RequiredFlag { get; set; }

        public int? CS_PersonID { get; set; }
        public int? CS_ID { get; set; }
        public int? CS_CodeID { get; set; }
        public string CS_StartDate { get; set; }
        public string CS_CodeDisplay { get; set; }
    }
}
