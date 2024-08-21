using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeAgencyGroupGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? UserID { get; set; }
        public int? AgencyCountyID { get; set; }
        

    }

    public class TitleIVeAgencyGroupGet_spResult
    {
        public int? AgencyCountyID { get; set; }
        public string County { get; set; }
        public string AgencyGroup { get; set; }
        public string AgencyGroupAbbreviation { get; set; }
        public string AgencyGroupAddressName { get; set; }
        public string AgencyGroupAddressStreet { get; set; }
        public string AgencyGroupAddressCSZ { get; set; }
        public string AgencyGroupAddressPhone { get; set; }
        public string AgencyGroupSCCInvoiceIdentifier { get; set; }
        public string AgencyGroupContractNumber { get; set; }
        public DateTime? TitleIVeStartDate { get; set; }
        public DateTime? TitleIVeEndDate { get; set; }
        public string StandardAgreementNbr { get; set; }
        public int? DirectlyContractWithCourt { get; set; }
        public string FederalIDNumber { get; set; }
        public string PersonWhoWillPrepareInvoice { get; set; }
        public string PrepareInvoicePersonTitle { get; set; }
        public string PrepareInvoicePersonPhone { get; set; }
        public string PrepareInvoicePersonEmail { get; set; }
        public int? NumberOfSocialWorkers { get; set; }
        public int? NumberOfInvestigators { get; set; }
        public int? NumberOfParalegals { get; set; }
        public int? NumberofAdminAssistants { get; set; }
        public int? UseWorkHoursForActivityLog { get; set; }
        public int? TitleIVeAgencyGroupID { get; set; }
        public int? IsPanelAgency { get; set; }
        public int? IsStreamliner { get; set; }
        public DateTime? FullReviewMonth1 { get; set; }
        public DateTime? FullReviewMonth2 { get; set; }
    }
}
