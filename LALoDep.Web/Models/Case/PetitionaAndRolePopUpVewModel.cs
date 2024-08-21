using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;

namespace LALoDep.Models.Case
{
    public class PetitionaAndRolePopUpVewModel
    {
        public List<pd_PetitionGetClosedByRoleID_sp_Result> PetitionaDetailList { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }


        public PetitionaAndRolePopUpVewModel()
        {
            PetitionaDetailList = new List<pd_PetitionGetClosedByRoleID_sp_Result>();
        }
    }
}