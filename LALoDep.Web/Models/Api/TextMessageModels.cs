using LALoDep.Domain.JcatsMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Api
{
    public class TextMessageSendToModel
    {
        public List<JcatsMessageGetSendTo_spResult> SendTo { get; set; }
        public bool IsDisabled { get; set; }
        public string DisabledMessage { get; set; }
    }

    public class SendTextMessagesModel
    {
        public int CaseID { get; set; }        
        public string Message { get; set; }
        public List<JcatsMessageSendToNumber> MessageSendToList { get; set; }
    }

    public class JcatsMessageSendToNumber
    {

        public string SendTo { get; set; }
        public int? RoleID { get; set; }
        public byte? RoleClient { get; set; }
    }
}