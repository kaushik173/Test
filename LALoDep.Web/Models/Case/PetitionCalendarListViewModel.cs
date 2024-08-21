using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Case
{
    public class PetitionCalendarListViewModel
    {
        public int? HearingID { get; set; }
        public string ItemType { get; set; }
        public string ItemDate{ get; set; }
        public string ItemTime { get; set; }
        public string HearingType { get; set; }
    }
}