using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Mobile
{
    public class MobileIndividualCalendar_spResult
    {
        public int? HearingID { get; set; }
        public string SortEventDateTime { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string HearingType { get; set; }
        public string Clients { get; set; }

        public int? CaseID { get; set; }

    }
    public class MobileIndividualCalendar_spParams
    {
        public int? PersonID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
    public class MobileCaseInfoGet_spParams
    {
        public int CaseID { get; set; }
        public int AttorneyPersonID { get; set; }

        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }


    public class MobileCaseInfoGet_spResult
    {

        public int? CaseID { get; set; }
        public string KeyNoteSubject { get; set; }
        public string KeyNoteEntry { get; set; }
        public int? NextHearingID { get; set; }
        public string NextHearing { get; set; }
    }

    public class MobileCaseInfoGetHearings_spParams
    {
        public int CaseID { get; set; }

        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
    public class MobileCaseInfoGetHearings_spResult
    {

        public int HearingID { get; set; }
        public string HearingInfo { get; set; }
        public string SortDateTime { get; set; }
    }
    public class MobileRoleInfoGet_spParams
    {
        public int CaseID { get; set; }
        public int AttorneyPersonID { get; set; }

        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
    public class MobileRoleInfoGet_spResult
    {

        public int PersonID { get; set; }
        public string PersonDisplay { get; set; }
        public string AddressDisplay1 { get; set; }
        public string AddressDisplay2 { get; set; }
        public string AddressDisplay3 { get; set; }
        public string AddressDisplay4 { get; set; }
    }





    public class MobileCaseSearch_spParams
    {
        public int AttorneyPersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PetitionNumber { get; set; }

        public short OnlyOpenCases { get; set; }

        public Guid GUID { get; set; }

        public int StartRecord { get; set; }
        public int Range { get; set; }

        public int TotalRecords { get; set; }

        public string SortOption { get; set; }



        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
    public class MobileCaseSearch_spResult
    {

        public int wtMobileCaseSearchSequence { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int PetitionID { get; set; }
        public int RoleID { get; set; }
        public int PersonID { get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public string PersonNameDisplay { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string CaseNumber { get; set; }
        public int RoleClient { get; set; }
        public int CaseID { get; set; }
        public string DOB { get; set; }
        public string ClosedDate { get; set; }
        public int TotalRecords { get; set; }
        public System.Guid wtMobileCaseSearchGUID { get; set; }
        public string LeadAttorney { get; set; }
    }
    public class pd_MobileCodesGet_spParams
    {

        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string SystemValueIDList { get; set; }
        public int? CodeTypeID { get; set; }
        public int? IncludeCodeID { get; set; }
        public int? CodeDisplayLength { get; set; }
        public string CodePaddingChar { get; set; }
        public string LoadOption { get; set; }
        public string SortOption { get; set; }
        public int  UserID { get; set; }
        public Guid  BatchLogJobID { get; set; }
    }
    public class pd_MobileCodesGet_spResult
    {

        public int CodeID { get; set; }
        public string CodeDisplay { get; set; }

        public string CodeDisplaySort { get; set; }

    }


}
