namespace QuartzJobManager
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslJobStatistic = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmToggleScheduler = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvJobs = new System.Windows.Forms.DataGridView();
            this.cmuJobs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmuItemResumeJob = new System.Windows.Forms.ToolStripMenuItem();
            this.cmuItemPauseJob = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCloseApp = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).BeginInit();
            this.cmuJobs.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslJobStatistic});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslJobStatistic
            // 
            this.tslJobStatistic.Name = "tslJobStatistic";
            this.tslJobStatistic.Size = new System.Drawing.Size(128, 17);
            this.tslJobStatistic.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmToggleScheduler,
            this.tsmCloseApp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmToggleScheduler
            // 
            this.tsmToggleScheduler.Name = "tsmToggleScheduler";
            this.tsmToggleScheduler.Size = new System.Drawing.Size(45, 20);
            this.tsmToggleScheduler.Text = "Start";
            this.tsmToggleScheduler.Click += new System.EventHandler(this.tsmToggleScheduler_Click);
            // 
            // dgvJobs
            // 
            this.dgvJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJobs.Location = new System.Drawing.Point(12, 27);
            this.dgvJobs.Name = "dgvJobs";
            this.dgvJobs.RowTemplate.Height = 24;
            this.dgvJobs.Size = new System.Drawing.Size(776, 398);
            this.dgvJobs.TabIndex = 2;
            this.dgvJobs.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvJobs_CellMouseClick);
            // 
            // cmuJobs
            // 
            this.cmuJobs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmuItemResumeJob,
            this.cmuItemPauseJob});
            this.cmuJobs.Name = "cmuJobs";
            this.cmuJobs.Size = new System.Drawing.Size(120, 48);
            // 
            // cmuItemResumeJob
            // 
            this.cmuItemResumeJob.Name = "cmuItemResumeJob";
            this.cmuItemResumeJob.Size = new System.Drawing.Size(119, 22);
            this.cmuItemResumeJob.Text = "Resume";
            this.cmuItemResumeJob.Click += new System.EventHandler(this.cmuItemResumeJob_Click);
            // 
            // cmuItemPauseJob
            // 
            this.cmuItemPauseJob.Name = "cmuItemPauseJob";
            this.cmuItemPauseJob.Size = new System.Drawing.Size(119, 22);
            this.cmuItemPauseJob.Text = "Pause";
            this.cmuItemPauseJob.Click += new System.EventHandler(this.cmuItemPauseJob_Click);
            // 
            // tsmCloseApp
            // 
            this.tsmCloseApp.Name = "tsmCloseApp";
            this.tsmCloseApp.Size = new System.Drawing.Size(50, 20);
            this.tsmCloseApp.Text = "Close";
            this.tsmCloseApp.Click += new System.EventHandler(this.tsmCloseApp_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvJobs);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJobs)).EndInit();
            this.cmuJobs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslJobStatistic;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmToggleScheduler;
        private System.Windows.Forms.DataGridView dgvJobs;
        private System.Windows.Forms.ContextMenuStrip cmuJobs;
        private System.Windows.Forms.ToolStripMenuItem cmuItemResumeJob;
        private System.Windows.Forms.ToolStripMenuItem cmuItemPauseJob;
        private System.Windows.Forms.ToolStripMenuItem tsmCloseApp;
    }
}

