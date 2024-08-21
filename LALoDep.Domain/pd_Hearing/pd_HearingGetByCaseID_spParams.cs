using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingGetByCaseID_spParams
    {

        public int CaseID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        public int UnresultedFlag { get; set; }

    }
    public class pd_HearingGetByCaseID_spResults
    {

        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public System.DateTime? HearingDateTime { get; set; }
        public int? HearingOfficerPersonID { get; set; }
        public int? HearingCourtDepartmentCodeID { get; set; }
        public int? HearingRequestedByCodeID { get; set; }
        public int? HearingResultCodeID { get; set; }
        public int? HearingFollowedRecommendations { get; set; }
        public decimal? HearingInvoiceAmount { get; set; }
        public short? RecordStateID { get; set; }

        public string HearingOfficerFirstName { get; set; }
        public string HearingOfficerLastName { get; set; }
        public string HearingTypeCodeValue { get; set; }
        public string HearingTypeCodeShortValue { get; set; }
        public string HearingCourtDepartmentCodeValue { get; set; }
        public string HearingRequestedByCodeValue { get; set; }
        public string HearingNote { get; set; }
        public int? NoteID { get; set; }
        public string AppearingAttorneyFirstName { get; set; }
        public string AppearingAttorneyLastName { get; set; }
        public decimal? AppearingAttorneyHours { get; set; }
        public string HearingDisplay { get; set; }
        public int? WorkFlag { get; set; }
        public int? NoDeleteFlag { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
    }
    public class pd_HearingOfficerGet_spParams
    {
        public int? CaseID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        public int? AgencyID { get; set; }


    }
    public class pd_HearingOfficerGet_spResults
    {

        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int ActiveFlag { get; set; }
   

    }
}
