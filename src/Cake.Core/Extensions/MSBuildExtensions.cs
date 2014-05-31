using System;
using Cake.Core.IO;
using Cake.Core.MSBuild;

namespace Cake.Core.Extensions
{
    public static class MSBuildExtensions
    {
        public static void MSBuild(this ICakeContext context, FilePath solution)
        {
            MSBuild(context, solution, settings => { });
        }

        public static void MSBuild(this ICakeContext context, FilePath solution, Action<MSBuildSettings> configurator)
        {
            var settings = new MSBuildSettings(solution);
            configurator(settings);
            new MSBuildRunner().Run(context, settings);
        }
    }
}
