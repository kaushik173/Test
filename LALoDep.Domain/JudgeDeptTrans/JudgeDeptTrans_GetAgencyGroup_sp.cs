
namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_GetAgencyGroup_spParams
    {
        public int? AgencyGroupID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class JudgeDeptTrans_GetAgencyGroup_spResult
    {
         
        public string AgencyGroupDisplay { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? Selected { get; set; }
    }
}
