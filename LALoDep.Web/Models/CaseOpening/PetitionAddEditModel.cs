using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Aspose.Words.Lists;
using LALoDep.Domain.pd_Allegation;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.CaseOpening
{
    public class PetitionAddEditModel
    {

        public int PetitionID { get; set; }
        public int CaseAttributeID { get; set; }
    
        public int PetitionTypeID { get; set; }
        public IEnumerable<SelectListItem> PetitionTypeList { get; set; }
        public string FileDate { get; set; }
        public string CaseNumber { get; set; }
        public string CloseDate { get; set; }
        public string PhysicalFileName { get; set; }
        public int AttorneyAgencyRoleTypeCodeID { get; set; }
        public IEnumerable<pd_RoleGetForPetition_spResult> RoleList { get; set; }
        public bool IsDel { get; set; }

        public IEnumerable<SelectListItem> AllegationTypeList { get; set; }
        public IEnumerable<SelectListItem> AllegationFindingList { get; set; }
        public IEnumerable<pd_AllegationGetByPetitionID_spResult> AllegationList { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
        public string AttorneyID { get; set; }
        public int CaseClosedDate { get; set; }
        public IEnumerable<pd_CodeGetBySystemValueTypeID_spResults> RoleTypeList { get; set; }
        public PetitionAddEditModel()
        {
            AllegationList = new List<pd_AllegationGetByPetitionID_spResult>();
            AttorneyList = new List<SelectListItem>();
            RoleTypeList = new List<pd_CodeGetBySystemValueTypeID_spResults>();
        }
    }


    public class PetitionCopyModel
    {
        public int PetitionID { get; set; }
        public DateTime PetitionFileDate { get; set; }
      
        public pd_PetitionGet_spResult Petition { get; set; }
        public IEnumerable<pd_RoleGetForPetitionCopy_spResult> PetitionRoleList { get; set; }
        public int PetitionTypeCodeID { get; set; }
         
        public PetitionCopyModel()
        {
            Petition=new pd_PetitionGet_spResult();
            PetitionRoleList = new List<pd_RoleGetForPetitionCopy_spResult>();
        }

    }
}