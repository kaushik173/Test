
namespace LALoDep.Domain.pd_Motions

{
    public class pd_MotionGetHeader_spParams
    {
        public int? MotionID { get; set; }
        public int? PetitionID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_MotionGetHeader_spResult
    {
        public pd_MotionGetHeader_spResult()
        {
        }
        public string MotionHeaderDisplay { get; set; }
    }
}
