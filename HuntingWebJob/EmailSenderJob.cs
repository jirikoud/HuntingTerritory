using HuntingModel.Context;
using HuntingModel.Database;
using HuntingWebJob.Properties;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HuntingWebJob
{
    public class EmailSenderJob
    {
        private string GetSenderAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return Settings.Default.EmailSenderDefaultAddress;
            }
            return address;
        }

        private bool SendEmail(HuntingEntities dataContext, EmailInfo email, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                var myMessage = new SendGridMessage();
                var address = GetSenderAddress(email.SenderAddress);
                myMessage.From = new MailAddress(address);
                if (string.IsNullOrWhiteSpace(Settings.Default.TestServerEmail))
                {
                    myMessage.AddTo(email.ReceiverAddress);
                }
                else
                {
                    myMessage.AddTo(Settings.Default.TestServerEmail);
                }
                myMessage.Subject = email.Subject;

                //Add the HTML and Text bodies
                myMessage.Html = email.Message;
                myMessage.Text = email.Message;

                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                if (apiKey == null)
                {
                    apiKey = "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";
                }
                var transportWeb = new Web(apiKey);
                transportWeb.DeliverAsync(myMessage).Wait();
                return true;
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                return false;
            }
        }

        public void ProcessJob()
        {
            using (var dataContext = new HuntingEntities())
            {
                var emailList = EmailContext.GetSendList(dataContext);
                if (emailList != null)
                {
                    foreach (var emailInfo in emailList)
                    {
                        string errorMessage;
                        var isSuccess = SendEmail(dataContext, emailInfo, out errorMessage);
                        if (isSuccess)
                        {
                            EmailContext.ProcessSuccess(dataContext, emailInfo);
                        }
                        else
                        {
                            EmailContext.ProcessFailure(dataContext, emailInfo, errorMessage);
                        }
                    }
                }
            }
        }
    }
}
