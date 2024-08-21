using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class CSECSummaryViewModel
    {

        public int AgencyID { get; set; }
        public int StaffTypeID { get; set; }
        public string CompletedStartDate { get; set; }
        public string CompletedEndDate { get; set; }
        public IEnumerable<SelectListItem> AgencyList
        {
            get; set;
        }
        public IEnumerable<SelectListItem> StaffTypeList
        {
            get; set;
        }
    }
    public class MyCSECQueueViewModel
    {
        public string PageTitle { get; set; }
        public string EncryptedPersonID { get; set; }
        public int PersonID { get; set; }
        public bool IncludeCompleted { get; set; }
        public bool ShowCompletes { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DateRangeType { get; set; }
        public string IncompleteList { get; set; }
        public string RestoreList { get; set; }
        public IEnumerable<SelectListItem> DateRangeTypeList
        {
            get; set;
        }

    }



    public class MyARQueueViewModel
    {
        public string PersonName { get; set; }
        public int PersonID { get; set; }
        [Display(Name = "Date Range Type")]
        public string DateRangeType { get; set; }
        [Display(Name = "Include Completed AR's")]
        public int IncludeCompletedFlag { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }

        public IEnumerable<KeyValuePair<string, string>> DateRanges
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Due", "Due"),
                    //new KeyValuePair<string, string>("Reminder", "Reminder"),
                    new KeyValuePair<string, string>("Completed", "Completed"),
                    new KeyValuePair<string, string>("Hearing", "Hearing"),
                    new KeyValuePair<string, string>("Request", "Request"),
                };
            }
        }
        public IEnumerable<KeyValuePair<string, string>> IncludeCompleted
        {
            get
            {
                return new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("0", "No"),
                    new KeyValuePair<string, string>("1", "Yes"),
                };
            }
        }
    }

    public class TrainingUploadViewModel
    {
        public string ErrorMessage { get; set; }
        public string FileName { get; set; }

        public IEnumerable<SelectListItem> VenueList { get; set; }
        public IEnumerable<SelectListItem> CreditTypeList { get; set; }
        public IEnumerable<SelectListItem> PersonList { get; set; }
        public List<TrainingUploadFileModel> TrainingUploadFileModelList { get; set; }
        public List<TrainingUploadFileModel> TrainingUploadFileModelForAddModeList { get; set; }

        public TrainingUploadViewModel()
        {
            TrainingUploadFileModelList = new List<TrainingUploadFileModel>();

            TrainingUploadFileModelForAddModeList = new List<TrainingUploadFileModel>();
        }


    }

    public class TrainingUploadFileModel
    {

        public int? JcatsPersonID { get; set; }
        public string CourseTitle { get; set; }
        public string Provider { get; set; }
        public string SubjectMatter { get; set; }
        public int? JcatsCreditTypeCodeID { get; set; }

        public bool Participatory { get; set; }
        public decimal? Hours { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? JcatsVenueCodeID { get; set; }

    }
    public class MyInvoiceQueueViewModel
    {
        public bool ResetPageCache { get; set; }
        public string PageTitle { get; set; }
        public string EncryptedPersonID { get; set; }
        public int? PersonID { get; set; }
        public int? YearQuarterID { get; set; }
        public int? InvoiceStatusCodeID { get; set; }
        public string InvoiceNumber { get; set; }
        public string SAPNumber { get; set; }

        public int NotInvoicedFlag { get; set; }

        public IEnumerable<SelectListItem> InvoiceStatusList
        {
            get; set;
        }
        public IEnumerable<SelectListItem> YearQuarterList
        {
            get; set;
        }
    }
}