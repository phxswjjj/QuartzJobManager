using QuartzJobManager.Utility;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuartzJobManager
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            InitializeLog();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void InitializeLog()
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            logger.Information("log init");

            LogFactory.Instance = logger; ;
        }
    }
}
