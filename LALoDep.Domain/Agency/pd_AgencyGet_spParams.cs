using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Agency
{
    public class pd_AgencyGet_spParams
    {
        public int? AgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_AgencyGet_spResult
    {
        public int? AgencyID { get; set; }
        public int? AgencyGroupID { get; set; }
        public string AgencyName { get; set; }
        public string AgencyAbbreviation { get; set; }
        public string AgencyCaseNumberFormat { get; set; }
        public string AgencyMergePath { get; set; }
        public int? InsertedByUserID { get; set; }
        public DateTime? InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public DateTime? UpdatedOnDateTime { get; set; }
        public short? RecordStateID { get; set; }
        public string AgencyAddressName { get; set; }
        public string AgencyAddressStreet { get; set; }
        public string AgencyAddressCSZ { get; set; }
        public string AgencyAddressPhone { get; set; }
        public string AgencyAddressFax { get; set; }
        public string AgencyFederalTaxID { get; set; }
        public int? AgencyContactPersonID1 { get; set; }
        public int? AgencyContactPersonID2 { get; set; }
        public string DefaultHearingTime { get; set; }
        public string DefaultHearingTimeContested { get; set; }
        public byte? UniquePetitionNumbersFlag { get; set; }
        public byte? DataValidation_RequireJudgeFlag { get; set; }
        public byte? DisplayARAssignedFlag { get; set; }
        public string AttachFileMyDocumentsDirectory { get; set; }
        public byte? AttachFileDeleteFileAfterUploadFlag { get; set; }
        public byte? DataValidation_RequireHearingHoursFlag { get; set; }
        public byte? DisplayInCourtTimeDescriptionFlag { get; set; }
        public byte? DataValidation_RequireReferralSourceFlag { get; set; }
        public int? CopyCase_HearingResultCodeID { get; set; }

    }
}
