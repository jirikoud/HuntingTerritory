using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SendGrid;
using System.Threading;
using HuntingModel.Context;
using HuntingWebJob.Properties;
using NLog;

namespace HuntingWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static EmailSenderJob emailSenderJob = new EmailSenderJob();
        private static AclUserJob aclUserJob = new AclUserJob();
        private static DemoAccountJob demoAccountJob = new DemoAccountJob();

        static void Main()
        {
            int counter = 0;
            logger.Info("HuntingWebJob started");
            while (true)
            {
                try
                {
                    if (counter % Settings.Default.EmailSenderWaitSec == 0)
                    {
                        logger.Info("Processing EmailSenderJob...");
                        emailSenderJob.ProcessJob();
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "EmailSenderJob");
                }

                try
                {
                    if (counter % Settings.Default.AclUserWaitSec == 0)
                    {
                        logger.Info("Processing AclUserJob...");
                        aclUserJob.ProcessJob();
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "AclUserJob");
                }

                try
                {
                    if (counter % Settings.Default.DemoAccountWaitSec == 0)
                    {
                        logger.Info("Processing DemoAccountJob...");
                        demoAccountJob.ProcessJob();
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(exception, "DemoAccountJob");
                }

                counter++;
                Thread.Sleep(1000);
            }
        }
    }
}
