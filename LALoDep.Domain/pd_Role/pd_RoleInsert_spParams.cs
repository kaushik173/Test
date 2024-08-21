using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleInsert_spParams
    {
        public int RoleID { get; set; }
        public int? AgencyID { get; set; }
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public byte RoleClient { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int MobileCaseInsertProcessFlag { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }





    }

    public class pd_RoleUpdate_spParams
    {
        public int RoleID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public int RoleTypeCodeID { get; set; }
        public byte RoleClient { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }        
    }    
}
