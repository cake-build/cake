using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Diagnostics
{
    /// <summary>
    ///     Contains the aliases related to the logging pipeline.
    /// </summary>
    [CakeAliasCategory("Logging")]
    public static class PipelineAliases
    {
        /// <summary>
        ///     Adds a <see cref="ICakeLog" /> based logger to the logging pipeline
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext" /> context</param>
        /// <param name="log">The logger to add</param>
        public static void AddLog(this ICakeContext context, ICakeLog log)
        {
            context.LogPipeline.AddLog(log);
        }

        /// <summary>
        ///     Removes a <see cref="ICakeLog" /> based logger from the logging pipeline
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext" /> context</param>
        /// <param name="log">The logger to remove</param>
        public static void RemoveLog(this ICakeContext context, ICakeLog log)
        {
            context.LogPipeline.RemoveLog(log);
        }

        /// <summary>
        ///     Adds a <see cref="ICakeLog" /> logger to the logging pipeline to send the specified log levels to a file
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext" /> context</param>
        /// <param name="file">The <see cref="FilePath" /> that specifies the file to which to log</param>
        /// <param name="levels">The <see cref="LogLevel" /> for which to send log output to the file</param>
        /// <returns>The added logger, to remove it from the pipeline call <see cref="RemoveLog" /></returns>
        /// <remarks>Creates a logger that will log with the <see cref="Verbosity"/> that is currently being used.  </remarks>
        public static ICakeLog LogToFile(this ICakeContext context, FilePath file, params LogLevel[] levels)
        {
            ICakeLog ret = new FileLog(file, levels) { Verbosity = context.Log.Verbosity };

            context.AddLog(ret);

            return ret;
        }

        /// <summary>
        ///     Adds a <see cref="FullLogActionEntry" /> delegate that will be called when logging is called
        /// </summary>
        /// <param name="context">The <see cref="ICakeContext" /> context</param>
        /// <param name="action">The delegate to call on log entry</param>
        /// <returns>The added logger, to remove it from the pipeline call <see cref="RemoveLog" /></returns>
        public static ICakeLog LogToAction(this ICakeContext context, FullLogActionEntry action)
        {
            ICakeLog ret = new ActionLog(action) { Verbosity = context.Log.Verbosity };

            context.AddLog(ret);

            return ret;
        }
    }
}