using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporty.Jobs
{
    public class ScheduledJob : IScheduledJob
    {

        /// <summary>
        /// wird derzeit nicht aufgerufen, evtl. später verwenden
        /// </summary>
        public void Run()
        {
            // Get an instance of the Quartz.Net scheduler
            var schd = GetScheduler();

            // Start the scheduler if its in standby
            if (!schd.IsStarted)
                schd.Start();

            // Define the Job to be scheduled
            var job = JobBuilder.Create<HelloWorldJob>()
                .WithIdentity("WriteHelloToLog", "IT")
                .RequestRecovery()
                .Build();
            // construct job info

            // Associate a trigger with the Job
            var trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithIdentity("WriteHelloToLog", "IT")
                .WithCronSchedule("0 0/1 * 1/1 * ? *") // visit http://www.cronmaker.com/ Queues the job every minute
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();

            // Validate that the job doesn't already exists
            if (schd.CheckExists(new JobKey("WriteHelloToLog", "IT")))
            {
                schd.DeleteJob(new JobKey("WriteHelloToLog", "IT"));
            }

            var schedule = schd.ScheduleJob(job, trigger);
            //schd.Start();
            Console.WriteLine("Job '{0}' scheduled for '{1}'", "WriteHelloToLog", schedule.ToString("r"));
        }

        // Get an instance of the Quartz.Net scheduler
        private static IScheduler GetScheduler()
        {
            try
            {
                var properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "ServerScheduler";

                // set remoting expoter
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = string.Format("tcp://{0}:{1}/{2}", "localhost", "555",
                                                                             "QuartzScheduler");

                // Get a reference to the scheduler
                var sf = new StdSchedulerFactory(properties);

                return sf.GetScheduler();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Scheduler not available: '{0}'", ex.Message);
                throw;
            }
        }
    }
}
