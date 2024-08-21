using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Mobile
{
    public class MobileCaseInsert_spParams
    {   
        public int? AgencyID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public int? ClientRoleTypeCodeID { get; set; }
        public string CaseLastName { get; set; }
        public string CaseFirstName { get; set; }
        public DateTime? CaseAppointmentDate { get; set; }
        public string PetitionNumber { get; set; }
        public int? DepartmentCodeID { get; set; }
        public int? AssociationCodeID { get; set; }
        public DateTime? PetitionFileDate { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int AllegationTypeCodeID1 { get; set; }
        public int AllegationTypeCodeID2 { get; set; }
        public int AllegationTypeCodeID3 { get; set; }
        public int AllegationTypeCodeID4 { get; set; }
    }
}
