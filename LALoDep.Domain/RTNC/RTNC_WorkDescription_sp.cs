namespace LALoDep.Domain.RTNC
{

    public class RTNC_WorkDescription_spParams
    {
        public int? WorkID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class RTNC_WorkDescription_spResult
    {
        public RTNC_WorkDescription_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public int? Selected { get; set; }
        public int? DefaultIVeEligibleCodeID { get; set; }
        public int? DefaultCanChangeFlag { get; set; }
        public int? AttorneyDefaultIVeEligibleCodeID { get; set; }
        public int? AttorneyDefaultCanChangeFlag { get; set; }
        public int? SupervisorDefaultIVeEligibleCodeID { get; set; }
        public int? SupervisorDefaultCanChangeFlag { get; set; }
    }
}
