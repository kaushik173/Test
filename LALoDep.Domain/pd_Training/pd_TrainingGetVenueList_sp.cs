namespace LALoDep.Domain.pd_Training
{
    public class pd_TrainingGetVenueList_spParams
    {
        public int? TrainingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }

    }


    public class pd_TrainingGetVenueList_spResult
    {
        public pd_TrainingGetVenueList_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public int? SortOrder { get; set; }
    }
}
