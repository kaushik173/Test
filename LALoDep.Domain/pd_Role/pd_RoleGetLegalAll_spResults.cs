using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetLegalAll_spParams
    {
        public int CaseID { get; set; }
        public int CaseAgencyID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetLegalAll_spResults
    {
        public int IsAgencyAttorneyFlag { get; set; }
        public string LegalType { get; set; }
        public int? LegalTypeID { get; set; }
        public int? Staff { get; set; }
        public string FullName { get; set; }
        public int? PersonID { get; set; }
        public int? RoleID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public System.DateTime? RoleStartDate { get; set; }
        public System.DateTime? RoleEndDate { get; set; }
        public byte? RoleClient { get; set; }
        public short RecordStateID { get; set; }

    }
    public class pd_RoleGetByCaseIDForAssociationAttorney_spParams
    {
        public int CaseID { get; set; }
        public int CaseAgencyID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetByCaseIDForAssociationAttorney_spResults
    {
        public int PersonID { get; set; }
        public string PersonNameDisplay { get; set; }

    }
    public class pd_RoleGetLegalFreeFormGeneral_spParams
    {
     

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetLegalFreeFormGeneral_spResults
    {
        public string LegalType { get; set; }
        public int LegalTypeID { get; set; }

    }

}
