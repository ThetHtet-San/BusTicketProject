using Com.MrIT.Common;
using Com.MrIT.Common.Configuration;
using Com.MrIT.DataRepository;
using Com.MrIT.DBEntities;
using Com.MrIT.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.MrIT.Services
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly IEmailTemplateRepository _repoEmailTemplate;
        private readonly IEmailLogRepository _repoEmailLog;
        private readonly IErrorLogRepository _repoErrorLog;
        private readonly AppSettings _appSettings;
        public EmailService(IEmailTemplateRepository repoEmailTemplate, IEmailLogRepository repoEmailLog, IErrorLogRepository repoErrorLog,  IOptions<AppSettings> appSettings)
        {
            this._repoEmailTemplate = repoEmailTemplate;
            this._repoEmailLog = repoEmailLog;
            this._repoErrorLog = repoErrorLog;
            this._appSettings = appSettings.Value;
        }

        public void SendForRegistration(VmRegistrationEmail emailContent, string user)
        {
            try
            {
                //get email template
                var emailTemplate =  _repoEmailTemplate.GetByName("to_user_successful_registration");
                if (emailTemplate == null)
                {
                    //save error log
                    SaveErrorLog("Failed while send email", "to_user_successful_registration", "No template to send email", "Error Log", user);
                }
                else
                {
                    //replace with email Content
                    emailTemplate.Content = emailTemplate.Content.Replace("[Name]", emailContent.Name);
                    emailTemplate.Content = emailTemplate.Content.Replace("[Email]", emailContent.Email);
                    emailTemplate.Content = emailTemplate.Content.Replace("[URL]", _appSettings.App_Identity.PublicSiteUrl + emailContent.URL);

                    //send email
                    var emailResult = Email.SendMail(_appSettings, emailContent.Email, "", emailTemplate.Subject, emailTemplate.Content);

                    //log email
                    SaveEmailLog(emailContent.Email, "", emailTemplate.Subject, emailTemplate.Content, emailResult,user);

                }
            }
            catch (Exception ex)
            {
                InsertErrorAndEmail(ex, user, "to_user_successful_registration");
            }
        }

        public void InsertErrorAndEmail(Exception ex, string user, string url)
        {
            try
            {

                SaveErrorLog("Erron on Program", ex.StackTrace, ex.Message, url, user);

                var emailTemplate = _repoEmailTemplate.GetByName("to_siteadmin_error");
                if (emailTemplate == null)
                {
                    //save error log
                    SaveErrorLog("Failed while send email", "to_siteadmin_error", "No template to send email", "Error Log", user);
                }
                else
                {
                    string subject = emailTemplate.Subject;



                    string body = emailTemplate.Content;
                    body = body.Replace("[User]", user);
                    body = body.Replace("[Error]", ex.Message);
                    body = body.Replace("[StackTrace]", ex.StackTrace);
                    body = body.Replace("[URL]", url);

                    //Get SieAdmin
                    string siteAdmin = _appSettings.App_Identity.ErrorEmailTo;

                    var emailResult = Email.SendMail(_appSettings, siteAdmin, "", subject, body);
                    //save email log
                    SaveEmailLog(siteAdmin, "", subject, body, emailResult, user);

                }
            }
            catch (Exception ex1)
            {
                //nothing
                SaveErrorLog("Erron on Program", ex1.StackTrace, ex1.Message, "Inserting Error Log", user);
            }
        }

        public void SendForContactUs(VmContactUsEmail emailContent, string user)
        {
            try
            {                
                   //SaveErrorLog("Erron on Program", ex.StackTrace, ex.Message, url, user);
                var emailTemplate = _repoEmailTemplate.GetByName("to_send_admin_contact_us");
                if (emailTemplate == null)
                {
                    //save error log
                    //SaveErrorLog("Failed while send email", "to_send_admin_contact_us", "No template to send email", "Error Log", user);
                }
                else
                {
                    string subject = emailTemplate.Subject;

                    string body = emailTemplate.Content;
                    body = body.Replace("[Name]", emailContent.Name);
                    body = body.Replace("[Email]", emailContent.Email);
                    body = body.Replace("[Phone]", emailContent.Phone);
                    body = body.Replace("[Subject]", emailContent.Subject);
                    body = body.Replace("[Message]", emailContent.Message);

                    //Get SieAdmin
                    string siteAdmin = "zweminoo@mritmyanmar.com";//_appSettings.App_Identity.SampleEmailTo;

                    var emailResult = Email.SendMail(_appSettings, siteAdmin, "", subject, body);
                    //save email log
                    SaveEmailLog(siteAdmin, "", subject, body, emailResult, user);
                }
            }
            catch (Exception ex1)
            {
                //nothing
                //SaveErrorLog("Erron on Program", ex1.StackTrace, ex1.Message, "Inserting Error Log", user);
            }
        }

        private void SaveEmailLog(string email, string ccEmail, string subject, string content, string emailResult, string user)
        {
            try
            {
                var emailLog = new EmailLog();
                emailLog.CreatedBy = emailLog.ModifiedBy = user;
                emailLog.Receiver = email;
                emailLog.ReceiverCC = ccEmail;
                emailLog.Subject = subject;
                emailLog.Body = content;
                if (emailResult == "")
                {
                    emailLog.IsSuccess = true;
                }
                else
                {
                    emailLog.IsSuccess = false;
                }

                _repoEmailLog.Add(emailLog);
            }
            catch(Exception ex)
            {
                SaveErrorLog("Erron on Program", ex.StackTrace, ex.Message, "Saving Email Log", user);
            }
        }

        public void SaveErrorLog(string Error, string StackTrace, string Message, string URL, string CreatedBy)
        {
            try
            {
                var errorLog = new ErrorLog();
                errorLog.CreatedBy = errorLog.ModifiedBy = CreatedBy;
                errorLog.Error = Error;
                errorLog.StackTrace = StackTrace;
                errorLog.Message = Message;
                errorLog.URL = URL;

                _repoErrorLog.Add(errorLog);
                _repoErrorLog.Commit();
            }
            catch
            {
                //nothing
            }

        }

        
    }
}
