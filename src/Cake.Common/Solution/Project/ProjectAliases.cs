using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Contains functionality related to MSBuild project files
    /// </summary>
    [CakeAliasCategory("MSBuild Resource")]
    public static class ProjectAliases
    {
        /// <summary>
        /// Parses project information from project file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        public static ProjectParserResult ParseProject(this ICakeContext context, FilePath projectPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var parser = new ProjectParser(context.FileSystem, context.Environment);
            return parser.Parse(projectPath);
        }
    }
}
