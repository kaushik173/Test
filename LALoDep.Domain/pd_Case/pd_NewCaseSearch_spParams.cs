using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_NewCaseSearch_spParams
    {
        public int AgencyID { get; set; }
        public string DocketNumber { get; set; }
        public string DocketNumber2 { get; set; }
        public string DocketNumber3 { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherLastName { get; set; }
        public string MotherDOB { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

        public string ChildFirstName { get; set; }
        public string ChildLastName { get; set; }
        public string ChildDOB { get; set; }

        public string ChildFirstName2 { get; set; }
        public string ChildLastName2 { get; set; }
        public string ChildDOB2 { get; set; }


        public string ChildFirstName3 { get; set; }
        public string ChildLastName3 { get; set; }
        public string ChildDOB3 { get; set; }

        public string FatherFirstName { get; set; }
        public string FatherLastName { get; set; }
        public string FatherDOB { get; set; }

        public string FatherFirstName2 { get; set; }
        public string FatherLastName2 { get; set; }
        public string FatherDOB2 { get; set; }


      

    }
    public class qcal_MyCalendar_spParams
    {
        public int PersonID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        public int PendingHearingOnlyFlag { get; set; }





    }
    public class qcal_MyCalendar_spResult
    {
        public int? ClientPresentFlag { get; set; }
        public byte? HearingReadyFlag { get; set; }
        public string CaseNumber { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string HearingType { get; set; }
        public string HearingDateTime { get; set; }
        public string HearingDepartment { get; set; }
        public string Client { get; set; }
        public string CalendarNumber { get; set; }


        public string HearingResult { get; set; }
        public string AssignedAttorney { get; set; }
        public string CaseName { get; set; }
        public string TimeDisplay { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? AttachedFileCount { get; set; }
        public string AttachedFileDisplay { get; set; }
        public string AttachedFileCountDisplay { get; set; }
        public string AttachedFilePath { get; set; }
        public string SortCal { get; set; }
        public string SortDate { get; set; }
        public string SortTime { get; set; }
        public int? SortIsClient { get; set; }

    }
    public class qcal_MyCalendarAttorneyList_spResult
    {
        public string NameDisplay { get; set; }

        public int PersonID { get; set; }
        public byte? ShowClientPresentReadyFlag { get; set; }

    }
    public class qcal_MyCalendarAttorneyList_spParams
    {

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_ActivitySheet_spParams
    {
        public int CaseID { get; set; }
        public int HearingID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }




    }
    public class qcal_ActivitySheet_spResult
    {
        public string WorkDate { get; set; }
        public string WorkDescription { get; set; }
        public string WorkNote { get; set; }
        public string SortDate { get; set; }

    }
    public class
    qcal_MostRecentAR_spParams
    {
        public int CaseID { get; set; }
        public int HearingID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }




    }
    public class
    qcal_MostRecentAR_spResult
    {
        public int HearingReportFilingDueID { get; set; }


    }

    public class qcal_StatusBarHeader_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }




    }
    public class qcal_StatusBarHeader_spResult
    {
        public string CaseNumber { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string HearingType { get; set; }
        public string HearingDate { get; set; }
        public string HearingDeptCal { get; set; }
        public string CaseName { get; set; }

    }



    public class qcal_CalendarNumbering_spParams
    {
        public int? AgencyID { get; set; }
        public DateTime? HearingDate { get; set; }
        public int? DepartmentCodeID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        public string SortOption { get; set; }




    }
    public class qcal_CalendarNumberingUpdate_spParams
    {
        public int HearingID { get; set; }
        public string CalNbr { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }




    }

    public class qcal_CalendarNumbering_spResult
    {
        public string CaseName { get; set; }
        public string CaseNbr { get; set; }
        public string CalNbr { get; set; }
        public string HearingType { get; set; }
        public string Minors { get; set; }
        public int? HearingID { get; set; }


    }

    public class pd_NewCaseSearch_spResult
    {

        public int? KeySequence { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string DOB { get; set; }
        public string Agency { get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public int? RoleClient { get; set; }
        public string CaseNumber { get; set; }
        public string ClosedDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string LeadAttorney { get; set; }
        public int? CaseID { get; set; }
        public int? RoleID { get; set; }
        public string AgencyName { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalCases { get; set; }
    }

    public class pd_CaseInsert_spParams
    {
        public int CaseID { get; set; }
        public int AgencyID { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? CaseAppointmentDate { get; set; }
        public DateTime? CaseClosedDate { get; set; }
        public short CasePanelCase { get; set; }
        public int DepartmentID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }

        public int UserID { get; set; }


        public Guid BatchLogJobID { get; set; }

    }
    public class pd_CaseUpdate_spParams
    {
        public int CaseID { get; set; }
        public int AgencyID { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? CaseAppointmentDate { get; set; }
        public DateTime? CaseClosedDate { get; set; }
        public short CasePanelCase { get; set; }
        public int DepartmentID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }

        public int UserID { get; set; }


        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingGet_spResult
    {
        public int? OfficerPersonID { get; set; }
        public int? MediaPresentFlag { get; set; }
        public string CourtReporter { get; set; }
        public byte? NoticeProperFlag { get; set; }
        public byte? ReasonableEffortFlag { get; set; }
        public string GeneralHearingNote { get; set; }
        public string ClientType { get; set; }
        public int? CanAddAPOFFE { get; set; }
        public string CourtOfficer { get; set; }
        public string CSW { get; set; }
        public int? CSWPresentFlag { get; set; }
    }
    public class qcal_AS_HearingGet_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingAttendanceGet_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class qcal_AS_HearingAttendanceGet_spResult
    {
        public int ClientFlag { get; set; }
        public int? ChildFlag { get; set; }
        public int DisplayOrder { get; set; }
        public string NameDisplay { get; set; }
        public string NameDisplaySuffix { get; set; }
        public int? HearingAttendanceID { get; set; }
        public int? RoleID { get; set; }
        public int? AttendedFlag { get; set; }
        public int? CounselPersonID { get; set; }
        public int? FillinCounselPersonID { get; set; }
        public string Placement { get; set; }
        public int? PersonID { get; set; }
        public int? ICWAPersonClassificationID { get; set; }
        public int? ICWAFlag { get; set; }
        public int? DetPlacementPersonClassificationID { get; set; }
        public int? DetPlacementCodeID { get; set; }
        public DateTime? DetPlacementStartDate { get; set; }
        public int? HearingPersonID { get; set; }
        public int? HearingID { get; set; }
        public int? PetitionID { get; set; }
        public int? HearingPetitionResultCodeID { get; set; }
        public int? OrderBackFlag { get; set; }
        public int? ASFAFlag { get; set; }
        public int? AppearanceWaivedFlag { get; set; }
        public int? AppearanceRequiredFlag { get; set; }
        public int? NonOffendingFlag { get; set; }
        public string Allegation { get; set; }
        public string AllegationFinding { get; set; }
        public int? AllegationFindingCodeID { get; set; }
        public int? AllegationID { get; set; }
        public int? IncarcerationFacilityCodeID { get; set; }

        public int? DetPlacementHearingID { get; set; }
        public string DetPlacementComment { get; set; }

        public int? CS_PersonID { get; set; }
        public int? CS_ID { get; set; }
        public int? CS_CodeID { get; set; }
        public string CS_StartDate { get; set; }
        public string CS_CodeDisplay { get; set; }
        public string CurrentERH { get; set; }
        public int? HideCounselDropdownFlag { get; set; }
        public string RaceNeededLabel { get; set; }
    }
    public class qcal_AS_HearingCounselGetList_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingCounselGetList_spResult
    {
        public int PersonID { get; set; }
        public string FullName { get; set; }

    }
    public class qcal_AS_HearingNoteTypeGetNew_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingNoteTypeGetNew_spResult
    {
        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }

    }
    public class qcal_AS_HearingNoteGetList_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingNoteGetList_spResult
    {

        public string NoteLink { get; set; }
        public string NoteInfo { get; set; }
        public int NoteID { get; set; }
        public string SortDate { get; set; }
    }
    public class qcal_AS_HearingUpdate_spParams
    {
        public int HearingID { get; set; }
        public int HearingOfficerPersonID { get; set; }
        public int HearingMediaPresentFlag { get; set; }
        public string HearingCourtReporter { get; set; }
        public int HearingNoticeProperFlag { get; set; }
        public int HearingReasonableEffortFlag { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string HearingCourtOfficer { get; set; }
        public string HearingCSW { get; set; }
        public byte? HearingCSWPresentFlag { get; set; }

    }
    public class qcal_AS_HearingAttendanceIUD_spParams
    {
        public string IUD { get; set; }
        public int? HearingAttendanceID { get; set; }
        public int? HearingID { get; set; }
        public int? RoleID { get; set; }
        public int? AttendedFlag { get; set; }
        public int? CounselPersonID { get; set; }
        public int? FillinCounselPersonID { get; set; }
        public string Placement { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? AppearanceRequiredFlag { get; set; }
         
    }
    public class qcal_AS_PersonClassificationAddRemoveICWA_spParams
    {

        public int? PersonID { get; set; }
        public int? ICWAFlag { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class qcal_AS_NotePersonGetAll_spParams
    {
        public int HearingID { get; set; }
        public int NoteID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_NotePersonGetAll_spResult
    {

        public string PersonDisplay { get; set; }
        public int NotePersonID { get; set; }
        public int PersonID { get; set; }
        public int Selected { get; set; }
    }

    public class qcal_AS_PredeterminedAnswersGet_spParams
    {
        public int HearingID { get; set; }
        public int NoteTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class qcal_AS_PredeterminedAnswersGet_spResult
    {

        public string PredeterminedAnswer { get; set; }
        public int Seq { get; set; }


    }

    public class qcal_AS_HearingNoteIUD_spParams
    {
        public string IUD { get; set; }
        public int NoteID { get; set; }
        public int HearingID { get; set; }
        public int NoteTypeCodeID { get; set; }
        public string NoteEntry { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingNoteIUD_spParams2
    {
        public string IUD { get; set; }
        public int? NoteID { get; set; }
        public int HearingID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteEntry { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_HearingNoteIUD_spResult
    {

        public int? NoteID { get; set; }


    }
    public class qcal_AS_NotePersonIUD_spParams
    {
        public string IUD { get; set; }
        public int NoteID { get; set; }
        public int NotePersonID { get; set; }
        public int PersonID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class qcal_AS_DCCGetList_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_DCCGetList_spResult
    {
        public int PersonID { get; set; }
        public string FullName { get; set; }

    }
    public class qcal_AS_DCCGet_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_DCCGet_spResult
    {
        public string CurrentDCC { get; set; }
        public int? CurrentDCCPersonID { get; set; }
        public int? CurrentDCCRoleID { get; set; }
        public int? HearingAttendanceID { get; set; }
        public int? AttendedFlag { get; set; }
        public int? CounselPersonID { get; set; }
        public int? FillinCounselPersonID { get; set; }
    }
    public class qcal_AS_DCCSet_spParams
    {
        public int? CurrentDCCPersonID { get; set; }
        public int? HearingAttendanceID { get; set; }
        public int? HearingID { get; set; }
        public int? CurrentDCCRoleID { get; set; }

        public int? AttendedFlag { get; set; }
        public int? CounselPersonID { get; set; }
        public int? FillinCounselPersonID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
}
