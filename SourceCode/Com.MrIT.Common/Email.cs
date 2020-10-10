using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Com.MrIT.Common
{
    public static class Email
    {
        public static string SendMail(Configuration.AppSettings appSettings, string toMail, string cCMail, string subject, string body, List<Attachment> attachments = null)
        {
            var result = "";
            try
            {

                var fromAddress = new MailAddress(appSettings.App_Identity.fromMail, appSettings.App_Identity.Name);

                if (appSettings.App_Identity.SampleEmailTo != "")
                {
                    toMail = appSettings.App_Identity.SampleEmailTo;
                    cCMail = "";
                }


                var smtp = new SmtpClient
                {
                    Host = appSettings.App_Identity.MailServer,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(appSettings.App_Identity.fromMail, appSettings.App_Identity.fromMailPassword)
                };

                MailMessage msgMail = new MailMessage();
                msgMail.From = fromAddress;
                msgMail.IsBodyHtml = true;
                msgMail.Subject = subject;
                msgMail.Body = body;

                String strTo = toMail;
                char[] toDelimiter = new[] { ';' };
                var toArr = strTo.Split(toDelimiter, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < toArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(toArr[i].ToString().Trim()))
                    {
                        msgMail.To.Add(toArr[i].ToString().Trim());
                    }
                }

                String strCC = cCMail;
                char[] ccDelimiter = new[] { ';' };
                var ccArr = strCC.Split(ccDelimiter, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < ccArr.Length; j++)
                {
                    if (!string.IsNullOrEmpty(ccArr[j].ToString().Trim()))
                    {
                        msgMail.CC.Add(ccArr[j].ToString().Trim());
                    }
                }
                if (attachments != null)
                {
                    foreach (var _attach in attachments)
                    {
                        System.Net.Mail.Attachment data = new System.Net.Mail.Attachment(_attach.ToString());
                        msgMail.Attachments.Add(data);
                    }
                }
                msgMail.Priority = MailPriority.Normal;
                smtp.Send(msgMail);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}
