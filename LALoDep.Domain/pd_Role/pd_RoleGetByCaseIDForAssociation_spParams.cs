using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetByCaseIDForAssociation_spParams
    {
        public int CaseID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetByCaseIDForAssociation_spResult
    {
        public int CaseID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public int PersonID { get; set; }
        public int AgencyID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public System.DateTime? PersonDOB { get; set; }
        public string Role { get; set; }
        public byte RoleClient { get; set; }
        public int SystemValueSequence { get; set; }
    }
    public class pd_RoleGetByCaseIDForAssociationRelatedTo_spParams
    {
        public int CaseID { get; set; }
        public int  CaseAgencyID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetByCaseIDForAssociationRelatedTo_spResults
    {
        public int? RoleID { get; set; }
        public int CaseID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public int PersonID { get; set; }
        public int AgencyID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public System.DateTime? Persondob { get; set; }
        public string Role { get; set; }
        public byte RoleClient { get; set; }
        public System.DateTime? RoleStartDate { get; set; }
        public System.DateTime? RoleEndDate { get; set; }
        public int AgencyCodeDisplayOrder { get; set; }
    }
    public class pd_RoleGetLegalSpecific_spParams
    {
        public int CaseID { get; set; }
        public int? AgencyID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetLegalSpecific_spResult
    {
        public string FullName { get; set; }
        public int? PersonID { get; set; }
        public string LegalType { get; set; }
        public int? LegalTypeID { get; set; }
        public int? RealLegalTypeID { get; set; }
        public int? SystemValueSequence { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? OtherAgencyAttorneyFlag { get; set; }
    }

    public class pd_RoleGetLegalStatus_spParams
    {
        public int CaseID { get; set; }
          public int CaseAgencyID { get; set; }
      
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RoleGetLegalStatus_spResult
    {
        public string LegalType { get; set; }
        public int LegalTypeID { get; set; }
        public int Multiple { get; set; }
        public int Staff { get; set; }
        public int OnCase { get; set; }
        public int SystemValueSequence { get; set; }
    }

}
