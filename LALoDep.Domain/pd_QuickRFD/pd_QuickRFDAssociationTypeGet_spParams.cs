using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_QuickRFD
{
    public class pd_QuickRFDAssociationTypeGet_spParams
    {
        public int CaseID { get; set; }
        public int ReportFilingDueID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_QuickRFDAssociationTypeGet_spResult
    {
        public int AssociationTypeCodeID { get; set; }
        public string AssociationTypeDisplay { get; set; }
    }
    public class pd_QuickRFDRoleInsert_spParams
    {
        public int CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int RFDID { get; set; }
        public int? AssociationTypeCodeID { get; set; }
        public int CaretakerFlag { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public string EmailAddress { get; set; }
        public string WorkPhone { get; set; }
        public string CelPhone { get; set; }
        public int AddressID { get; set; }
        public string Number_602ClientRoleIDList { get; set; }
        public string Non602ClientRoleIDList { get; set; }
        public int RecordStateID { get; set; }
        public int UserID { get; set; }
        public DateTime? AddrssStartDate { get; set; }
    }



}
