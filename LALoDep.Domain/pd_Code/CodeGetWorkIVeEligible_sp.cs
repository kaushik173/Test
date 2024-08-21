namespace LALoDep.Domain.pd_Code
{
    public class CodeGetWorkIVeEligible_spParams
    {
        public int? AgencyID { get; set; }
        public int? WorkID { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public string SortOption { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class CodeGetWorkIVeEligible_spResult
    {
        public CodeGetWorkIVeEligible_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
    }
}
