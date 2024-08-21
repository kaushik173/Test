using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Petition
{
    public class pd_PetitionGet_spParams
    {   public int UserID { get; set; }
        public int PetitionID { get; set; }   
        public Guid BatchLogJobID { get; set; }
   
    }
    public class pd_PetitionGet_spResult
    {
        public int PetitionID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public System.DateTime PetitionFileDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int PetitionTypeCodeID { get; set; }
        public System.DateTime? PetitionCloseDate { get; set; }
        public short RecordStateID { get; set; }
 
   
        public string PetitionTypeCodeValue { get; set; }
        public string AgencyName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? Client { get; set; }
        public System.DateTime? CloseDate { get; set; }

    }

    public class pd_RoleGetAllByPetitionID_spParams
    {
        public int UserID { get; set; }
        public int PetitionID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_RoleGetAllByPetitionID_spResult
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
    }
} 