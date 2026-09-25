// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Core;

/// <summary>
/// Represents a context for scripts and script aliases.
/// </summary>
public interface ICakeContext
{
    /// <summary>
    /// Gets the file system.
    /// </summary>
    /// <value>The file system.</value>
    /// <example>
    /// <code>
    /// Task("Check-Publish")
    ///     .Does(context =>
    /// {
    ///     if (context.FileSystem.Exist("./publish.enabled"))
    ///     {
    ///         Information("Publish is enabled");
    ///     }
    /// });
    /// </code>
    /// </example>
    IFileSystem FileSystem { get; }

    /// <summary>
    /// Gets the environment.
    /// </summary>
    /// <value>The environment.</value>
    /// <example>
    /// <code>
    /// Task("Env")
    ///     .Does(context =>
    /// {
    ///     Information("Working directory: {0}", context.Environment.WorkingDirectory);
    ///     Information("HOME: {0}", context.Environment.GetEnvironmentVariable("HOME") ?? "unknown");
    /// });
    /// </code>
    /// </example>
    ICakeEnvironment Environment { get; }

    /// <summary>
    /// Gets the globber.
    /// </summary>
    /// <value>The globber.</value>
    /// <example>
    /// <code>
    /// Task("List-Sources")
    ///     .Does(context =>
    /// {
    ///     foreach (var file in context.Globber.GetFiles("**/*.cs"))
    ///     {
    ///         Information(file);
    ///     }
    /// });
    /// </code>
    /// </example>
    IGlobber Globber { get; }

    /// <summary>
    /// Gets the log.
    /// </summary>
    /// <value>The log.</value>
    /// <example>
    /// <code>
    /// Task("Hello")
    ///     .Does(context =>
    /// {
    ///     context.Log.Information("Hello");
    /// });
    /// </code>
    /// </example>
    ICakeLog Log { get; }

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    /// <value>The arguments.</value>
    /// <example>
    /// <code>
    /// Task("Args")
    ///     .Does(context =>
    /// {
    ///     if (context.Arguments.HasArgument("target"))
    ///     {
    ///         Information("Target: {0}", context.Arguments.GetArgument("target"));
    ///     }
    /// });
    /// </code>
    /// </example>
    ICakeArguments Arguments { get; }

    /// <summary>
    /// Gets the process runner.
    /// </summary>
    /// <value>The process runner.</value>
    /// <example>
    /// <code>
    /// Task("Git-Status")
    ///     .Does(context =>
    /// {
    ///     using (var process = context.ProcessRunner.Start("git", new ProcessSettings { Arguments = "status" }))
    ///     {
    ///         process.WaitForExit();
    ///     }
    /// });
    /// </code>
    /// </example>
    IProcessRunner ProcessRunner { get; }

    /// <summary>
    /// Gets the registry.
    /// </summary>
    /// <value>The registry.</value>
    /// <example>
    /// <code>
    /// Task("Framework-Release")
    ///     .Does(context =>
    /// {
    ///     using (var key = context.Registry.LocalMachine.OpenKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"))
    ///     {
    ///         Information("Release: {0}", key?.GetValue("Release"));
    ///     }
    /// });
    /// </code>
    /// </example>
    IRegistry Registry { get; }

    /// <summary>
    /// Gets the tool locator.
    /// </summary>
    /// <value>The tool locator.</value>
    /// <example>
    /// <code>
    /// Task("Resolve-Git")
    ///     .Does(context =>
    /// {
    ///     context.Tools.RegisterFile("./tools/git.exe");
    ///     var git = context.Tools.Resolve("git");
    ///     Information("Git: {0}", git);
    /// });
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// Task("Resolve-XUnit")
    ///     .Does(context =>
    /// {
    ///     InstallTool("nuget:?package=xunit.runner.console&amp;version=2.9.3");
    ///     var xunit = context.Tools.Resolve("xunit.console.exe");
    ///     Information("xUnit: {0}", xunit);
    /// });
    /// </code>
    /// </example>
    IToolLocator Tools { get; }

    /// <summary>
    /// Gets the data context resolver.
    /// </summary>
    /// <example>
    /// <code>
    /// Setup&lt;Foo&gt;(context => new Foo { Place = "World" });
    ///
    /// Task("Hello")
    ///     .Does(context =>
    /// {
    ///     var data = context.Data.Get&lt;Foo&gt;();
    ///     Information("Hello {0}", data.Place);
    /// });
    /// </code>
    /// </example>
    ICakeDataResolver Data { get; }

    /// <summary>
    /// Gets the cake configuration.
    /// </summary>
    /// <example>
    /// <code>
    /// Task("Config")
    ///     .Does(context =>
    /// {
    ///     Information("Tools path: {0}", context.Configuration.GetValue("Paths_Tools"));
    /// });
    /// </code>
    /// </example>
    ICakeConfiguration Configuration { get; }

    /// <summary>
    /// Gets the tool installer.
    /// </summary>
    /// <value>The tool installer.</value>
    /// <example>
    /// <code>
    /// Task("Load-JMeter")
    ///     .Does(context =>
    /// {
    ///     context.ToolInstaller.Install(new PackageReference("nuget:?package=JMeter&amp;version=5.6.3"));
    /// });
    /// </code>
    /// </example>
    IToolInstaller ToolInstaller =>
        throw new CakeException("The current ICakeContext does not provide a tool installer.");

    /// <summary>
    /// Gets the service provider.
    /// </summary>
    /// <value>The service provider.</value>
    /// <example>
    /// <code>
    /// Task("MyTask")
    ///     .Does(context =>
    /// {
    ///     var log = context.ServiceProvider.GetRequiredService&lt;ICakeLog&gt;();
    ///     log.Information("Hello from IoC");
    /// });
    /// </code>
    /// </example>
    IServiceProvider ServiceProvider =>
        throw new CakeException("The current ICakeContext does not provide a service provider.");
}
