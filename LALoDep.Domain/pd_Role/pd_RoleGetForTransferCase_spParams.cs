using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetForTransferCase_spParams
    {
        public int RoleTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_RoleGetForTransferCase_spParams_new
    {
        public int RoleTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string Mode { get; set; }
    }

    public class pd_RoleGetForTransferCase_spResult
    {
        public string DisplayName { get; set; }
        public int PersonID { get; set; }
        public int? InAgencyFlag { get; set; }
        public int? TransferToAgencyID { get; set; }
        public int? CopyCaseHearingResultCodeID { get; set; }
        public string CopyCaseHearingResult { get; set; }

    }
}
