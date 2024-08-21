using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models
{
    public class CaseSearchViewModel:DataTablePerameters
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocketNumber { get; set; }
        public string JcatsNumber { get; set; }
        public bool Appointment { get; set; }
        public string HHSA { get; set; }
        public int StartRecord { get; set; }
        public int Range { get; set; }
        public int SortID { get; set; }
        public int AgencyID { get; set; }
        public int UserID { get; set; }
        public bool OnViewLoad { get; set; }
        public short OnlyOpenCases { get; set; }
        public string CaseAccessLevelMessage { get; set; }
        public List<AgencyModel> AgancyList { get; set; }
        public string SearchGuid { get; set; }
        public bool ParamChanged { get; set; }
    }
}