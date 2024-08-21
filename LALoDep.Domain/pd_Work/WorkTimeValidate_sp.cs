using System;

namespace LALoDep.Domain.pd_Work
{
    public class WorkTimeValidate_spParams
    {
        public int? WorkID { get; set; }
        public int? PersonID { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class WorkTimeValidate_spResult
    {
        public WorkTimeValidate_spResult()
        {
        }
        public string ValidationMessage { get; set; }
    }
}
