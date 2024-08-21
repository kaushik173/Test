namespace Jcats.SD.Domain.COAC
{
    public class coac_AttorneyList_spParams
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? UserID { get; set; }

    }


    public class coac_AttorneyList_spResult
    {
        public coac_AttorneyList_spResult()
        {
        }
        public string AttorneyDisplay { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int? AttorneyAgencyID { get; set; }
    }
}
