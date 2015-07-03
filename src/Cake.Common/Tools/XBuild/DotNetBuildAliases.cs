using System;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// Contains functionality to run either MSBuild on Windows or XBuild on Mac/Linux/Unix.
    /// </summary>
    [CakeAliasCategory("DotNetBuild")]
    public static class DotNetBuildAliases
    {
        /// <summary>
        /// Builds the specified solution using MSBuild or XBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        [CakeMethodAlias]
        public static void DotNetBuild(this ICakeContext context, FilePath solution)
        {
            DotNetBuild(context, solution, settings => { });
        }

        /// <summary>
        /// Builds the specified solution using MSBuild or XBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <param name="configurator">The configurator.</param>
        [CakeMethodAlias]
        public static void DotNetBuild(this ICakeContext context, FilePath solution, Action<DotNetBuildSettings> configurator)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (configurator == null)
            {
                throw new ArgumentNullException("configurator");
            }

            var dotNetSettings = new DotNetBuildSettings(solution);
            configurator(dotNetSettings);

            // On Mac/Linux/Unix run XBuild, on windows run MSBuild
            if (context.Environment.IsUnix()) 
            {                
                var xbuildSettings = new XBuildSettings(solution) 
                {
                    Configuration = dotNetSettings.Configuration,
                    Verbosity = dotNetSettings.Verbosity
                };

                xbuildSettings.Targets.Clear();
                foreach (var t in dotNetSettings.Targets)
                {
                    xbuildSettings.Targets.Add(t);
                }

                xbuildSettings.Properties.Clear();
                foreach (var kvp in dotNetSettings.Properties)
                {
                    xbuildSettings.Properties.Add(kvp);
                }
                
                var runner = new XBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner);
                runner.Run(xbuildSettings);
            } 
            else 
            {
                var msbuildSettings = new MSBuildSettings(solution) 
                {
                    Configuration = dotNetSettings.Configuration,
                    Verbosity = dotNetSettings.Verbosity
                };

                msbuildSettings.Targets.Clear();
                foreach (var t in dotNetSettings.Targets)
                {
                    msbuildSettings.Targets.Add(t);
                }

                msbuildSettings.Properties.Clear();
                foreach (var kvp in dotNetSettings.Properties)
                {
                    msbuildSettings.Properties.Add(kvp);
                }
                
                var runner = new MSBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner);
                runner.Run(msbuildSettings);
            }
        }
    }
}
