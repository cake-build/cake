using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to assembly info.
    /// </summary>
    public static class AssemblyInfoExtensions
    {
        /// <summary>
        /// Creates an assembly information file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void CreateAssemblyInfo(this ICakeContext context, FilePath outputPath, AssemblyInfoSettings settings)
        {
            var creator = new AssemblyInfoCreator(context.FileSystem, context.Environment, context.Log);
            creator.Create(outputPath, settings);
        }
    }
}
