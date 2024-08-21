namespace LALoDep.Domain.JcatsMessage
{
    
           public class JcatsMessageOptOutCheck_spParams
    {
        public string  AddressToCheck { get; set; }
      
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }


    }
    public class JcatsMessageOptOutCheck_spResult
    {
        public string AlertDisplay { get; set; } 


    }
    public class JcatsMessageInsert_spParams
    {
        public string InsertMode { get; set; }
        public int? JcatsMessageID { get; set; }
        public int? CaseID { get; set; }
        public string EntityType { get; set; }
        public int? EntityID { get; set; }
        public int? ResponseRegardingJcatsMessageID { get; set; }
        public int? ResponseFromAddress { get; set; }
        public string AddressSentTo { get; set; }
        public string JcatsMessageType { get; set; }
        public string MessageBody { get; set; }
        public string DeliveryStatus { get; set; }
        public System.DateTime? DateTimeSent { get; set; }
        public string SmsId { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int? DoNotReturnRecordset { get; set; }

    }
    public class JcatsMessageInsert_spResult
    {
        public int? JcatsMessageID { get; set; }
        public int? CaseID { get; set; }
        public string EntityType { get; set; }
        public int? EntityID { get; set; }
        public int? ResponseRegardingJcatsMessageID { get; set; }
        public string ResponseFromAddress { get; set; }
        public string AddressSentTo { get; set; }
        public string JcatsMessageType { get; set; }
        public string MessageBody { get; set; }
        public string DeliveryStatus { get; set; }
        public System.DateTime? DateTimeSent { get; set; }
        public string SmsId { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.DateTime? InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.DateTime? UpdatedOnDateTime { get; set; }
        public byte? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
        public string SentFrom { get; set; }
    }
}