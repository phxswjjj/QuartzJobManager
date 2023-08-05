using Quartz;
using QuartzJobManager.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzJobManager.Jobs
{
    //同一個 JobKey 只會有一個執行個體
    [DisallowConcurrentExecution]
    internal class Job1 : IJob
    {
        public Task Execute(IJobExecutionContext context) => Task.Run(() => Run(context));

        private void Run(IJobExecutionContext context)
        {
            var job = context.JobDetail;

            var uid = this.GetHashCode();
            var logger = LogFactory.Create<Job1>()
                .ForContext("uid", uid)
                .ForContext("JobKey", job.Key);

            logger.Information("{JobKey} {uid} running");

            //給不同 Seed 才會在用一個時間點產生不同亂數結果
            var rnd = new Random(uid);
            var wait = rnd.Next(5_000, 40_000);
            logger.ForContext("wait", wait)
                .Information("{JobKey} {uid} {wait:N0}ms..");
            System.Threading.Thread.Sleep(wait);

            logger.Information("{JobKey} {uid} completed");
        }
    }
}
