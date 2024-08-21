using LALoDep.Domain.Advisement;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class AdvisementViewModel
    {
        public int CaseID { get; set; }
        public int AttorneyPersonID { get; set; }
        public IEnumerable<Advisement_GetByCaseID_spResult> Advisements { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }

    }
}