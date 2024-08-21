using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Calendar
{
    public class qa_CaseloadGetForHearingAdd_spParams
    {
        public int AttorneyPersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }


    public class qa_CaseloadGetForHearingAdd_spResult
    {
        
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string ClientName { get; set; }
        public string ClientDOB { get; set; }
        public string NextHearingType { get; set; }
        public string NextHearingDate { get; set; }
        public string NextHearingDept { get; set; }
        public string PetitionNumber { get; set; }
        public string CaseNumber { get; set; }
    }
}
