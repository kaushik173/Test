using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetByCaseIDChildRespondent_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }


    }
    public class pd_RoleGetByCaseIDChildRespondent_spResult
    {
         
        public int CaseNameFlag { get; set; }
        public int RoleID { get; set; }
        public int CaseID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public int PersonID { get; set; }
        public int AgencyID { get; set; }
         public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public System.DateTime? Persondob { get; set; }
        public int? PersonSexCodeID { get; set; }
        public string Role { get; set; }
        public byte RoleClient { get; set; }
        public System.DateTime? RoleStartDate { get; set; }
        public System.DateTime? RoleEndDate { get; set; }
        public int? RespondentFlag { get; set; }
        public int? ChildFlag { get; set; }
        public string DisplayName { get; set; }
        public int? AKACount { get; set; }
    }

    public class pd_RoleGetForHearingAttendingAttorney_spParams
    {
        public int UserID { get; set; }
        public int CaseID { get; set; }
        public Guid BatchLogJobID { get; set; }
            public int HearingID { get; set; }
     
    }

    public class pd_RoleGetForHearingAttendingAttorney_spResult
    {
        public string PersonNameDisplay { get; set; }
        public int RoleID { get; set; }
        public int OnCase { get; set; }
        public int PersonID { get; set; }
        public int AttendingHearingFlag { get; set; }
        public int HearingAttendanceID { get; set; }
    }

}
