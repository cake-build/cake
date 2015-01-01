using System;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core.Text;

namespace Cake.Common.Text
{
    /// <summary>
    /// Contains functionality related to text transformation.
    /// </summary>
    [CakeAliasCategory("Text")]
    public static class TextTransformationAliases
    {
        /// <summary>
        /// Creates a text transformation from the provided template.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="template">The template.</param>
        /// <returns>A <see cref="TextTransformation{TTemplate}"/> representing the provided template.</returns>
        [CakeMethodAlias]
        public static TextTransformation<TextTransformationTemplate> TransformText(this ICakeContext context, string template)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new TextTransformation<TextTransformationTemplate>(
                context.FileSystem, context.Environment,
                new TextTransformationTemplate(template));
        }

        /// <summary>
        /// Creates a text transformation from the provided template on disc.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The template file path.</param>
        /// <returns>A <see cref="TextTransformation{TTemplate}"/> representing the provided template.</returns>
        [CakeMethodAlias]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Stream reader leaves stream open.")]
        public static TextTransformation<TextTransformationTemplate> TransformTextFile(this ICakeContext context, FilePath path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Make the path absolute if necessary.
            path = path.IsRelative ? path.MakeAbsolute(context.Environment) : path;

            // Read the content of the file.
            var file = context.FileSystem.GetFile(path);
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                return new TextTransformation<TextTransformationTemplate>(
                    context.FileSystem, context.Environment,
                    new TextTransformationTemplate(reader.ReadToEnd()));
            }
        }
    }
}
