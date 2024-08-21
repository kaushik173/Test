using System.Collections.Generic;

namespace LALoDep.Models
{
    public class EmailRecipient
    {

        public string ToName { get; set; }
        public string ToAddress { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }


        public Dictionary<string, object> CustomData = new Dictionary<string, object>();
    }
}