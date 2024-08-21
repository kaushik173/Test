using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_UserGroups
{
    public class pd_SecurityDelete_spParams
    {
        public int JcatsGroupID{ get; set; }
        public int SecurityItemID { get; set; }
        public int? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LoadOption{ get; set; }
    }
    public class pd_SecurityGetAllBySecurityItemID_spParams
    {
        public int SecurityItemID { get; set; }
        public int? AgencyID { get; set; }
        public string JcatsGroupName { get; set; }
    
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
      
    }
    public class pd_SecurityGetAllBySecurityItemID_spResults
    {
        public int JcatsGroupID { get; set; }
        public int SecurityCategoryID { get; set; }
        public string JcatsGroupName { get; set; }
        public string SecurityItemDisplay { get; set; }
        public string SecurityCategory { get; set; }
        public int HasSecurityFlag { get; set; }
      

    }
    











}
