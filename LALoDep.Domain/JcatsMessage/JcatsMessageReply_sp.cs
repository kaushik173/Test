namespace LALoDep.Domain.JcatsMessage
{
    public class JcatsMessageReply_spParams
    {
        public string SmsID { get; set; }
        public string ResponseFromAddress { get; set; }
        public string MessageBody { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public string ResponseToAddress { get; set; }
        public int SendEmailFlag { get; set; }
        
    }
    public class JcatsMessageReply_spResult
    {
      
        public string Status { get; set; }
        public int? InsertedID { get; set; }
        public string recipients { get; set; }
        public string from_address { get; set; }
        public string reply_to { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string body_format { get; set; }
        public int? CaseID { get; set; }
        public string JcatsUserIDList { get; set; }
    }
}