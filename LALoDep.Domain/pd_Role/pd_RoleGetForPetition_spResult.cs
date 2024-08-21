using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetForPetition_spResult
    {
		public int? Selected { get; set; }
		public int? UsePetitionRoleDateRange { get; set; }
		public string PetitionRoleStartDate { get; set; }
		public string PetitionRoleEndDate { get; set; }
		public string CaseRoleDateRange { get; set; }
		public int? PersonID { get; set; }
		public string PersonName { get; set; }
		public int? PersonAgencyID { get; set; }
		public int? RoleID { get; set; }
		public string Role { get; set; }
		public int? RoleTypeCodeID { get; set; }
		public int? PetitionRoleID { get; set; }
		public byte? RoleClient { get; set; }
		public string RoleStartDate { get; set; }
		public string RoleEndDate { get; set; }
		public int? AgencyAttorneyFlag { get; set; }
		public int? RespondentFlag { get; set; }
		public int? ChildFlag { get; set; }
		public int? OnCase { get; set; }
		public string PetitionRoleInsertedOn { get; set; }
		public string SortDate { get; set; }
	}
}
