// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.TextTransform
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/mono/monodevelop/tree/master/main/src/addins/TextTemplating/TextTransform">TextTransform</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="TextTransformSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=Mono.TextTransform"
    /// </code>
    /// </para>
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
