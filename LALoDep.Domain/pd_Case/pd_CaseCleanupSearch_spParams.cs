using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseCleanupSearch_spParams
    {
        public int? AgencyID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_CaseCleanupSearch_spResult
    {
        public string ASPActionDisplay { get; set; }
        public string ASPAction { get; set; }
        public string NG_NavigationURL { get; set; }
        public int? SortOrder { get; set; }
        public string SearchType { get; set; }
        public int CaseID { get; set; }
        public string Client { get; set; }
        public string CaseName { get; set; }
        public string PetitionNumber { get; set; }
        public string CaseNumber { get; set; }
        public string Attorney { get; set; }
        public string InsertedOn { get; set; }
        public string InsertedBy { get; set; }
        public string SortInsetedOn { get; set; }
        public string CustomSort { get; set; }
    }
}
