namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkGetFilterByStaffType_spParams
    {
        public int? CaseID { get; set; }
        public int? ReferralID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_WorkGetFilterByStaffType_spResult
    {
      
        public string FilterBy { get; set; }
        public int? DefaultFlag { get; set; }
    }
}
