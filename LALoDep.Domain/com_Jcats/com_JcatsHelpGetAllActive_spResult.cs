using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcats.SD.Domain.com_Jcats
{
    public class com_JcatsHelpGetAllActive_spResult
    {
        public string JcatsHelpGroupDisplayName { get; set; }
        public string JcatsHelpPageDisplayName { get; set; }
        public string JcatsHelpPageName { get; set; }
        public int JcatsHelpGroupDisplayOrder { get; set; }
        public byte JcatsHelpSelectedFlag { get; set; }
        public int JcatsHelpID { get; set; }
    }

    public class JcatsTutorialGetList_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? UserID { get; set; }
   
    }
    public class JcatsTutorialGetList_spResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public int? DisplayOrder { get; set; }
     
    }


}
