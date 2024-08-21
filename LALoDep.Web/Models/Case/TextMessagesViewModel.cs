using LALoDep.Domain.JcatsMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Case
{
    public class TextMessagesViewModel
    {
        public string OtherMobilePhone { get; set; }
        public string Message { get; set; }
        public string DisabledMessage { get; set; }
        public bool IsDisabled { get; set; }

        public List<JcatsMessageGetSendTo_spResult> MessageSendToList { get; set; }
        public List<JcatsMessageGetHistoryForCase_spResult> MessageHistory { get; set; }
        public JcatsMessageGetPermissions_spResult MessagePermission { get; set; }

    }
}