using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using LALoDep.Domain.pd_CodeTables;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Profile;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.CaseOpening
{
    public class ActionRequestModel
    {


        public IEnumerable<pd_HearingReportFilingDueGetByCaseID_spResults> ActionRequestList { get; set; }

        public List<pd_ProfileGetList_spResult> ProfileList { get; set; }
    
        public ActionRequestModel()
        {
            ActionRequestList = new List<pd_HearingReportFilingDueGetByCaseID_spResults>();

            ProfileList = new List<pd_ProfileGetList_spResult>();


        }


        /*For Calendar Most Recent AR*/

        public bool PrintAR { get; set; }
        public bool PrintARProfile { get; set; }
        public int MostRecentARHearingRequestID { get; set; }
    }
    public class ActionRequestAddModel
    {

        public string RequestDate { get; set; }
        public int RequestTypeID { get; set; }
        public IEnumerable<pd_CodeGetByTypeIDAndUserID_spResult> RequestTypeList { get; set; }

        public int HearingID { get; set; }
        public IEnumerable<SelectListItem> HearingList { get; set; }
        public int RequestByID { get; set; }
        public IEnumerable<SelectListItem> RequestByList { get; set; }
        public int RequestForID { get; set; }
        public IEnumerable<SelectListItem> RequestForList { get; set; }
        public string DueDate { get; set; }
        public int LegalResearchTypeID { get; set; }
        public IEnumerable<SelectListItem> LegalResearchTypeList { get; set; }
        public IEnumerable<pd_CodeGetByTypeIDAndUserID_spResult> RequestList { get; set; }
        public DataTable ClientAddressList { get; set; }

        public string RequestNote { get; set; }
        public string RequestItemIds { get; set; }
        public string ClientAddressIds { get; set; }
        public string ControlType { get; set; }
        public ActionRequestAddModel()
        {

            RequestTypeList = new List<pd_CodeGetByTypeIDAndUserID_spResult>();
            RequestByList = new List<SelectListItem>();
            HearingList = new List<SelectListItem>();
            RequestForList = new List<SelectListItem>();
            LegalResearchTypeList = new List<SelectListItem>();
            RequestList = new List<pd_CodeGetByTypeIDAndUserID_spResult>();
            ClientAddressList = new DataTable();

        }
        public bool ForceCreateARAnyway { get; set; }

    }
    public class ActionRequestEditModel
    {
        public int HearingReportFilingDueID { get; set; }
        public string ControlType { get; set; }
        public string RequestDate { get; set; }
        public int RequestTypeID { get; set; }
        public string RequestType { get; set; }

        public int HearingID { get; set; }
        public int RequestByID { get; set; }

        public string RequestBy { get; set; }
        public string Hearing { get; set; }

        public int RequestForID { get; set; }
        public IEnumerable<SelectListItem> RequestForList { get; set; }
        public string DueDate { get; set; }
        public int LegalResearchTypeID { get; set; }
        public string LegalResearchType { get; set; }
        public bool Completed { get; set; }
        public IEnumerable<pd_RFDRoleContactGetByReportFilingDueID_spResult> ClientRoleList { get; set; }
        public string CompletedDate { get; set; }

        public string RequestNote { get; set; }
        public string InvestigatorEvaluationNote { get; set; }
        public int? InvestigatorEvaluationNoteID { get; set; }

        public string CaretakerEvaluationNote { get; set; }
        public int? CaretakerEvaluationNoteID { get; set; }

        public string SocialWorkerEvaluationNote { get; set; }
        public int? SocialWorkerEvaluationNoteID { get; set; }

        public string TherapistEvaluationNote { get; set; }
        public int? TherapistEvaluationNoteID { get; set; }
        public string ProbationOfficerEvaluationNote { get; set; }
        public int? ProbationOfficerEvaluationNoteID { get; set; }

        public int RoleCount { get; set; }

        public ActionRequestEditModel()
        {

            ClientRoleList = new List<pd_RFDRoleContactGetByReportFilingDueID_spResult>();

            RequestForList = new List<SelectListItem>();


        }
    }
    
}