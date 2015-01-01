using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Solution
{
    /// <summary>
    /// Contains functionality related to MSBuild solution files
    /// </summary>
    [CakeAliasCategory("MSBuild Resource")]
    public static class SolutionAliases
    {
        /// <summary>
        /// Parses project information from solution file
        /// </summary>
        /// <param name="context"></param>
        /// <param name="solutionPath"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        public static SolutionParserResult ParseSolution(this ICakeContext context, FilePath solutionPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var parser = new SolutionParser(context.FileSystem, context.Environment);
            return parser.Parse(solutionPath);
        }
    }
}
