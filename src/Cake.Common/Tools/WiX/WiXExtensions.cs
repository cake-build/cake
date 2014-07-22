using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    /// <summary>
    /// Contains functionality related to running WiX tools.
    /// </summary>
    public static class WiXExtensions
    {
        /// <summary>
        /// Compiles all .wxs sources matching the <paramref name="pattern"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The globbing pattern.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void WiXCandle(this ICakeContext context, string pattern, CandleSettings settings = null)
        {
            var files = context.Globber.GetFiles(pattern);
            WiXCandle(context, files, settings ?? new CandleSettings());
        }

        /// <summary>
        /// Compiles all .wxs sources in <paramref name="sourceFiles"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceFiles">The source files.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void WiXCandle(this ICakeContext context, IEnumerable<FilePath> sourceFiles, CandleSettings settings = null)
        {
            var runner = new CandleRunner(context.Environment, context.Globber, context.ProcessRunner);
            runner.Run(sourceFiles, settings ?? new CandleSettings());
        }
    }
}
