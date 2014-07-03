using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild
{
    public static class MSBuildExtensions
    {
        [CakeMethodAlias]
        public static void MSBuild(this ICakeContext context, FilePath solution)
        {
            MSBuild(context, solution, settings => { });
        }

        [CakeMethodAlias]
        public static void MSBuild(this ICakeContext context, FilePath solution, Action<MSBuildSettings> configurator)
        {
            var settings = new MSBuildSettings(solution);
            configurator(settings);

            var runner = new MSBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner);
            runner.Run(settings);
        }
    }
}
