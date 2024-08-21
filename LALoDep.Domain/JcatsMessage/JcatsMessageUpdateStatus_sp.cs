namespace LALoDep.Domain.JcatsMessage
{
    public class JcatsMessageUpdateStatus_spParams
    {
        public int? JcatsMessageID { get; set; }
        public string SmsId { get; set; }
        public string DeliveryStatus { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public string  DebugErrorMessage { get; set; }
    }

}