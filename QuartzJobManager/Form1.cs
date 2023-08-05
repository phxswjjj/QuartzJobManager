using Quartz;
using Quartz.Impl;
using QuartzJobManager.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuartzJobManager
{
    public partial class Form1 : Form
    {
        private IScheduler Scheduler;

        public Form1()
        {
            InitializeComponent();

            InitializeQuartz();
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
                .StartNow()
                .WithSimpleSchedule((builder) =>
                {
                    builder.WithIntervalInSeconds(10)
                        .RepeatForever();
                })
                .Build();

            var every20s = TriggerBuilder
                .Create()
                .StartNow()
                .WithSimpleSchedule((builder) =>
                {
                    builder.WithIntervalInSeconds(20)
                        .RepeatForever();
                })
                .Build();

            scheduler.ScheduleJob(job1, every10s);
            scheduler.ScheduleJob(job2, every20s);

            this.Scheduler = scheduler;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Scheduler.Start();
        }
    }
}
