using LALoDep.Domain.pd_Hearing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Task
{
    public class ARTransferViewModel
    {
        public string PersonName { get; set; }
        public int PersonID { get; set; }
        [Display(Name = "Date Range Type")]
        public string DateRangeType{ get; set; }
        
        [Display (Name="Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Transfer To")]
        public int? TransferToPersonID { get; set; }
        public IEnumerable<KeyValuePair<string, string>> DateRanges
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("Due", "Due"),
                    new KeyValuePair<string, string>("Reminder", "Reminder"),
                };
            }
        }

        public List<PersonViewModel> TransferTo { get; set; }
        
        public ARTransferViewModel()
        {
            TransferTo = new List<PersonViewModel>();            
        }
    }

    public class ARTransferModel
    {
        public int? TransferToPersonID { get; set; }
        public List<pd_HearingReportFilingDueGetByDateRangeTypePersonID_spResult> ARsToTransfer { get; set; }

        public ARTransferModel()
        {
            ARsToTransfer = new List<pd_HearingReportFilingDueGetByDateRangeTypePersonID_spResult>();
        }
    }
}