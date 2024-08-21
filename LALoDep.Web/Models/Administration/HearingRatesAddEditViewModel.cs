using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_HearingRates;

namespace LALoDep.Models.Administration
{
    public class HearingRatesAddEditViewModel
    {
        public int? HearingRateID { get; set; }
        public decimal? HearingRate { get; set; }
        public int HearingTypeID { get; set; }
        public int AgencyID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime? DateStartDate { get; set; }    
        public DateTime? DateEndDate { get; set; }
        public bool Deleted { get; set; }
        public bool OnViewLoad {get; set; }
        public int ButtonID { get; set; }
        public List<pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult> HearingRatesList { get; set; }
        public  HearingRatesAddEditViewModel()
        {
            HearingRatesList = new List<pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult>();
        }
    }   
}