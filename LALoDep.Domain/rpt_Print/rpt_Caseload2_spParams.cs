using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.rpt_Print
{
    public class rpt_Caseload2_spParams
    {
        public string Casestatus { get; set; }
        public DateTime? AppointmentStartDate { get; set; }
        public DateTime? AppointmentEndDate { get; set; }
        public int? HearingTypeID { get; set; }
        public DateTime? HearingDate { get; set; }
        public int? AttorneyID { get; set; }
        public int? InvestigatorID { get; set; }
        public int? LegalAssistantID { get; set; }
        public string RoleType { get; set; }
        public string SortOption { get; set; }
        public int? DepartmentID { get; set; }
        public int AgencyID { get; set; }
        public int? DesignatedDayCodeID { get; set; }
        public byte? PlacedWithParentFlag { get; set; }
        public int? AddressTypeCodeID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
