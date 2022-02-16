// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        /// Gets or sets the working directory for the tool process.
        /// </summary>
        /// <value>The working directory for the tool process.</value>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to opt out of using
        /// an explicit working directory for the process.
        /// </summary>
        public bool NoWorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the argument customization.
        /// Argument customization is a way that lets you add, replace or reuse arguments passed to a tool.
        /// This allows you to support new tool arguments, customize arguments or address potential argument issues.
        /// </summary>
        /// <example>
        /// <para>Combining tool specific settings and tool arguments:</para>
        /// <code>
        /// NuGetAddSource("Cake", "https://www.myget.org/F/cake/api/v3/index.json",
        ///     new NuGetSourcesSettings { UserName = "user", Password = "incorrect",
        ///     ArgumentCustomization = args=&gt;args.Append("-StorePasswordInClearText")
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// <para>Setting multiple tool arguments:</para>
        /// <code>
        /// MSTest(pathPattern, new MSTestSettings()
        ///     { ArgumentCustomization = args=&gt;args.Append("/detail:errormessage")
        ///                                            .Append("/resultsfile:TestResults.trx") });
        /// </code>
        /// </example>
        /// <value>The delegate used to customize the <see cref="Cake.Core.IO.ProcessArgumentBuilder" />.</value>
        public Func<ProcessArgumentBuilder, ProcessArgumentBuilder> ArgumentCustomization { get; set; }

        /// <summary>
        /// Gets or sets search paths for files, directories for temporary files, application-specific options, and other similar information.
        /// </summary>
        /// <example>
        /// <code>
        /// MSBuild("./src/Cake.sln", new MSBuildSettings {
        ///     EnvironmentVariables = new Dictionary&lt;string, string&gt;{
        ///         { "TOOLSPATH", MakeAbsolute(Directory("./tools")).FullPath }
        ///     }});
        /// </code>
        /// </example>
        public IDictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets or sets whether the exit code from the tool process causes an exception to be thrown.
        /// <para>
        /// If the delegate is null (the default) or returns false, then an exception is thrown upon a non-zero exit code.
        /// </para>
        /// <para>
        /// If the delegate returns true then no exception is thrown.
        /// </para>
        /// <para>
        /// This can be useful when the exit code should be ignored, or if there is a desire to apply logic that is conditional
        /// on the exit code value.
        /// </para>
        /// </summary>
        /// <example>
        /// Don't throw exceptions if DotNetCoreTest returns non-zero:
        /// <code>
        /// DotNetCoreTest("MyProject.csproj", new DotNetCoreTestSettings {
        ///     HandleExitCode = _=&gt; true
        /// });
        /// </code>
        /// </example>
        /// <example>
        /// Use custom logic for exit code:
        /// <code>
        /// DotNetCoreTest("MyProject.csproj", new DotNetCoreTestSettings {
        ///     HandleExitCode = exitCode =&gt; exitCode switch {
        ///         0 =&gt; throw new CakeException("ZERO"),
        ///         1 =&gt; true, // treat 1 and 2 as handled "ok".
        ///         2 =&gt; true,
        ///         _ =&gt; false // everything else will throw via default implementation
        ///     };
        /// });
        /// </code>
        /// </example>
        public Func<int, bool> HandleExitCode { get; set; }
    }
}