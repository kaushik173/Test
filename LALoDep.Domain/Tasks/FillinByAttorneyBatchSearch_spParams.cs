using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Tasks
{
    public class FillinByAttorneyBatchSearch_spParams
    {
        public int? AgencyID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AssignedAttorneyPersonID { get; set; }
        public int? DepartmentCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }









    public class FillinByAttorneyBatchAdd_spParams
    {

        public string HearingDate { get; set; }

        public int? HearingID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? FillinAttorneyPersonID { get; set; }



        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class AttorneyFillinIUD_spParams
    {

        public string IUD { get; set; }
        public DateTime? AttorneyFillinStartDate { get; set; }
        public DateTime? AttorneyFillinEndDate { get; set; }
        public int? AttorneyFillinDepartmentCodeID { get; set; }
        public int? AttorneyFillinID { get; set; }
        public int? ForPersonID { get; set; }
        public int? PersonID { get; set; }
        public int? RecordStateID { get; set; }

        public DateTime? RecordTimeStamp { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class FillinByAttorneyBatchSearch_spResult
    {
        public string HearingDate { get; set; }
        public string HearingType { get; set; }
        public string HearingDept { get; set; }
        public string RootCaseNbr { get; set; }
        public string CaseName { get; set; }
        public string AssignedAttorney { get; set; }
        public string FillinAttorney { get; set; }
        public int? HearingID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public string SortDateTime { get; set; }
    }

    public class AttorneyFillinIUD_spResult
    {
        public int AttorneyFillinID { get; set; }

    }
}
