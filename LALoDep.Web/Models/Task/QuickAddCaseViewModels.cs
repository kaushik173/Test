using LALoDep.Domain.Advisement;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.qcal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class FillinByAttorneyBatchViewModel
    {
        [Display(Name = "Agency")]
        public int? AgencyID { get; set; }

        [Display(Name = "Assigned Attorney")]
        public int? AssignedAttorneyID { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }
        [Display(Name = "Will Fillin Cover All Hearings During Date Range?")]
        public bool FillinCoverYesNo { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }


        public IEnumerable<SelectListItem> AssignedAttorneyList { get; set; }


        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public IEnumerable<SelectListItem> Agencies { get; set; }
        [Display(Name = "Add Fill-In Attorney")]
        public int? AddFillInAttorneyID { get; set; }
        [Display(Name = "Attorney")]
        public int? AddAttorneyID { get; set; }
        [Display(Name = "Dept")]

        public int? AddDepartmentID { get; set; }

        public string FillinCoverCheckedData { get; set; }



    }

    public class CalendarNumberingUpdate
    {

        public string CalNbr { get; set; }
        public int HearingID { get; set; }
    }
    public class CalendarNumberingViewModel
    {
        public List<CalendarNumberingUpdate> CalNbrs { get; set; }
        public string Date { get; set; }
        public int? AgencyID { get; set; }
        public List<SelectListItem> AgencyList { get; set; }
        public int DepartmentID { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
        public string SortOption { get; set; }


        public CalendarNumberingViewModel()
        {
            AgencyList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
            CalNbrs = new List<CalendarNumberingUpdate>();
        }

    }
    public class QuickCalMyCalendarViewModel
    {

        public string HearingDate { get; set; }
        public int PendingHearingsOnly { get; set; }
        public List<qcal_MyCalendar_spResult> HearingList { get; set; }
        public string ShowAll { get; set; }
        public List<SelectListItem> AttorneyList { get; set; }
        public int AttorneyPersonID { get; set; }
        public List<Advisement_GetForAttorney_spResult> AdvisementList { get; set; }

        public QuickCalMyCalendarViewModel()
        {
            AttorneyList = new List<SelectListItem>();
               HearingList = new List<qcal_MyCalendar_spResult>();
            AdvisementList = new List<Advisement_GetForAttorney_spResult>();
        }

    }
    [Serializable]
    public class QuickAddCaseSearchViewModel
    {
        [Display(Name = "Agency")]
        public int? AgencyID { get; set; }
        [Display(Name = "Case # (Child 1)")]
        public string CaseNumber1 { get; set; }
        [Display(Name = "Case # (Child 2)")]
        public string CaseNumber2 { get; set; }
        [Display(Name = "Case # (Child 3)")]
        public string CaseNumber3 { get; set; }

        public string ChildFirstName1 { get; set; }
        public string ChildLastName1 { get; set; }
        public string Child1DOB { get; set; }


        public string ChildFirstName2 { get; set; }
        public string ChildLastName2 { get; set; }
        public string Child2DOB { get; set; }


        public string ChildFirstName3 { get; set; }
        public string ChildLastName3{ get; set; }
        public string Child3DOB { get; set; }




        public string FatherFirstName1 { get; set; }
        public string FatherLastName1 { get; set; }
        public string Father1DOB { get; set; }

        public string FatherFirstName2 { get; set; }
        public string FatherLastName2 { get; set; }
        public string Father2DOB { get; set; }

        [Display(Name = "Mother Last Name(Exact)")]
        public string MotherLastName { get; set; }
        [Display(Name = "Mother First Name")]
        public string MotherFirstName { get; set; }

        [Display(Name = "Mother DOB")]
        public string MotherDOB { get; set; }

        public IEnumerable<SelectListItem> AgencyList { get; set; }


        [Display(Name = "For Attorney")]
        public string AttorneyID { get; set; }
        public IEnumerable<qcal_AttorneyListForQuickCaseAdd_spResult> AttorneyList { get; set; }
         

    }

    public class QuickAddCaseViewModel
    {
        public List<QuickAddCasePerson> CasePersonList { get; set; }
        public List<QuickAddCasePerson> ChildList { get; set; }
        public List<QuickAddCasePerson> ParentList { get; set; }
        [Display(Name = "Appt Date")]
        public string AppointmentDate { get; set; }
        [Display(Name = "Panel")]
        public int AgencyID { get; set; }
        public bool IsPanel { get; set; }

        [Display(Name = "Case Judge")]
        public int? CaseOfficerPersonID { get; set; }

        [Display(Name = "Case Dept")]
        public int? CaseDepartmentID { get; set; }
        public IEnumerable<SelectListItem> HearingDepartmentList { get; set; }

        [Display(Name = "Case Referral Source")]
        public int? CaseRefrelSourceID { get; set; }
        public IEnumerable<SelectListItem> CaseRefrelSourceList { get; set; }

        [Display(Name = "Attorney")]
        public int? AttorneyPersonID { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }

        [Display(Name = "Designated Day")]
        public int? DesignatedDayCodeID { get; set; }
        public IEnumerable<SelectListItem> DesignatedDayList { get; set; }

        [Display(Name = "Ptn File Dt")]
        public string PetitionFileDate { get; set; }
        [Display(Name = "Ptn Type")]
        public int? PetitionTypeCodeID { get; set; }
        public IEnumerable<SelectListItem> PetitionTypeList { get; set; }
        [Display(Name = "Allegation 1")]
        public int? Allegation1ID { get; set; }
        [Display(Name = "Allegation 2")]
        public int? Allegation2ID { get; set; }
        [Display(Name = "Allegation 3")]
        public int? Allegation3ID { get; set; }
        [Display(Name = "Allegation 4")]
        public int? Allegation4ID { get; set; }
        [Display(Name = "Allegation 5")]
        public int? Allegation5ID { get; set; }
        [Display(Name = "Allegation 6")]
        public int? Allegation6ID { get; set; }
        public IEnumerable<SelectListItem> AllegationList { get; set; }

        [Display(Name = "Physical File Name")]
        public string PhysicalFileName { get; set; }

        [Display(Name = "Hearing Type")]
        public int? HearingTypeID { get; set; }

        [Display(Name = "Hearing Type")]
        public int? OtherHearingTypeID { get; set; }
        public IEnumerable<SelectListItem> HearingTypeList { get; set; }
        public IEnumerable<SelectListItem> HearingTypeList2 { get; set; }

        [Display(Name = "Hearing Date")]
        public string HearingDate { get; set; }

        [Display(Name = "Hearing Date")]
        public string OtherHearingDate { get; set; }

        [Display(Name = "Hearing Time")]
        public string HearingTime { get; set; }

        [Display(Name = "Hearing Time")]
        public string OtherHearingTime { get; set; }

        [Display(Name = "Hearing Dept")]
        public int? HearingDepartmentID { get; set; }

        [Display(Name = "Hearing Dept")]
        public int? OtherHearingDepartmentID { get; set; }

        [Display(Name = "Hearing Officer")]
        public int? HearingOfficerID { get; set; }

        [Display(Name = "Hearing Officer")]
        public int? OtherHearingOfficerID { get; set; }

        public IEnumerable<SelectListItem> HearingOfficerList { get; set; }

        [Display(Name = "Appearing Attorney")]
        public int? AppearingAttorneyID { get; set; }
        [Display(Name = "Appearing Attorney")]
        public int? OtherAppearingAttorneyID { get; set; }
        [Display(Name = "Note Type")]
        public int? NoteTypeID { get; set; }
        public IEnumerable<SelectListItem> NoteTypeList { get; set; }

        [Display(Name = "Subject")]
        public string NoteSubject { get; set; }
        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Placement Address")]
        public int? PlacementAddressID { get; set; }
        public IEnumerable<SelectListItem> PlacementAddressList { get; set; }
        [Display(Name = "OR Address Street")]
        public string Street { get; set; }
        public string City { get; set; }
        [Display(Name = "State")]
        public int? StateID { get; set; }
        public IEnumerable<CodeViewModel> StateList { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Address Phone")]
        public string AddressPhone { get; set; }

        public IEnumerable<SelectListItem> RoleParentList { get; set; }
        public IEnumerable<SelectListItem> RoleChildList { get; set; }
        public IEnumerable<SelectListItem> SexList { get; set; }
        public IEnumerable<SelectListItem> ChildrenAssociationTypeList { get; set; }
        public int AttorneyRoleTypeID { get; set; }
        public int DOBRequiredForChildren { get; set; }
        public QuickAddCaseViewModel()
        {
            CasePersonList = new List<QuickAddCasePerson>();
            ChildList = new List<QuickAddCasePerson>();
            ParentList = new List<QuickAddCasePerson>();
        }
    }

    public class QuickAddCasePerson
    {
        public string CheckBoxLabel { get; set; }
        public bool IsParent { get; set; }
        public bool IsClient { get; set; }
        public string CaseNumber { get; set; }
        public int? RoleID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DOB { get; set; }

        public int? SexTypeCodeID { get; set; }
        public int? ChildrenAssociationTypeCodeID { get; set; }

        public bool IsSS { get; set; }
        public bool HasAddress { get; set; }
        public bool IsDefaultChecked { get; set; }

      
    }

    public class HearingCaseFile
    {
        public int HearingID { get; set; }
        public int HearingCaseFileID { get; set; }
        public int CaseFileID { get; set; }
        public bool Selected { get; set; }

    }
}