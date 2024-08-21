using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonInsert_spParams
    {
        public int PersonID { get; set; }
        public int AgencyID { get; set; }
        public DateTime? PersonDOB { get; set; }
        public int PersonRaceCodeID { get; set; }
        public int PersonSexCodeID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? PersonLanguageCodeID { get; set; }


    }
    public class pd_PersonNameInsert_spParams
    {
        public int PersonNameID { get; set; }
        public int? AgencyID { get; set; }
        public int PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameMiddle { get; set; }
        public int PersonNameTypeCodeID { get; set; }
        public string PersonNameSoundex { get; set; }


        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int PersonNameSalutationCodeID { get; set; }
        public int PersonNameSuffixCodeID { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }










    }
    public class pd_JcatsUserGetByJcatsUserLoginName_spResult
    {


        public string JcatsUserLoginName { get; set; }
        public int  JcatsUserID { get; set; }
        
    }
    public class pd_JcatsUserGetByJcatsUserLoginName_spParams
    {
        
   
        public string JcatsUserLoginName { get; set; }
        public int? ExcludeCurrentJcatsUserID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_JcatsUserInsert_spParams
    {
        public int JcatsUserID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public int JcatsGroupID { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsUserPassword { get; set; }
        public string JcatsUserLoginKey { get; set; }
        public string JcatsUserEMail { get; set; }
        public DateTime JcatsUserStartDate { get; set; }
        public DateTime? JcatsUserEndDate { get; set; }
        public DateTime? JcatsUserAccessDateTime { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }



    }
    
    public class pd_JcatsUserUpdate_spParams
    {
       
        public int JcatsUserID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public int JcatsGroupID { get; set; }
        public string JcatsUserLoginName { get; set; }
        public string JcatsUserPassword { get; set; }
        public string JcatsUserLoginKey { get; set; }
        public string JcatsUserEMail { get; set; }
        public DateTime JcatsUserStartDate { get; set; }
        public DateTime? JcatsUserEndDate { get; set; }
        public DateTime? JcatsUserAccessDateTime { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }



    }
    public class pd_JcatsUserUpdateUserLevel_spParams
    {

        public int JcatsUserID { get; set; }
        public int?  JcatsUserLevelCodeID { get; set; }
      
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }



    }
    public class NG_pd_JcatsUserUpdateTimeOut_spParams
    {

        public int JcatsUserID { get; set; }
        public int? JcatsUserTimeOut { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; } 
    }
    
    public class pd_StaffInfoInsert_spParams
    {
         

        public int StaffInfoID { get; set; }
        public int PersonID { get; set; }
        public int? EmailToPrimaryPersonContactID { get; set; }
        public int? EmailToSecondaryPersonContactID { get; set; }
        public int AlternateContactPersonID { get; set; }
        public int EmailToAlternatePersonContactFlag { get; set; }
        public string StaffInfoBarNumber { get; set; }
        public DateTime? StaffInfoBarAdmittedDate { get; set; }
        public DateTime? StaffInfoEligibilityEffectiveDate { get; set; }
        public DateTime? StaffInfoEligibilityEndingDate { get; set; }
        public string StaffInfoComment { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int StaffInfoStaffTeamCodeID { get; set; }
        public int StaffInfoEmployeeStatusCodeID { get; set; }
        public string StaffInfoEmployeeID { get; set; }
    }

}
