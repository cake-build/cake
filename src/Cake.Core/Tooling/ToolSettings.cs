// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Base class for tool settings.
    /// </summary>
    public class ToolSettings
    {
        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets optional timeout for tool execution.
        /// </summary>
        public TimeSpan? ToolTimeout { get; set; }

        /// <summary>
        /// Gets or sets the argument customization.
        /// Argument customization is a way that lets you add, replace or reuse arguments passed to a tool.
        /// This allows you to support new tool arguments, customize arguments or address potential argument issues.
        /// </summary>
        /// <example>
        /// <code>
        /// NuGetAddSource("Cake", "https://www.myget.org/F/cake/api/v3/index.json",
        ///     new NuGetSourcesSettings { UserName = "user", Password = "incorrect",
        ///     ArgumentCustomization = args=&gt;args.Append("-StorePasswordInClearText")
        /// });
        /// </code>
        /// </example>
        /// <value>The delegate used to customize the <see cref="Cake.Core.IO.ProcessArgumentBuilder" />.</value>
        public Func<ProcessArgumentBuilder, ProcessArgumentBuilder> ArgumentCustomization { get; set; }
    }
}
