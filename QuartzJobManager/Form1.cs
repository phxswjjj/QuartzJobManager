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

        const string ColNameJobGroup = "ColJobGroup";
        const string ColNameJobKey = "ColJobKey";
        const string ColNameJobDescription = "ColJobDescription";
        const string ColNameJobStatus = "ColJobStatus";
        const string ColNameJobRunTime = "ColJobRunTime";
        const string ColNameJobPreviousFiredTime = "ColJobPreviousFiredTime";
        const string ColNameJobNextFireTime = "ColJobNextFireTime";

        private readonly CancellationTokenSource FormClosingSignal;
        private readonly List<Task> Tasks = new List<Task>();

        public Form1()
        {
            InitializeComponent();

            InitializeJobViewer();

            InitializeQuartz();

            var formClosingSignal = new CancellationTokenSource();
            this.FormClosingSignal = formClosingSignal; ;

            InitializeScanJobStatisticTask();

            InitializeMonitorJobTask();
        }

        private void InitializeMonitorJobTask()
        {
            var token = this.FormClosingSignal.Token;

            //隨 Process 生死不特別處理
            var task = Task.Factory.StartNew(() =>
            {
                var logger = LogFactory.Create("MonitorJobTask");
                logger.Information("{ModuleId} init");

                while (true)
                {
                    //延遲啟動，否則 Invoke 不好處理
                    if (!this.InvokeRequired)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    if (token.IsCancellationRequested)
                    {
                        logger.Information("{ModuleId} Cancel");
                        return;
                    }

                    try
                    {
                        this.Invoke(new MethodInvoker(MonitorJob));
                    }
                    catch (ObjectDisposedException)
                    {
                        //ignore: 關閉程式時會拋錯誤
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    Thread.Sleep(200);
                }
            }, token);
            this.Tasks.Add(task);
        }
        private void MonitorJob()
        {
            var scheduler = this.Scheduler;
            var gv = this.dgvJobs;

            var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result;
            var removeableJobKeys = new List<JobKey>(jobKeys);

            var removeRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow grow in gv.Rows)
            {
                var job = (IJobDetail)grow.Tag;

                if (removeableJobKeys.Contains(job.Key))
                {
                    //update job
                    job = scheduler.GetJobDetail(job.Key).Result;
                    BindData(grow, job);
                }
                else
                {
                    //remove job(add list)
                    removeRows.Add(grow);
                }
                removeableJobKeys.Remove(job.Key);
            }
            //execute remove job
            foreach (var removeRow in removeRows)
                gv.Rows.Remove(removeRow);

            //add job
            foreach (var jobKey in removeableJobKeys)
            {
                gv.Rows.Add();
                int newRowIndex = gv.Rows.GetLastRow(DataGridViewElementStates.None);
                var newRow = gv.Rows[newRowIndex];
                var job = scheduler.GetJobDetail(jobKey).Result;
                BindData(newRow, job);
            }

            var executingJobs = scheduler.GetCurrentlyExecutingJobs().Result;
            foreach (DataGridViewRow grow in gv.Rows)
            {
                var job = (IJobDetail)grow.Tag;
                var executingJob = executingJobs.FirstOrDefault(j => j.JobDetail.Key == job.Key);

                var cellStatus = grow.Cells[ColNameJobStatus];
                var cellRunTime = grow.Cells[ColNameJobRunTime];

                if (executingJob != null)
                {
                    cellRunTime.Value = executingJob.JobRunTime.ToString(@"hh\:mm\:ss\.fff");
                    if (IsJobPaused(scheduler, job))
                        cellStatus.Value = "Running -> Paused";
                    else if (scheduler.InStandbyMode)
                        cellStatus.Value = "Running -> Standby";
                    else
                        cellStatus.Value = "Running";
                }
                else if (IsJobPaused(scheduler, job))
                    cellStatus.Value = "Paused";
                else if (scheduler.InStandbyMode)
                    cellStatus.Value = "Standby";
                else
                    cellStatus.Value = "Idle";
            }
        }
        private bool IsJobPaused(IScheduler scheduler, IJobDetail job)
        {
            var triggers = scheduler.GetTriggersOfJob(job.Key).Result;
            foreach (var trigger in triggers)
            {
                var triggerState = scheduler.GetTriggerState(trigger.Key).Result;
                if (triggerState == TriggerState.Paused)
                    return true;
            }
            return false;
        }

        private void BindData(DataGridViewRow newRow, IJobDetail job)
        {
            var gv = newRow.DataGridView;
            var scheduler = this.Scheduler;

            newRow.Tag = job;
            newRow.Cells[ColNameJobGroup].Value = job.Key.Group;
            newRow.Cells[ColNameJobKey].Value = job.Key.Name;
            newRow.Cells[ColNameJobDescription].Value = job.Description;

            var triggers = scheduler.GetTriggersOfJob(job.Key).Result;
            var prevFiredTime = triggers.Max(t => t.GetPreviousFireTimeUtc());
            var prevFiredTimeText = "NA";
            if (prevFiredTime.HasValue)
                prevFiredTimeText = prevFiredTime.Value.LocalDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            newRow.Cells[ColNameJobPreviousFiredTime].Value = prevFiredTimeText;

            var nextFireTime = triggers.Min(t => t.GetNextFireTimeUtc());
            var nextFireTimeText = "NA";
            if (nextFireTime.HasValue)
                nextFireTimeText = nextFireTime.Value.LocalDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            newRow.Cells[ColNameJobNextFireTime].Value = nextFireTimeText;
        }

        private void InitializeJobViewer()
        {
            var gv = this.dgvJobs;

            gv.AutoGenerateColumns = false;
            gv.AllowUserToAddRows = false;
            gv.AllowUserToDeleteRows = false;
            gv.ReadOnly = true;

            var generateTextColumn = new Func<string, string, DataGridViewTextBoxColumn>((name, title) =>
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = title
                };
                return col;
            });

            var colJobGroup = generateTextColumn(ColNameJobGroup, "Group");
            gv.Columns.Add(colJobGroup);

            var colJobName = generateTextColumn(ColNameJobKey, "Job");
            gv.Columns.Add(colJobName);

            var colJobDescription = generateTextColumn(ColNameJobDescription, "Description");
            gv.Columns.Add(colJobDescription);

            var colStatus = generateTextColumn(ColNameJobStatus, "Status");
            colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gv.Columns.Add(colStatus);

            var colRunTime = generateTextColumn(ColNameJobRunTime, "Run Time");
            colRunTime.Width = 80;
            gv.Columns.Add(colRunTime);

            var colPreviousFiredTime = generateTextColumn(ColNameJobPreviousFiredTime, "Prev Fired Time");
            colPreviousFiredTime.Width = 120;
            gv.Columns.Add(colPreviousFiredTime);

            var colNextFireTime = generateTextColumn(ColNameJobNextFireTime, "Next Fire Time");
            colNextFireTime.Width = 120;
            gv.Columns.Add(colNextFireTime);
        }

        private void InitializeScanJobStatisticTask()
        {
            var token = this.FormClosingSignal.Token;

            //隨 Process 生死不特別處理
            var task = Task.Factory.StartNew(() =>
            {
                var logger = LogFactory.Create("ScanJobStatisticTask");
                logger.Information("{ModuleId} init");

                var scheduler = this.Scheduler;

                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        logger.Information("{ModuleId} Cancel");
                        return;
                    }

                    var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result;
                    var totalJobs = jobKeys.Count;

                    var jobs = scheduler.GetCurrentlyExecutingJobs().Result;
                    var totalRunningJobs = jobs.Count;

                    tslJobStatistic.Text = $"running: {totalRunningJobs} / {totalJobs}";

                    Thread.Sleep(200);
                }
            }, token);
            this.Tasks.Add(task);
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
            var job3 = JobBuilder.Create<Job1>()
                .WithIdentity("Job3")
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

            var every2min = TriggerBuilder
                .Create()
                .WithCronSchedule("0 */2 * ? * * *")
                .Build();

            scheduler.ScheduleJob(job1, every10s);
            scheduler.ScheduleJob(job2, every20s);
            scheduler.ScheduleJob(job2, every30s);
            scheduler.ScheduleJob(job3, every2min);

            this.Scheduler = scheduler;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Scheduler.Start();

            RefreshToggleSchedulerMenuItem();
        }

        private void RefreshToggleSchedulerMenuItem()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(RefreshToggleSchedulerMenuItem));
                return;
            }

            var scheduler = this.Scheduler;
            if (scheduler.InStandbyMode)
                tsmToggleScheduler.Text = "Start";
            else
                tsmToggleScheduler.Text = "Stop";

            tsmToggleScheduler.Enabled = true;
        }

        private void tsmToggleScheduler_Click(object sender, EventArgs e)
        {
            tsmToggleScheduler.Enabled = false;

            var scheduler = this.Scheduler;
            if (scheduler.InStandbyMode)
                tsmToggleScheduler.Text = "Starting...";
            else
                tsmToggleScheduler.Text = "Stopping...";

            Task.Run(() =>
            {
                var logger = LogFactory.Create("ToggleScheduler");
                if (scheduler.InStandbyMode)
                {
                    scheduler.Start();
                    logger.Information("Scheduler Start");
                    Thread.Sleep(1000);
                }
                else
                {
                    logger.Information("Scheduler Stopping...");
                    //停止 trigger
                    scheduler.Standby().Wait();
                    //等工作結束
                    while (scheduler.GetCurrentlyExecutingJobs().Result.Count > 0)
                        Thread.Sleep(100);
                    logger.Information("Scheduler Stopped");
                }

                RefreshToggleSchedulerMenuItem();
            });
        }

        private void dgvJobs_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var scheduler = this.Scheduler;

            if (e.Button != MouseButtons.Right) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var gv = (DataGridView)sender;
            var grow = gv.Rows[e.RowIndex];
            var job = (IJobDetail)grow.Tag;

            cmuItemResumeJob.Visible = false;
            cmuItemPauseJob.Visible = false;

            if (IsJobPaused(scheduler, job))
            {
                cmuItemResumeJob.Tag = job;
                cmuItemResumeJob.Visible = true;
                cmuJobs.Show(Cursor.Position);
            }
            else
            {
                cmuItemPauseJob.Tag = job;
                cmuItemPauseJob.Visible = true;
                cmuJobs.Show(Cursor.Position);
            }
        }

        private void cmuItemResumeJob_Click(object sender, EventArgs e)
        {
            var scheduler = this.Scheduler;

            var cmuItem = (ToolStripMenuItem)sender;
            var job = (IJobDetail)cmuItem.Tag;

            scheduler.ResumeJob(job.Key);
        }
        private void cmuItemPauseJob_Click(object sender, EventArgs e)
        {
            var scheduler = this.Scheduler;

            var cmuItem = (ToolStripMenuItem)sender;
            var job = (IJobDetail)cmuItem.Tag;

            scheduler.PauseJob(job.Key);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var logger = LogFactory.Create<Form1>();
            logger.Information("{ModuleId} Closing Send Cancel");
            this.FormClosingSignal.Cancel();
            this.Tasks.ForEach(t => t.Wait());
            logger.Information("{ModuleId} Canceled");
            this.FormClosingSignal.Dispose();
        }
    }
}
