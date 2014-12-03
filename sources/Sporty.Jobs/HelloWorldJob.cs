using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sporty.Jobs
{
    public class HelloWorldJob : IJob
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(HelloWorldJob));

        /// <summary> 
        /// Empty constructor for job initilization
        /// <para>
        /// Quartz requires a public empty constructor so that the
        /// scheduler can instantiate the class whenever it needs.
        /// </para>
        /// </summary>
        public HelloWorldJob()
        {

        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Log.ErrorFormat("{0}****{0}Job {1} fired @ {2} next scheduled for {3}{0}***{0}",
                                                                        Environment.NewLine,
                                                                        context.JobDetail.Key,
                                                                        context.FireTimeUtc.Value.ToString("r"),
                                                                        context.NextFireTimeUtc.Value.ToString("r"));


                Log.ErrorFormat("{0}***{0}Hello World!{0}***{0}", Environment.NewLine);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("{0}***{0}Failed: {1}{0}***{0}", Environment.NewLine, ex.Message);
            }
        }
    }
}
