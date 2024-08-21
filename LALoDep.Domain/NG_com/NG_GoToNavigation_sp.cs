namespace LALoDep.Domain.NG_com
{
    public class NG_GoToNavigation_spParams
    {
        public string NG_AspPageName { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? PetitionID { get; set; }
        public int? RoleID { get; set; }
        public string OtherEntityType { get; set; }
        public int? OtherEntityID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class NG_GoToNavigation_spResult
    {
        public NG_GoToNavigation_spResult()
        {
        }
        public int? DisplayOrder { get; set; }
        public string GoToDisplayName { get; set; }
        public string GoToURL { get; set; }
    }
}
