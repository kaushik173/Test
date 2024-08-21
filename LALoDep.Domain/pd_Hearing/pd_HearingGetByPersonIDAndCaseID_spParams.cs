using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
   public class pd_HearingGetByPersonIDAndCaseID_spParams
    {
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
   public class pd_HearingGetByPersonIDAndCaseID_spResult
   {
       public int? HearingID { get; set; }
       public int? CaseID { get; set; }
       public int? HearingTypeCodeID { get; set; }
       public DateTime? HearingDateTime { get; set; }
       public int? HearingOfficerPersonID { get; set; }
       public int? HearingResultCodeID { get; set; }
       public string Type { get; set; }
       public string Result { get; set; }
       public string PersonNameLast { get; set; }
       public string PersonNameFirst { get; set; }
       public int? HearingCourtDepartmentCodeID { get; set; }
       public int? HearingRequestedByCodeID { get; set; }
       public string CourtDepartment { get; set; }
       public string RequestedBy { get; set; }
   }
}
