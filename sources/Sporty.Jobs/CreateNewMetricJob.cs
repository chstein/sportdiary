using Common.Logging;
using Quartz;
using Sporty.Business.Interfaces;
using Sporty.Business.Repositories;
using Sporty.Common;
using Sporty.Common.Mail;
using Sporty.DataModel;
using Sporty.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sporty.Jobs
{
    public class CreateNewMetricJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CreateNewMetricJob));

        /// <summary>
        /// Job der jede Minute in die DB schaut und täglich Mails für neue Tageswerte verschickt.
        /// </summary>
        public CreateNewMetricJob()
        {

        }

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("Execute CreateNewMetricJob");

            
            try
            {
                var connString = "";
                var mailTemplatePath = "";

                Log.InfoFormat("Current Path: {0}", Assembly.GetExecutingAssembly().Location);
                var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                Log.InfoFormat("AppSettings: {0}", config.AppSettings.Settings);
                if (config.AppSettings != null)
                {
                    var cs = config.AppSettings.Settings[Constants.ConnectionString];
                    if (cs != null)
                    {
                        Log.Info("Use connectionstring from app.config");
                        connString = cs.Value;
                    }
                    var template = config.AppSettings.Settings[Constants.CreateNewMetricJobMailTemplate];
                    if (template != null)
                    {
                        mailTemplatePath = template.Value;
                    }
                }
                var dbContext = new SportyEntities(connString);
                //var dbContext = new EntitiesDataContext(connString);
                var userRepository = new UserRepository(dbContext);
                var profileRepository = new ProfileRepository(dbContext);
                var now = DateTime.Now;
                //utc wird nicht benötigt, da die Werte täglich ausgelesen werden
                var profiles = profileRepository.GetProfiles(p => p.DailyMetricsMailSendingTime.HasValue &&
                    p.DailyMetricsMailSendingTime.Value.Hours == now.Hour && p.DailyMetricsMailSendingTime.Value.Minutes == now.Minute); 

                //var jobs = jobRepository.FindJobs(IsTimeToRun());
                Log.InfoFormat("{0} Profiles for sending Mails found.", profiles.Count());
                foreach (var profile in profiles)
                {
                    var user = userRepository.GetUser(profile.UserId);
                    if (!String.IsNullOrEmpty(user.Email))
                    {
                        Log.InfoFormat("Start to send email to {0} with template {1}", user.Email, mailTemplatePath);
                        MailHandler.SendCreateMetricMail(user.Name, user.Email, mailTemplatePath);
                        //job.LastRun = context.FireTimeUtc.Value.DateTime;
                        //var utc = DateTime.UtcNow.AddDays(1);
                        //job.NextRun = new DateTime(utc.Year, utc.Month, utc.Day, utc.Hour, 0, 0);
                        //Log.Info("Update Rundates");
                        //jobRepository.Save(job);
                    }
                }
            }
            catch (Exception exc)
            {
                Log.Error("Error during executing CreateNewMetricJob", exc);
            }
        }

    }
}
