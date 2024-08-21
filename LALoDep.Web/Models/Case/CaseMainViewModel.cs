using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;

namespace LALoDep.Models.Case
{
    public class CaseMainViewModel
    {
        public IEnumerable<CaseFileGetByCaseID_spResult> CaseFiles { get; set; }
        public pd_CaseGet_sp_Result CaseGet { get; set; }
        public IEnumerable<pd_RoleGetByCaseID_spResult> CaseRole { get; set; }
        public IEnumerable<pd_PetitionAndAllegationGetByCaseID_spResult> PetitionAndAllegation { get; set; }
        public IEnumerable<pd_CaseGetOtherByPersonIDCaseID_sp_Result> CaseGetOtherByPerson { get; set; }
        public IEnumerable<pd_HearingGetByCaseID_spResult> Hearing { get; set; }

        public IEnumerable<pd_HearingPersonsGetByCaseID_spResult> HearingPersons { get; set; }
        public List<pd_RelatedCasesGetList_spResult> RelatedCasesGetList { get; set; }

        public CaseMainViewModel()
        {
            RelatedCasesGetList = new List<pd_RelatedCasesGetList_spResult>();

            CaseFiles = new List<CaseFileGetByCaseID_spResult>();
            CaseGet = new pd_CaseGet_sp_Result();
            CaseRole = new List<pd_RoleGetByCaseID_spResult>();
            PetitionAndAllegation = new List<pd_PetitionAndAllegationGetByCaseID_spResult>();
            CaseGetOtherByPerson = new List<pd_CaseGetOtherByPersonIDCaseID_sp_Result>();
            Hearing = new List<pd_HearingGetByCaseID_spResult>();
            HearingPersons = new List<pd_HearingPersonsGetByCaseID_spResult>();
        }
    }
}