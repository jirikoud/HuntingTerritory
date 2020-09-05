using HuntingModel.Database;
using HuntingModel.Enumeration;
using HuntingModel.Infrastructure;
using HuntingModel.Localization;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HuntingModel.Context
{
    public class EmailContext
    {
        private const int MAX_SEND_COUNT = 10;
        private const int MAX_RETRY_COUNT = 5;

        private const string CREATE_ACCOUNT_TEMPLATE = "CreateAccount.html";
        private const string INVITE_USER_TEMPLATE = "InviteUser.html";
        private const string FORGOTTEN_PASSWORD_TEMPLATE = "ForgottenPassword.html";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void CreateRegistrationEmail(HuntingEntities dataContext, AclUser aclUser, int userId)
        {
            var template = TemplateContext.LoadTemplate(CREATE_ACCOUNT_TEMPLATE);
            if (template == null)
            {
                return;
            }
            var email = new EmailInfo();
            email.SysCreated = DateTime.Now;
            email.SysCreator = userId;
            email.ReceiverAddress = aclUser.Email;
            email.SendStateEx = SendStateEnum.InSending;
            email.Subject = template.Title;
            email.Message = string.Format(template.TemplateContent, 
                ContextUtils.FullyQualifiedApplicationPath, aclUser.EmailCode);
            dataContext.EmailInfoes.Add(email);
        }

        public static void CreateForgottenEmail(HuntingEntities dataContext, AclUser aclUser)
        {
            var template = TemplateContext.LoadTemplate(FORGOTTEN_PASSWORD_TEMPLATE);
            if (template == null)
            {
                return;
            }
            var email = new EmailInfo();
            email.SysCreated = DateTime.Now;
            email.SysCreator = aclUser.Id;
            email.ReceiverAddress = aclUser.Email;
            email.SendStateEx = SendStateEnum.InSending;
            email.Subject = template.Title;
            email.Message = string.Format(template.TemplateContent,
                ContextUtils.FullyQualifiedApplicationPath, aclUser.EmailCode);
            dataContext.EmailInfoes.Add(email);
        }

        public static void CreateInviteEmail(HuntingEntities dataContext, AclUser aclUser, AclUser sender)
        {
            var template = TemplateContext.LoadTemplate(INVITE_USER_TEMPLATE);
            if (template == null)
            {
                return;
            }
            var email = new EmailInfo();
            email.SysCreated = DateTime.Now;
            email.SysCreator = sender.Id;
            email.ReceiverAddress = aclUser.Email;
            email.SendStateEx = SendStateEnum.InSending;
            email.Subject = template.Title;
            email.Message = string.Format(template.TemplateContent,
                ContextUtils.FullyQualifiedApplicationPath, aclUser.EmailCode, sender.Fullname);
            dataContext.EmailInfoes.Add(email);
        }

        public static List<EmailInfo> GetSendList(HuntingEntities dataContext)
        {
            try
            {
                var emailList = dataContext.EmailInfoes.Where(item => 
                    item.IsDeleted == false && 
                    item.SendState == (int)SendStateEnum.InSending).
                    Take(MAX_SEND_COUNT).ToList();
                return emailList;
            }
            catch(Exception exception)
            {
                logger.Error(exception, "GetSendList");
                return null;
            }
        }

        public static void ProcessFailure(HuntingEntities dataContext, EmailInfo emailInfo, string errorMessage)
        {
            emailInfo.SendDate = DateTime.Now;
            emailInfo.ErrorMessage = errorMessage;
            emailInfo.RetryCount++;
            if (emailInfo.RetryCount == MAX_RETRY_COUNT)
            {
                emailInfo.SendStateEx = SendStateEnum.Failed;
            }
            dataContext.SaveChanges();
        }

        public static void ProcessSuccess(HuntingEntities dataContext, EmailInfo emailInfo)
        {
            emailInfo.SendDate = DateTime.Now;
            emailInfo.SendStateEx = SendStateEnum.Failed;
            dataContext.SaveChanges();
        }
    }
}
