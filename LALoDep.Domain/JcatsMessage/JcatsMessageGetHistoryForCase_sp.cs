namespace LALoDep.Domain.JcatsMessage
{
    public class JcatsMessageGetHistoryForCase_spParams
    {
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
    }

    public class JcatsMessageGetHistoryForCase_spResult
    {
        public int? MessageGroupID { get; set; }
        public int? JcatsMessageID { get; set; }
        public string SmsId { get; set; }
        public string MessageType { get; set; }
        public string MessageDateTime { get; set; }
        public string PhoneDisplay { get; set; }
        public string RoleDisplay { get; set; }
        public string MessageStatus { get; set; }
        public string SentBy { get; set; }
        public string MessageBody { get; set; }
        public string SortMessageDateTime { get; set; }
        public int SortingID { get; set; }
    }
}