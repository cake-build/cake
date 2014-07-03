using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common
{
    public static class AssemblyInfoExtensions
    {
        [CakeMethodAlias]
        public static void CreateAssemblyInfo(this ICakeContext context, FilePath outputPath, AssemblyInfoSettings settings)
        {
            var creator = new AssemblyInfoCreator(context.FileSystem, context.Environment, context.Log);
            creator.Create(outputPath, settings);
        }
    }
}
