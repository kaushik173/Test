
namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_GetAgency_spParams
    {
        public int? AgencyGroupID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class JudgeDeptTrans_GetAgency_spResult
    {
        public JudgeDeptTrans_GetAgency_spResult()
        {
        }
        public string AgencyDisplay { get; set; }
        public int? AgencyID { get; set; }
    }
}
