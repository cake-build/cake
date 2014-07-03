using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.WiX
{
    public static class WiXExtensions
    {
        [CakeMethodAlias]
        public static void WiXCandle(this ICakeContext context, string pattern, CandleSettings settings = null)
        {
            var files = context.Globber.GetFiles(pattern);
            WiXCandle(context, files, settings ?? new CandleSettings());
        }

        [CakeMethodAlias]
        public static void WiXCandle(this ICakeContext context, IEnumerable<FilePath> sourceFiles, CandleSettings settings = null)
        {
            var runner = new CandleRunner(context.Environment, context.Globber, context.ProcessRunner);
            runner.Run(sourceFiles, settings ?? new CandleSettings());
        }
    }
}
