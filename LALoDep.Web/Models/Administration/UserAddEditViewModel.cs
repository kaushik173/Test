using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Users;
using LALoDep.Domain.pd_Users.Edit;
using LALoDep.Domain.TitleIVe;

namespace LALoDep.Models.Administration
{
    public class UserAddEditViewModel
    {
        public UserAddEditViewModel()
        {
            SupervisorList = new List<Supervisor_GetList_spResult>();
            TitleIVeStaffInfo = new TitleIVeStaffGet_spResult();
        }
        

        public pd_JCATSUserGet_spData JcatsUserData { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> AlternateContacts { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }
        public bool OnViewLoad { get; set; }
        public bool IsEmployee { get; set; }
        public StaffInfo StaffInfo { get; set; }
        public int? JcatsUserID { get; set; }
        public int PersonID { get; set; }
        public int HomeAgencyID { get; set; }
        public IEnumerable<SelectListItem> SalutationList { get; set; }
        public IEnumerable<SelectListItem> SuffixList { get; set; }
        public IEnumerable<SelectListItem> UserLevelList { get; set; }
        public IEnumerable<SelectListItem> EmployeeStatusList { get; set; }

        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<pd_AgencyGetSystemRoleByPersonID_spResult> SelectedAgencyList { get; set; }

        public int? SupervisorPersonID { get; set; }
        public List<Supervisor_GetList_spResult> SupervisorList { get; set; }

        public bool SupervisorChanged { get; set; }
        public TitleIVeStaffGet_spResult TitleIVeStaffInfo { get; set; }
      
        public IEnumerable<SelectListItem> OHCodeList { get; set; }

    }

    public class StaffInfo
    {
        public int StaffInfoID { get; set; }
        public string StaffInfoBarNumber { get; set; }
        public string StaffInfoComment { get; set; }
        public string EmailPrimary { get; set; }
        public string EmailSecondary { get; set; }
        public string Fax { get; set; }
        public int AlternateContactPersonID { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string StaffInfoBarAdmittedDate { get; set; }
        public string StaffInfoEligibilityEffectiveDate { get; set; }
        public string StaffInfoEligibilityEndingDate { get; set; }
        public int? EmailToPrimaryPersonContactID { get; set; }
        public int? EmailToSecondaryPersonContactID { get; set; }
        public bool EmailToAlternatePersonContactFlag { get; set; }
        public int? StaffInfoEmployeeStatusCodeID { get; set; }
        public int? FaxPersonContactID { get; set; }
        public int? MobilePersonContactID { get; set; }
        public int? WorkPersonContactID { get; set; }
        public string StaffInfoEmployeeID { get; set; }
        public int? EmailPrimaryPersonContactID { get; set; }
        public int? EmailSecondaryPersonContactID { get; set; }

        public bool UsePrimaryEmail { get; set; }
        public bool UseSecondaryEmail { get; set; }
    }

    public class pd_JCATSUserGet_spData
    {
        public int AgencyID { get; set; }
        public int JcatsUserID { get; set; }
        public int PersonID { get; set; }
        public int JcatsGroupID { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsUserStartDate { get; set; }
        public string JcatsUserEndDate { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? PersonNameSuffixCodeID { get; set; }
        public int? PersonNameSalutationCodeID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? JcatsUserLevelCodeID { get; set; }
        public string JcatsUserPassword { get; set; }
        public int? PersonNameID { get; set; }
        public int? JcatsUserTimeOut { get; set; }
    }
    public class UploadSignatureViewModel
    {
        public string UploadFileName { get; set; }
        
        public int JcatsUserID { get; set; }
        
        public int? SigFileUserID { get; set; }
        
        public string UploadFilePath { get; set; }
        public string UploadFromFilePath { get; set; }
        public bool SigFile { get; set; }
        public int? SigFileID { get; set; }
        public string SigFilePath { get; set; }
        public string InitFilePath { get; set; }
        public string NewSigFilePath { get; set; }
        public string UploadToFolderPath { get; set; }
    }
}