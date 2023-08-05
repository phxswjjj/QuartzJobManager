using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzJobManager.Utility
{
    internal class LogFactory
    {
        public static ILogger Instance { get; internal set; }

        public static ILogger Create<T>()
        {
            var logger = Instance
                .ForContext("ModuleId", typeof(T));
            return logger;
        }
    }
}
