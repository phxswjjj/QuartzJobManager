using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using QuartzJobManager.Jobs;
using QuartzJobManager.Utility;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuartzJobManager
{
    public partial class Form1 : Form
    {
        private IScheduler Scheduler;
        private readonly ILogger LogService;

        public Form1()
        {
            InitializeComponent();

            var logger = LogFactory.Create<Form1>();
            this.LogService = logger;

            InitializeQuartz();

            InitializeScanJobStatisticTask();
        }

        private void InitializeScanJobStatisticTask()
        {
            //隨 Process 生死不特別處理
            Task.Factory.StartNew(() =>
            {
                var logger = LogFactory.Create("ScanJobStatisticTask");
                logger.Information("{ModuleId} init");

                var scheduler = this.Scheduler;

                while (true)
                {
                    var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result;
                    var totalJobs = jobKeys.Count;

                    var jobs = scheduler.GetCurrentlyExecutingJobs().Result;
                    var totalRunningJobs = jobs.Count;

                    tslJobStatistic.Text = $"running: {totalRunningJobs} / {totalJobs}";

                    Thread.Sleep(200);
                }
            });
        }

        private void InitializeQuartz()
        {
            var scheduler = new StdSchedulerFactory().GetScheduler().Result;
            var job1 = JobBuilder.Create<Job1>()
                .WithIdentity("Job1")
                .Build();
            var job2 = JobBuilder.Create<Job1>()
                .WithIdentity("Job2")
                .Build();

            var every10s = TriggerBuilder
                .Create()
                .WithSimpleSchedule((builder) =>
                {
                    builder.WithIntervalInSeconds(10)
                        .RepeatForever();
                })
                .Build();

            var every20s = TriggerBuilder
                .Create()
                .WithSimpleSchedule((builder) =>
                {
                    builder.WithIntervalInSeconds(20)
                        .RepeatForever();
                })
                .Build();

            var every30s = TriggerBuilder
                .Create()
                .WithCronSchedule("*/30 * * ? * * *")
                .Build();

            scheduler.ScheduleJob(job1, every10s);
            scheduler.ScheduleJob(job2, every20s);
            scheduler.ScheduleJob(job2, every30s);

            this.Scheduler = scheduler;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Scheduler.Start();
        }
    }
}
