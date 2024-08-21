using System;

namespace LALoDep.Models
{
    public class EditHearingAmountViewModel
    {
        public int HearingID { get; set; }
        public decimal? BaseRate { get; set; }
        public decimal? ModifiedRate { get; set; }
        public string HearingDate { get; set; }

    }
}