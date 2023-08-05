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

        public static ILogger Create<T>() => Create(typeof(T).ToString());
        public static ILogger Create(string moduleId)
        {
            var logger = Instance
                .ForContext("ModuleId", moduleId);
            return logger;
        }
    }
}
