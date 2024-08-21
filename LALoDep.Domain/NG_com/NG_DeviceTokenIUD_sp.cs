namespace LALoDep.Domain.NG_com
{
    public class NG_DeviceTokenIUD_spParams
    {
        public string IUD { get; set; }
        public int? DeviceTokenID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public string DeviceType { get; set; }
        public short? RecordStateID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class NG_DeviceTokenIUD_spResult
    {
        public int? DeviceTokenID { get; set; }
        public int? UserID { get; set; }
        public string Token { get; set; }
        public string DeviceType { get; set; }
        public System.DateTime? InsertedDateTime { get; set; }
        public System.DateTime? UpdatedDateTime { get; set; }
        public short? RecordStateID { get; set; }

    }
}