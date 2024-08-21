using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Petition
{
    public class pd_PetitionGetByCaseID_spParams
    {

        public int UserID { get; set; }
        public int CaseID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class pd_PetitionGetByCaseID_spResult
    {

        public int PetitionID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public System.DateTime? PetitionFileDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? PetitionTypeCodeID { get; set; }
        public System.DateTime? PetitionCloseDate { get; set; }

        public string PetitionTypeCodeValue { get; set; }
        public string AgencyName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public byte? Client { get; set; }
        public string CloseDate { get; set; }
        public string CloseReason { get; set; }
        public int? ChildCount { get; set; }
        public int? RoleID { get; set; }
        public string Attorney { get; set; }
        public int? HA_AppearanceRequiredFlag { get; set; }
    }
    public class pd_PetitionGetAllByHearingID_spParams
    {

        public int UserID { get; set; }
        public int? HearingID { get; set; }
        public int? CaseID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class pd_PetitionGetAllByHearingID_spResult
    {
        public int? Selected { get; set; }
        public int? PetitionResultID { get; set; }
        public int? PetitionID { get; set; }
        public int? HearingPetitionKey { get; set; }
        public System.DateTime? PetitionFileDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string Type { get; set; }
        public string Child { get; set; }
        public int? PersonID { get; set; }
        public byte? RoleClient { get; set; }
        public System.DateTime? PetitionCloseDate { get; set; }
        public System.DateTime? HearingDateTime { get; set; }
        public int? PetitionTypeCodeID { get; set; }
        public int? OrderBackFlag { get; set; }
        public int? ASFAFlag { get; set; }
        public int? OrderedToAppear { get; set; }

        public int? HA_ID { get; set; }
        public int? HA_RoleID { get; set; }
        public int? HA_AttendedFlag { get; set; }
        public int? HA_CounselPersonID { get; set; }
        public int? HA_FillinCounselPersonID { get; set; }
        public string HA_Placement { get; set; }
        public int? HA_AppearanceRequiredFlag { get; set; }

    }
    public class pd_RoleGetForPetitionCopy_spParams
    {

        public int FromPetitionID { get; set; }
        public int UserID { get; set; }
        public int CaseID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_RoleGetForPetitionCopy_spResult
    {
        public string Suffix { get; set; }
        public string DefaultPetitonNumber { get; set; }
        public string NameDisplay { get; set; }
        public int? RoleID { get; set; }
        public byte? RoleClient { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }


    }
    public class pd_PetitionRoleGetForPetitionCopy_spParams
    {

        public int PetitionID { get; set; }
        public int UserID { get; set; }

        public Guid BatchLogJobID { get; set; }

    }
    public class pd_PetitionRoleGetForPetitionCopy_spResult
    {

        public int? Selected { get; set; }
        public int? PersonID { get; set; }
        public string PersonName { get; set; }
        public int? PersonAgencyID { get; set; }
        public int? RoleID { get; set; }
        public string Role { get; set; }
        public int? RoleTypeID { get; set; }
        public int? PetitionRoleID { get; set; }
        public byte? RoleClient { get; set; }
        public int? Sort { get; set; }
        public string RoleStartDate { get; set; }
        public string RoleEndDate { get; set; }
        public DateTime? PetitionRoleStartDate { get; set; }
    }
}
