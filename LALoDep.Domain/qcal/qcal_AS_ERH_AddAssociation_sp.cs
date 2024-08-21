
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.qcal
{

    public class qcal_AS_ERH_AddAssociation_spParams
    {
        public int? CaseAgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? ChildPersonID { get; set; }
        public int? ERHPersonID { get; set; }
        public int? AssociationCodeID { get; set; }
        public DateTime? StartDate { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_ERH_UpdateAssociation_spParams
    {
        public int? AssociationID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
}