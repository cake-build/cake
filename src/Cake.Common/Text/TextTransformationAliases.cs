// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
        /// <example>
        /// This sample shows how to create a <see cref="TextTransformation{TTemplate}"/> using
        /// the specified template.
        /// <code>
        /// string text = TransformText("Hello &lt;%subject%&gt;!")
        ///    .WithToken("subject", "world")
        ///    .ToString();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static TextTransformation<TextTransformationTemplate> TransformText(this ICakeContext context, string template)
        {
            return TransformText(context, template, "<%", "%>");
        }

        /// <summary>
        /// Creates a text transformation from the provided template, using the specified placeholder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="template">The template.</param>
        /// <param name="leftPlaceholder">The left placeholder.</param>
        /// <param name="rightPlaceholder">The right placeholder.</param>
        /// <returns>A <see cref="TextTransformation{TTemplate}"/> representing the provided template.</returns>
        /// <example>
        /// This sample shows how to create a <see cref="TextTransformation{TTemplate}"/> using
        /// the specified template and placeholder.
        /// <code>
        /// string text = TransformText("Hello {subject}!", "{", "}")
        ///    .WithToken("subject", "world")
        ///    .ToString();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static TextTransformation<TextTransformationTemplate> TransformText(
            this ICakeContext context,
            string template,
            string leftPlaceholder,
            string rightPlaceholder)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (leftPlaceholder == null)
            {
                throw new ArgumentNullException("leftPlaceholder");
            }
            if (rightPlaceholder == null)
            {
                throw new ArgumentNullException("rightPlaceholder");
            }

            // Create the placeholder.
            var placeholder = new Tuple<string, string>(leftPlaceholder, rightPlaceholder);

            // Create and return the text transformation.
            return new TextTransformation<TextTransformationTemplate>(
                context.FileSystem, context.Environment,
                new TextTransformationTemplate(template, placeholder));
        }

        /// <summary>
        /// Creates a text transformation from the provided template on disc.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The template file path.</param>
        /// <returns>A <see cref="TextTransformation{TTemplate}"/> representing the provided template.</returns>
        /// <example>
        /// This sample shows how to create a <see cref="TextTransformation{TTemplate}"/> using
        /// the specified template file with the placeholder format <c>&lt;%key%&gt;</c>.
        /// <code>
        /// string text = TransformTextFile("./template.txt")
        ///    .WithToken("subject", "world")
        ///    .ToString();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Stream reader leaves stream open.")]
        public static TextTransformation<TextTransformationTemplate> TransformTextFile(this ICakeContext context, FilePath path)
        {
            return TransformTextFile(context, path, "<%", "%>");
        }

        /// <summary>
        /// Creates a text transformation from the provided template on disc, using the specified placeholder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The template file path.</param>
        /// <param name="leftPlaceholder">The left placeholder.</param>
        /// <param name="rightPlaceholder">The right placeholder.</param>
        /// <returns>A <see cref="TextTransformation{TTemplate}"/> representing the provided template.</returns>
        /// <example>
        /// This sample shows how to create a <see cref="TextTransformation{TTemplate}"/> using
        /// the specified template file and placeholder.
        /// <code>
        /// string text = TransformTextFile("./template.txt", "{", "}")
        ///    .WithToken("subject", "world")
        ///    .ToString();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Stream reader leaves stream open.")]
        public static TextTransformation<TextTransformationTemplate> TransformTextFile(
            this ICakeContext context,
            FilePath path,
            string leftPlaceholder,
            string rightPlaceholder)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (leftPlaceholder == null)
            {
                throw new ArgumentNullException("leftPlaceholder");
            }
            if (rightPlaceholder == null)
            {
                throw new ArgumentNullException("rightPlaceholder");
            }

            // Make the path absolute if necessary.
            path = path.IsRelative ? path.MakeAbsolute(context.Environment) : path;

            // Create the placeholder.
            var placeholder = new Tuple<string, string>(leftPlaceholder, rightPlaceholder);

            // Read the content of the file.
            var file = context.FileSystem.GetFile(path);
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                // Create and return the text transformation.
                return new TextTransformation<TextTransformationTemplate>(
                    context.FileSystem, context.Environment,
                    new TextTransformationTemplate(reader.ReadToEnd(), placeholder));
            }
        }
    }
}
