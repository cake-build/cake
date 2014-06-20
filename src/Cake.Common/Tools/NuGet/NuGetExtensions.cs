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
    }
}
