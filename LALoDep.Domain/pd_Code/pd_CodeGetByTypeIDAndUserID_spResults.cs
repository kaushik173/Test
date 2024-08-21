using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGetByTypeIDAndUserID_spResults
    {
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public string CodeEnumName { get; set; }
        public string ChildAddressType { get; set; }
        public string NonChildAddressType { get; set; }
        public string CodeDisplay { get; set; }
    }

    public class pd_CodeGetByTypeIDAndUserID_spResults_ForHearingResult : pd_CodeGetByTypeIDAndUserID_spResults
    {
        public int ActiveFlag { get; set; }
       
    }
    public class CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults
    {
        public int? ParentCodeID { get; set; }
        public int? ChildCodeID { get; set; }
        public string Parent { get; set; }
        public string ParentShort { get; set; }
        public string Child { get; set; }
        public string ChildShort { get; set; }
        public string CodeDisplay { get; set; }
    }

    public class CodeHierarchyGetByCodeRelationshipIDAgencyID_spParams
    {
         
        public int CodeRelationshipID { get; set; }
        public int CaseAgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? IncludeParentCodeID { get; set; }
        public int? IncludeChildCodeID { get; set; }
        
 


    }
}
