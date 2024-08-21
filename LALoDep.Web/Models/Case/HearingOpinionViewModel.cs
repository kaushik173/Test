using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Case
{
    public class HearingOpinionViewModel
    {
        public int HearingID { get; set; }
        public int? RoleID{ get; set; }
        public int? NoteID { get; set; }
        public int? HearingOpinionID{ get; set; }
        public string OpinionNote { get; set; }
    }
}