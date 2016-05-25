using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.TextTransform
{
    /// <summary>
    /// Contains functionality related to transforming templates
    /// </summary>
    [CakeAliasCategory("Text")]
    public static class TextTransformAliases
    {
        /// <summary>
        /// Transform a text template.
        /// </summary>
        /// <example>
        /// <code>
        /// // Transform a .tt template.
        /// var transform = File("./src/Cake/Transform.tt");
        /// TransformTemplate(transform);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="sourceFile">The source file.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("T4 Text Templating")]
        public static void TransformTemplate(this ICakeContext context, FilePath sourceFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new TextTransformRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(sourceFile, new TextTransformSettings());
        }

        /// <summary>
        /// Transform a text template.
        /// </summary>
        /// <example>
        /// <code>
        /// // Transform a .tt template.
        /// var transform = File("./src/Cake/Transform.tt");
        /// TransformTemplate(transform, new TextTransformSettings { OutputFile="./src/Cake/Transform.cs" });
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("T4 Text Templating")]
        public static void TransformTemplate(this ICakeContext context, FilePath sourceFile, TextTransformSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new TextTransformRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(sourceFile, settings ?? new TextTransformSettings());
        }
    }
}