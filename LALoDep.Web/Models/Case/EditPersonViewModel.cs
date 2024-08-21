using LALoDep.Domain.pd_Person;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class EditPersonViewModel
    {
        public int PersonID { get; set; }

        [Display(Name = "Client")]
        public bool IsClient { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "DOB")]
        public DateTime? PersonDOB { get; set; }
        [Display(Name = "Race")]
        public int? PersonRaceCodeID { get; set; }
        public IEnumerable<SelectListItem> PersonRaceList { get; set; }

        [Display(Name = "Gender")]
        public int? PersonSexCodeID { get; set; }
        public IEnumerable<SelectListItem> PersonSexList { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? RoleStartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime? RoleEndDate { get; set; }

        [Display(Name = "Deceased")]
        public bool IsDeceased { get; set; }

        public int? DeceasedDate_PersonClassID { get; set; }

        [Display(Name = "Deceased Date")]
        public DateTime? DeceasedDate { get; set; }
        [Display(Name = "Language")]
        public int? PersonLanguageCodeID { get; set; }
        public IEnumerable<SelectListItem> PersonLanguageList { get; set; }

        [Display(Name = "Designated Day")]
        public int? DesignatedDayCodeID { get; set; }
        public IEnumerable<SelectListItem> DesignatedDayList { get; set; }

        [Display(Name = "Voc #")]
        public string VOC { get; set; }

        [Display(Name = "Voc Status")]
        public int? VOCStatusCodeID { get; set; }
        public IEnumerable<SelectListItem> VOCStatusList { get; set; }
        public int? PersonNameID { get; set; }
        public int? RoleID { get; set; }
        public IEnumerable<SelectListItem> PersonClassificationList { get; set; }
        public IEnumerable<SelectListItem> ClassificationEndReasonList { get; set; }
        public IEnumerable<SelectListItem> MedicationList { get; set; }
        public IEnumerable<SelectListItem> MedicationFrequencyList { get; set; }
        public List<PersonClassificationViewModel> PersonClassifications { get; set; }

        public List<PersonClassificationViewModel> PersonClassificationsClientStatus { get; set; }

        public List<PersionMedicationViewModel> PersionMedications { get; set; }
        public IEnumerable<SelectListItem> RoleTypeList { get; set; }
        public IEnumerable<SelectListItem> CaseStatusList { get; set; }
        public IEnumerable<PersonRace_GetList_spResult> PersonRaceGetList { get; set; }
        public IEnumerable<PersonRaceIUD_spParams> PersonRaceIUDList { get; set; }

        public short RecordStateID { get; set; }
        public int AgencyID { get; set; }
        public string PersonRaceVerbalValue { get; set; }

        public int? PersonNameTypeCodeID { get; set; }
        public string PersonNameMiddle { get; set; }
        public short? PersonNameRecordStateID { get; set; }

        public int? RoleTypeCodeID { get; set; }

        public int? DesignatedDayCaseAttrID { get; set; }
        public int? VOCCaseAttrID { get; set; }
        public int? VOCStatusCaseAttrID { get; set; }

        public int ButtonId { get; set; }

        public bool UpdateDeceased { get; set; }
        public bool UpdatePerson { get; set; }
        public bool UpdatePersonName { get; set; }
        public bool UpdateRole { get; set; }
        public int DOBRequiredForChildren { get; set; }
        public EditPersonViewModel()
        {
            CaseStatusList = new List<SelectListItem>();
            PersonRaceGetList = new List<PersonRace_GetList_spResult>();
            PersonRaceIUDList = new List<PersonRaceIUD_spParams>();
        }

    }

    public class PersonClassificationViewModel
    {
        public int? PersonClassificationID { get; set; }
        public int? PersonClassificationCodeID { get; set; }
        public string PersonClassificationStartDate { get; set; }
        public string PersonClassificationEndDate { get; set; }
        public int? PersonClassificationEndReasonCodeID { get; set; }
        public short? RecordStateID { get; set; }
        public bool DoDelete { get; set; }
        public int  CanEditFlag { get; set; }
    }

    public class PersionMedicationViewModel
    {
        public int? MedicationID { get; set; }
        public int? MedicationCodeID { get; set; }
        public decimal? MedicationDosage { get; set; }
        public int? MedicationFrequencyCodeID { get; set; }
        public string MedicationStartDate { get; set; }
        public string MedicationEndDate { get; set; }
        public byte? RecordStateID { get; set; }
        public bool DoDelete { get; set; }
    }
}