using System.Configuration;
using ActionMailer.Net;
using ActionMailer.Net.Mvc;
using LALoDep.Models;
using LALoDep.Core.Custom.Utility;

namespace LALoDep.Controllers
{
    public partial class MailController : MailerBase
    {
        readonly string _toAddress = ConfigurationManager.AppSettings["DebugEmail"];



        // GET: Mail
        public virtual EmailResult ResetPassword(EmailRecipient model)
        {
            To.Add(!string.IsNullOrEmpty(_toAddress) ? _toAddress : model.ToAddress);
            From = !string.IsNullOrEmpty(model.FromAddress) ? model.FromAddress : GetFromEmail();

            Subject = "Reset Password";

            return Email("ResetPassword", model);
        }

        private static string GetFromEmail()
        {
            var settings = ConfigurationManager.AppSettings["FromEmail"]; //(SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            return settings ?? "";
        }

        // GET: Mail
        public virtual EmailResult RaiseElmahErrorEmail(EmailRecipient model)
        {
            To.Add(!string.IsNullOrEmpty(_toAddress) ? _toAddress : model.ToAddress);
            From = !string.IsNullOrEmpty(model.FromAddress) ? model.FromAddress : GetFromEmail();

            Subject = "Elmah Error: " + model.CustomData["ErrorMessage"].ToString();

            return Email("RaiseElmahErrorEmail", model);
        }
        public virtual EmailResult ClearConsernEmailNotification(EmailRecipient model)
        {
            To.Add(!string.IsNullOrEmpty(_toAddress) ? _toAddress : model.ToAddress);
            From = !string.IsNullOrEmpty(model.FromAddress) ? model.FromAddress : GetFromEmail();
             
            Subject = !string.IsNullOrEmpty(model.Subject) ? model.Subject : "Hello";

            return Email("ClearConsernEmailNotification", model);
        }
        public virtual EmailResult TwilioReplyEmailNotification(EmailRecipient model)
        {
            To.Add(!string.IsNullOrEmpty(_toAddress) ? _toAddress : model.ToAddress);
            From = !string.IsNullOrEmpty(model.FromAddress) ? model.FromAddress : GetFromEmail();

            Subject = !string.IsNullOrEmpty(model.Subject) ? model.Subject : "Hello";

            return Email("ClearConsernEmailNotification", model);
        }


        protected override void OnMailSending(MailSendingContext context)
        {

            context.Cancel = true;
            var smtp = Utility.GetSmtpClient();
            smtp.Send(context.Mail);
        }

    }
}