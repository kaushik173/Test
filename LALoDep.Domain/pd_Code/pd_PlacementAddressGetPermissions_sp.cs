namespace LALoDep.Domain.pd_Code
{
    public class pd_PlacementAddressGetPermissions_spParams
    {
        public int? AddressID { get; set; }
        public int? PlacementAgencyCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_PlacementAddressGetPermissions_spResult
    {
       
        public string DisplayBanner { get; set; }
        public int? ReadOnlyCodeFlag { get; set; }
        public int? ReadOnlyAddressFlag { get; set; }
    }
}
