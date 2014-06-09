using Cake.Core;
using Cake.Core.IO;

namespace Cake.NuGet
{
    public static class NuGetExtensions
    {
        public static void NuGetPack(this ICakeContext context, FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var packer = new NuGetPacker(context.FileSystem, context.Environment, context.Globber, new ProcessRunner());
            packer.Pack(nuspecFilePath, settings);
        }
    }
}
