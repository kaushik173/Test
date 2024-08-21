using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace LALoDep.Custom.Twillio
{
    public class TwilioSMSHelper
    {
        private static string AccountSID => ConfigurationManager.AppSettings["AccountSID"];
        private static string AuthToken => ConfigurationManager.AppSettings["AuthToken"];
        private static string MessagingServiceSid => ConfigurationManager.AppSettings["MessagingServiceSid"];
        private static string FromPhoneNumber => ConfigurationManager.AppSettings["FromPhoneNumber"];
        private static string WebhookBaseUrl => ConfigurationManager.AppSettings["WebhookBaseUrl"];



        private readonly ITwilioRestClient _client;
        public TwilioSMSHelper()
        {
            _client = new TwilioRestClient(AccountSID, AuthToken);
        }
        public TwilioSMSHelper(ITwilioRestClient client)
        {
            _client = client;
        }

        public async Task<MessageResource> SendMessage(string sendTo, string message, string sendFrom = null)
        {
            sendTo = sendTo.StartsWith("+") ? sendTo : "+" + sendTo;

            var response = await MessageResource.CreateAsync(
               to: new PhoneNumber(sendTo),

               from: string.IsNullOrEmpty(sendFrom) ? null : new PhoneNumber(sendFrom),

               messagingServiceSid: MessagingServiceSid,
               body: message,
               client: _client, 
               statusCallback: new Uri(WebhookBaseUrl + @"TwilioWebhook/UpdateStatus")
               );
            return response;
        }
    }
}