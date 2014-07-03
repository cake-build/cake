using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace Cake.Common.Diagnostics
{
    public static class LoggingExtensions
    {
        [CakeMethodAlias]
        public static void Error(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Error(format, args);
        }

        [CakeMethodAlias]
        public static void Warning(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Warning(format, args);
        }

        [CakeMethodAlias]
        public static void Information(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Information(format, args);
        }

        [CakeMethodAlias]
        public static void Verbose(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Verbose(format, args);
        }

        [CakeMethodAlias]
        public static void Debug(this ICakeContext context, string format, params object[] args)
        {
            context.Log.Debug(format, args);
        }
    }
}
