using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingGetByCaseIDRFD_spParams
    {
        public int CaseID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }
    public class pd_HearingGetByCaseIDRFD_spResult
    {
        public string Type{ get; set; }
        public int HearingID { get; set; }

        public DateTime HearingDateTime { get; set; }

        public string ShortType { get; set; }

    }
    public class pd_HearingReportFilingDueGetRequestedBy_spParams
    {
        public int CaseID { get; set; }
        public int CaseAgencyID { get; set; }
        public int IncludePersonID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }
    public class pd_HearingReportFilingDueGetRequestedBy_spResult
    {
        public int PersonID { get; set; }

        public int Current { get; set; }

        public string PersonNameDisplay { get; set; }

    }
    public class pd_RFDRoleGetByReportFilingDueID_spParams
    {
        public int CaseID { get; set; }
        public int ReportFilingDueID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
    }
    public class pd_RFDRoleGetByReportFilingDueID_spResult
    {
       
        public int Selected { get; set; }
        public int? RoleID  { get; set; }
 
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string RoleTypeCodeValue { get; set; }
        public int Closed { get; set; }
        public string ClientDisplay { get; set; }
        public string ClientAddress { get; set; }




    }




    public class pd_RoleGetARRequestFor_spParams
    {
        public int CaseID { get; set; }
        public int CaseAgencyID { get; set; }
        public int IncludePersonID { get; set; }

        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }
        public int HearingReportFilingDueID { get; set; }


         
    }
    public class pd_RoleGetARRequestFor_spResult
    {
        public int PersonID { get; set; }
        public string DisplayName { get; set; }

        public int RoleTypeCodeID { get; set; }
        public string RoleTypeDisplay { get; set; }
    }
    public class sup_ARAssignmentConflicts_spParams
    {
        public int CaseID { get; set; }

        public int PersonID { get; set; }

        public int UserID { get; set; }
    }
    public class sup_ARAssignmentConflicts_spResult
    {
         
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? PersonID { get; set; }
        public int? DialogType { get; set; }
        public string ClientName { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string Comment { get; set; }
        
    }
}
