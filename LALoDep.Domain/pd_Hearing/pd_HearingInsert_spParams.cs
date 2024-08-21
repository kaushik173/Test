using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingInsert_spParams
    {
        public int HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int CaseID { get; set; }
        public int HearingTypeCodeID { get; set; }
        public DateTime HearingDateTime { get; set; }
        public int HearingOfficerPersonID { get; set; }
        public int HearingCourtDepartmentCodeID { get; set; }
        public int? HearingRequestedByCodeID { get; set; }
        public int? HearingResultCodeID { get; set; }
        public int? HearingFollowedRecommendations { get; set; }
        public decimal? HearingInvoiceAmount { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int? HearingMediaPresentFlag { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class pd_HearingPersonInsertByPetitionID_spParams
    {
        public int HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int PetitionID { get; set; }
        public int? HearingPersonResultCodeID { get; set; }

        public int RecordStateID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        public int? AppearanceRequiredFlag { get; set; }

    }
    public class pd_HearingAttendanceInsert_spParams
    {
        public int? HearingAttendanceID { get; set; }
        public int AgencyID { get; set; }
        public int HearingID { get; set; }
        public int RoleID { get; set; }

        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int? NewAttendingAttorneyPersonID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        private int attendedFlag = 1;
        public int? AttendedFlag
        {
            get
            {

                return attendedFlag;
            }

            set
            {
                if (value.HasValue == false)
                    attendedFlag = 1;
                else
                    attendedFlag = value.Value;
            }
        }
    }

    public class pd_HearingPetitionUpdate_spParams
    {

        public int HearingID { get; set; }
        public int PetitionID { get; set; }
        public int HearingPetitionResultCodeID { get; set; }
        public int? OrderBackFlag { get; set; }
        public int? ASFAFlag { get; set; }
        public int? AppearanceWaivedFlag { get; set; }
        public int? NonOffendingFlag { get; set; }
        public int? IncarcerationFacilityCodeID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class pd_HearingPetitionDelete_spParams
    {
        public int ID { get; set; }
        public int ID2 { get; set; }
        public string RecordTimeStamp { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_HearingExpenseClosedGetByCaseID_spParams
    {
        public int CaseID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class pd_HearingPetitionAutoUpdate_spParams
    {
        public int HearingID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class pd_HearingDelete_spParams
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

        public string RecordTimeStamp { get; set; }
    }
    public class pd_WorkInsertByHearingID_spParams
    {
        public int HearingID { get; set; }
        public int AttorneyPersonID { get; set; }
        public decimal WorkHours { get; set; }
        public int WorkDescriptionCodeID { get; set; }
        public int WorkPhaseCodeID { get; set; }
        public int RecordStateID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }








}
