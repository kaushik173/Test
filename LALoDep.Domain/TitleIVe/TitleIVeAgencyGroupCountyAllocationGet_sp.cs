namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeAgencyGroupCountyAllocationGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? UserID { get; set; }

    }


    public class TitleIVeAgencyGroupCountyAllocationGet_spResult
    {

        public int? TitleIVeAgencyGroupCountyAllocationID { get; set; }
        public int? AgencyGroupID { get; set; }
        public string CountyName { get; set; }
        public decimal? CACAllocation { get; set; }
        public decimal? CountyPercent { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public int? RecordStateID { get; set; }
     
    }
}
