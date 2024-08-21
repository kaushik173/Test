using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Association
{
  public  class pd_AssociationGetByPersonIDCaseID_spParams
    {
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public int UserID        {get;set;}
        public Guid BatchLogJobID {get;set;}
    
    }
  public class pd_AssociationGetByPersonIDCaseID_spResult
  {
      public int? AssociationID { get; set; }
      public int?  AgencyID { get; set; }
      public int? CaseID { get; set; }
      public int? PersonID { get; set; }
      public int? RelatedPersonID { get; set; }
      public int? AssociationCodeID { get; set; }
      public DateTime? AssociationStartDate { get; set; }
      public DateTime? AssociationEndDate { get; set; }
      public string Association { get; set; }
      public string PersonLastName { get; set; }
      public string PersonFirstName { get; set; }
      public string RelatedPersonLastName { get; set; }
      public string RelatedPersonFirstName { get; set; }
      public string Role { get; set; }
      public string PhoneNumber { get; set; }
  }
}
