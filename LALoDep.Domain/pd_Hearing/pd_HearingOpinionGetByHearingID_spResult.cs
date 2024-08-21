using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingOpinionGetByHearingID_spResult
    {
        public int? PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? RoleID { get; set; }
        public int? DisplayOrder { get; set; }
        public string RoleType { get; set; }
        public Int16? RoleClient { get; set; }
        public int? HearingOpinionID { get; set; }
        public string Opinion { get; set; }
        public int? NoteID { get; set; }
    }
}
