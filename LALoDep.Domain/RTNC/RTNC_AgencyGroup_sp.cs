namespace LALoDep.Domain.RTNC
{

    public class RTNC_AgencyGroup_spParams
    {
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class RTNC_AgencyGroup_spResult
    {
        public RTNC_AgencyGroup_spResult()
        {
        }
        public string AgencyGroupDisplay { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? Selected { get; set; }
    }
}
