using System;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseGetDefaults_spParams
    {
        public int  CaseID { get; set; }
        public int  UserID { get; set; }
        public int AgencyID { get; set; }

        public Guid  BatchLogJobID { get; set; }
    }
    public class pd_CaseGetDefaults_spResults
    {
        public byte? DOBRequiredForChildren { get; set; }
        public int? DefaultHearingOfficerPersonID { get; set; }
        public int? DefaultHearingCourtDepartmentCodeID { get; set; }
        public string DefaultHearingTime { get; set; }
        public string DefaultHearingTimeContested { get; set; }
        public byte? DataValidation_RequireJudgeFlag { get; set; }
        public byte? DisplayInCourtTimeDescriptionFlag { get; set; }
        public byte? DataValidation_RequireHearingHoursFlag { get; set; }
        public string HoursNotRequiredBeforeHearingDate { get; set; }
        
    }
}