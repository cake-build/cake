using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace Cake.Common.Diagnostics
{
    public static class LoggingExtensions
    {
        [CakeScriptMethod]
        public static void Error(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Error(format, args);
        }

        [CakeScriptMethod]
        public static void Warning(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Warning(format, args);
        }

        [CakeScriptMethod]
        public static void Information(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Information(format, args);
        }

        [CakeScriptMethod]
        public static void Verbose(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Verbose(format, args);
        }

        [CakeScriptMethod]
        public static void Debug(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Debug(format, args);
        }
    }
}
