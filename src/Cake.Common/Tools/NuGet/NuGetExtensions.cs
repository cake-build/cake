using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public static class NuGetExtensions
    {
        [CakeScriptMethod]
        public static void NuGetPack(this ICakeContext context, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var packer = new NuGetPacker(context.Environment, context.Globber, new ProcessRunner());
            packer.Pack(nuspecFilePath, settings);
        }


        [CakeScriptMethod]
        public static void NuGetRestore(this ICakeContext context, FilePath solution)
        {
            context.NuGetRestore(solution, settings => { });
        }

        [CakeScriptMethod]
        public static void NuGetRestore(this ICakeContext context, FilePath solution, Action<NugetRestoreSettings> configurator)
        {
            var settings = new NugetRestoreSettings(solution);
            configurator(settings);
            
            var runner = new NuGetRestorer(context.Environment, context.Globber, new ProcessRunner());
            runner.Restore(settings);
        }
    }
}
