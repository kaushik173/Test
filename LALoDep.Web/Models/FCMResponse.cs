using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class FCMResponse
    {
        public FCMResponse()
        {
            Results = new List<MessageResult>();
        }

        [JsonProperty(PropertyName = "multicast_id")]
        public long MulticastId { get; set; }
        [JsonProperty(PropertyName = "success")]
        public long Success { get; set; }
        [JsonProperty(PropertyName = "failure")]
        public long Failure { get; set; }
        [JsonProperty(PropertyName = "canonical_ids")]
        public long CanonicalIds { get; set; }
        [JsonProperty(PropertyName = "results")]
        public IList<MessageResult> Results { get; set; }
    }

    public class MessageResult
    {
        [JsonProperty(PropertyName = "message_id")]
        public string MessageId { get; set; }
        [JsonProperty(PropertyName = "registration_id")]
        public string RegistrationId { get; set; }
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}