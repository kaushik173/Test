using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;
using System.Web.Mvc;
using LALoDep.Domain.pd_Person;

namespace LALoDep.Models.Case
{
    public class ContactInformationViewModel
    {
        public int PersonID { get; set; }
        public List<pd_PersonContactGetByCaseID_spResults> ContactInfoList { get; set; }
        public IEnumerable<PersonViewModel> PersonList { get; set; }
        public List<pd_PersonContactUpdate_spParams> ContactInfoAddList { get; set; }

        public IEnumerable<SelectListItem> ContactTypeList { get; set; }
        public bool CanAddAccess { get; set; }
        public bool CanEditAccess { get; set; }
        
        public ContactInformationViewModel()
        {
            ContactInfoAddList = new List<pd_PersonContactUpdate_spParams>();
            ContactInfoList = new List<pd_PersonContactGetByCaseID_spResults>();
        }
    }
}