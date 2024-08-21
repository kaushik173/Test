using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LALoDep.Domain.Mobile;

namespace LALoDep.Areas.Mobile.Models
{
    public class CaseInfoMobileViewModel
    {
        public int  CaseID { get; set; }
        public string CaseNumber { get; set; }
         public string ClientName { get; set; }
          

      
         public List<MobileCaseInfoGetHearings_spResult> Hearings { get; set; }
         public List<MobileRoleInfoGet_spResult> Roles { get; set; }

        public CaseInfoMobileViewModel()
        {
            Hearings = new List<MobileCaseInfoGetHearings_spResult>();
            Roles = new List<MobileRoleInfoGet_spResult>();
        }
    }
}