using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_wt;
using System.Web.Mvc;
using Jcats.SD.Domain.COAC;

namespace LALoDep.Models.Case
{
    public class RecordTimeAddViewModel
    {
        public int NextCaseID { get; set; }
        public string NextCase { get; set; }
        public int? StaffOnPersonID { get; set; }
        public int? StaffNotOnPersonID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal? WorkHours { get; set; }
        public decimal? WorkMileage { get; set; }
        public int WorkDescriptionCodeID { get; set; }
        public int WorkPhaseCodeID { get; set; }
        public string WorkStartTime { get; set; }
        public string WorkEndTime { get; set; }
        public int UseWorkHoursForActivityLog { get; set; }
        public string NoteEntry { get; set; }
        public string NoteSubject { get; set; }
        public int ButtonID { get; set; }
        public string FromZipCode { get; set; }
        public string ToZipCode { get; set; }
        public List<wtRecordTimeGetCases_spResult> CaseList { get; set; }
        public IEnumerable<SelectListItem> Descriptions { get; set; }

        public IEnumerable<SelectListItem> Phases { get; set; }
        public List<pd_RoleGetByCaseIDClient_spResult> WorkForList { get; set; }
        public List<pd_RoleGetByCaseIDClient_spResult> DeleteWorkForList { get; set; }
        public List<pd_RoleGetByCaseIDBillingWorker_spResult> StaffOnCaseList { get; set; }
        public List<pd_RoleGetByCaseIDBillingWorker_spResult> StaffNotOnCaseList { get; set; }
        public string ControlType { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public IEnumerable<SelectListItem> IVeEligibleList { get; set; }
        public RecordTimeAddViewModel()
        {
            IVeEligibleList = new List<SelectListItem>();
            CaseList = new List<wtRecordTimeGetCases_spResult>();
            WorkForList = new List<pd_RoleGetByCaseIDClient_spResult>();
            DeleteWorkForList = new List<pd_RoleGetByCaseIDClient_spResult>();

            StaffOnCaseList = new List<pd_RoleGetByCaseIDBillingWorker_spResult>();
            StaffNotOnCaseList = new List<pd_RoleGetByCaseIDBillingWorker_spResult>();

        }
        public string QHEHearingID { get; set; }
        public int? HearingID { get; set; }

        public int? OtherDescriptionCodeID { get; set; }
        public int WorkHoursRequiredFlag { get; set; }
        public int WorkTimeVisibleFlag { get; set; }
        public int WorkPhaseRequiredFlag { get; set; }
        public int RecordTimeNoteSubjectFlag { get; set; }
        public string HoursLabel { get; set; }
    }
    public class CreateOtherAgencyCaseViewModel
    {
        public string ApptDate { get; set; }
        public int CaseID { get; set; }
        public List<coac_CaseRoles_spResult> CaseRolesList { get; set; }
        public List<coac_ConflictResults_spResult> ConflictResultsList { get; set; }
        public string AttorneyAndAgencyID { get; set; }
        public string SelectedPersonIDs { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }

        public CreateOtherAgencyCaseViewModel()
        {
            AttorneyList = new List<SelectListItem>();
            CaseRolesList = new List<coac_CaseRoles_spResult>();
            ConflictResultsList = new List<coac_ConflictResults_spResult>();


        }


    }
}
