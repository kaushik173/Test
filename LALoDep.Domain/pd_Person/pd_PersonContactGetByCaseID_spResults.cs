using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonContactGetByCaseID_spResults
  {
      public int? PersonContactID { get; set; }
      public int? AgencyID { get; set; }
      public int? PersonID { get; set; }
      public int? PersonContactTypeCodeID { get; set; }
      public string PersonContactInfo { get; set; }
      public Int16? RecordStateID { get; set; }
      public string PersonContactTypeCodeValue { get; set; }
      public string PersonContactTypeCodeShortValue { get; set; }
      public string PersonNameLast { get; set; }
      public string PersonNameFirst { get; set; }
      public byte? RoleClient { get; set; }
      public int? Editable { get; set; }
      public string RoleTypeCodeValue { get; set; }
    }
  
}
