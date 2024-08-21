using System;

namespace LALoDep.Domain.pd_Training
{
    public class pd_TrainingGetSummary_spParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? AgencyID { get; set; }
        public int? IncludeAllActiveStaff { get; set; }
        public int? VenueCodeID { get; set; }
        public int? SupervisorPersonID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_TrainingGetSummary_spResult
    {
        public pd_TrainingGetSummary_spResult()
        {
        }
        public string RoleType { get; set; }
        public string PersonNameDisplay { get; set; }
        public int? PersonID { get; set; }
        public decimal? Total { get; set; }
        public decimal? ATS { get; set; }
        public decimal? CI { get; set; }
        public decimal? Bias { get; set; }
        public decimal? Ethics { get; set; }
        public decimal? General { get; set; }
        public decimal? LOM { get; set; }
        public decimal? SA { get; set; }
        public decimal? SL { get; set; }
        public decimal? Other { get; set; }
    }
}
