// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.Command
{
    /// <summary>
    /// <para>Contains generic functionality for simplifying the execution tools with no dedicated alias available yet.</para>
    /// </summary>
    [CakeAliasCategory("Command")]
    public static class CommandAliases
    {
        /// <summary>
        /// Executes a generic tool/process based on arguments and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="toolExecutableNames">The tool executable names.</param>
        /// <param name="arguments">The optional arguments.</param>
        /// <param name="expectedExitCode">The expected exit code (default 0).</param>
        /// <param name="settingsCustomization">The optional settings customization (default null).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="toolExecutableNames"/> is null or empty.</exception>
        /// <example>
        /// <code>
        /// // Example with ProcessArgumentBuilder
        /// #tool dotnet:?package=DPI&amp;version=2022.8.21.54
        /// Command(
        ///     new []{ "dpi", "dpi.exe"},
        ///     new ProcessArgumentBuilder()
        ///         .Append("nuget")
        ///         .AppendQuoted(Context.Environment.WorkingDirectory.FullPath)
        ///         .AppendSwitch("--output", " ", "TABLE")
        ///         .Append("analyze")
        /// );
        ///
        ///
        /// // Example with implicit ProcessArgumentBuilder
        /// Command(
        ///     new []{ "dotnet", "dotnet.exe"},
        ///     "--version"
        /// );
        ///
        ///
        /// // Example specify expected exit code
        /// Command(
        ///     new []{ "dotnet", "dotnet.exe"},
        ///     expectedExitCode: -2147450751
        /// );
        ///
        ///
        /// // Example settings customization
        /// Command(
        ///     new []{ "dotnet", "dotnet.exe"},
        ///     settingsCustomization: settings => settings
        ///                                             .WithToolName(".NET tool")
        ///                                             .WithExpectedExitCode(1)
        ///                                             .WithArgumentCustomization(args => args.Append("tool"))
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        public static void Command(
            this ICakeContext context,
            ICollection<string> toolExecutableNames,
            ProcessArgumentBuilder arguments = null,
            int expectedExitCode = 0,
            Func<CommandSettings, CommandSettings> settingsCustomization = null)
            => context.Command(
                GetSettings(toolExecutableNames, expectedExitCode, settingsCustomization),
                arguments);

        /// <summary>
        /// Executes a generic command based on arguments and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The optional arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="settings"/> is null.</exception>
        /// <example>
        /// <code>
        /// #tool dotnet:?package=DPI&amp;version=2022.8.21.54
        /// // Reusable tools settings i.e. created in setup.
        /// var settings = new CommandSettings {
        ///         ToolName = "DPI",
        ///         ToolExecutableNames =  new []{ "dpi", "dpi.exe"},
        ///      };
        ///
        /// // Example with ProcessArgumentBuilder
        /// Command(
        ///     settings,
        ///     new ProcessArgumentBuilder()
        ///          .Append("nuget")
        ///          .AppendQuoted(Context.Environment.WorkingDirectory.FullPath)
        ///          .AppendSwitch("--output", " ", "TABLE")
        ///          .Append("analyze")
        /// );
        ///
        /// // Example with implicit ProcessArgumentBuilder
        /// Command(
        ///      settings,
        ///      $"nuget --output TABLE analyze"
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        public static void Command(
            this ICakeContext context,
            CommandSettings settings,
            ProcessArgumentBuilder arguments = null)
        {
            var runner = GetRunner(context, settings, ref arguments);

            runner.RunCommand(arguments);
        }

        /// <summary>
        /// Executes a generic tool/process based on arguments, tool executable names and redirects standard output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="toolExecutableNames">The tool executable names.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="arguments">The optional arguments.</param>
        /// <param name="expectedExitCode">The expected exit code (default 0).</param>
        /// <param name="settingsCustomization">The optional settings customization.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arguments"/>, <paramref name="context"/> or <paramref name="toolExecutableNames"/> is null.</exception>
        /// <returns>The exit code.</returns>
        /// <example>
        /// <code>
        /// using System.Text.Json.Serialization;
        /// using System.Text.Json;
        /// #tool dotnet:?package=DPI&amp;version=2022.8.21.54
        ///
        /// // Example with ProcessArgumentBuilder
        /// var exitCode = Command(
        ///     new []{ "dpi", "dpi.exe"},
        ///     out var standardOutput,
        ///     new ProcessArgumentBuilder()
        ///          .Append("nuget")
        ///          .AppendQuoted(Context.Environment.WorkingDirectory.FullPath)
        ///          .AppendSwitch("--output", " ", "JSON")
        ///          .Append("analyze")
        /// );
        ///
        /// var packageReferences =  JsonSerializer.Deserialize&lt;DPIPackageReference[]&gt;(
        ///     standardOutput
        /// );
        ///
        /// // Example with implicit ProcessArgumentBuilder
        /// var implicitExitCode = Command(
        ///      new []{ "dpi", "dpi.exe"},
        ///      out var implicitStandardOutput,
        ///      $"nuget --output JSON analyze"
        /// );
        ///
        /// var implicitPackageReferences =  JsonSerializer.Deserialize&lt;DPIPackageReference[]&gt;(
        ///     implicitStandardOutput
        /// );
        ///
        /// // Example settings customization
        /// var settingsCustomizationExitCode = Command(
        ///     new []{ "dpi", "dpi.exe"},
        ///     out var settingsCustomizationStandardOutput,
        ///     $"nuget --output JSON analyze",
        ///     settingsCustomization: settings => settings
        ///                                             .WithToolName("DPI")
        ///                                             .WithArgumentCustomization(args => args.AppendSwitchQuoted("--buildversion", " ", "1.0.0"))
        /// );
        ///
        /// var settingsCustomizationPackageReferences =  JsonSerializer.Deserialize&lt;DPIPackageReference[]&gt;(
        ///     settingsCustomizationStandardOutput
        /// );
        ///
        /// // Record used in example above
        /// public record DPIPackageReference(
        ///     [property: JsonPropertyName("source")]
        ///     string Source,
        ///     [property: JsonPropertyName("sourceType")]
        ///     string SourceType,
        ///     [property: JsonPropertyName("packageId")]
        ///     string PackageId,
        ///     [property: JsonPropertyName("version")]
        ///     string Version
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        public static int Command(
            this ICakeContext context,
            ICollection<string> toolExecutableNames,
            out string standardOutput,
            ProcessArgumentBuilder arguments = null,
            int expectedExitCode = 0,
            Func<CommandSettings, CommandSettings> settingsCustomization = null)
            => context.Command(
                GetSettings(toolExecutableNames, expectedExitCode, settingsCustomization),
                out standardOutput,
                arguments);

        /// <summary>
        /// Executes a generic tool/process based on arguments, settings and redirects standard output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="arguments">The optional arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="settings"/> is null.</exception>
        /// <returns>The exit code.</returns>
        /// <example>
        /// <code>
        /// using System.Text.Json.Serialization;
        /// using System.Text.Json;
        /// #tool dotnet:?package=DPI&amp;version=2022.8.21.54
        /// // Reusable tools settings i.e. created in setup.
        /// var settings = new CommandSettings {
        ///         ToolName = "DPI",
        ///         ToolExecutableNames =  new []{ "dpi", "dpi.exe" },
        ///      };
        ///
        /// // Example with ProcessArgumentBuilder
        /// var exitCode = Command(
        ///     settings,
        ///     out var standardOutput,
        ///     new ProcessArgumentBuilder()
        ///          .Append("nuget")
        ///          .AppendQuoted(Context.Environment.WorkingDirectory.FullPath)
        ///          .AppendSwitch("--output", " ", "JSON")
        ///          .Append("analyze")
        /// );
        ///
        /// var packageReferences =  JsonSerializer.Deserialize&lt;DPIPackageReference[]&gt;(
        ///     standardOutput
        /// );
        ///
        /// // Example with implicit ProcessArgumentBuilder
        /// var implicitExitCode = Command(
        ///      settings,
        ///      out var implicitStandardOutput,
        ///      $"nuget --output JSON analyze"
        /// );
        ///
        /// var implicitPackageReferences =  JsonSerializer.Deserialize&lt;DPIPackageReference[]&gt;(
        ///     implicitStandardOutput
        /// );
        ///
        /// // Record used in example above
        /// public record DPIPackageReference(
        ///     [property: JsonPropertyName("source")]
        ///     string Source,
        ///     [property: JsonPropertyName("sourceType")]
        ///     string SourceType,
        ///     [property: JsonPropertyName("packageId")]
        ///     string PackageId,
        ///     [property: JsonPropertyName("version")]
        ///     string Version
        /// );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        public static int Command(
            this ICakeContext context,
            CommandSettings settings,
            out string standardOutput,
            ProcessArgumentBuilder arguments = null)
        {
            var runner = GetRunner(context, settings, ref arguments);

            return runner.RunCommand(arguments, out standardOutput);
        }

        /// <summary>
        /// Executes a generic tool/process based on arguments, settings, redirects standard output and standard error.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="toolExecutableNames">The tool executable names.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="standardError">The standard error.</param>
        /// <param name="arguments">The optional arguments.</param>
        /// <param name="expectedExitCode">The expected exit code (default 0).</param>
        /// <param name="settingsCustomization">The optional settings customization (default null).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="toolExecutableNames"/> is null.</exception>
        /// <returns>The exit code.</returns>
        /// <example>
        /// <code>
        /// // Example with ProcessArgumentBuilder
        /// var exitCode = Command(
        ///     new []{ "dotnet", "dotnet.exe" },
        ///     out var standardOutput,
        ///     out var standardError,
        ///     new ProcessArgumentBuilder()
        ///         .Append("tool"),
        ///     expectedExitCode:1
        /// );
        ///
        /// Verbose("Exit code: {0}", exitCode);
        /// Information("Output: {0}", standardOutput);
        /// Error("Error: {0}", standardError);
        ///
        ///
        /// // Example with implicit ProcessArgumentBuilder
        /// var implicitExitCode = Command(
        ///     new []{ "dotnet", "dotnet.exe" },
        ///     out var implicitStandardOutput,
        ///     out var implicitStandardError,
        ///     "tool",
        ///     expectedExitCode:1
        /// );
        ///
        /// Verbose("Exit code: {0}", implicitExitCode);
        /// Information("Output: {0}", implicitStandardOutput);
        /// Error("Error: {0}", implicitStandardError);
        ///
        ///
        /// // Example settings customization
        /// var settingsCustomizationExitCode = Command(
        ///     new []{ "dotnet", "dotnet.exe" },
        ///     out var settingsCustomizationStandardOutput,
        ///     out var settingsCustomizationStandardError,
        ///     settingsCustomization: settings => settings
        ///                                         .WithToolName(".NET Tool")
        ///                                         .WithArgumentCustomization(args => args.Append("tool"))
        ///                                         .WithExpectedExitCode(1)
        /// );
        ///
        /// Verbose("Exit code: {0}", settingsCustomizationExitCode);
        /// Information("Output: {0}", settingsCustomizationStandardOutput);
        /// Error("Error: {0}", settingsCustomizationStandardError);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        public static int Command(
            this ICakeContext context,
            ICollection<string> toolExecutableNames,
            out string standardOutput,
            out string standardError,
            ProcessArgumentBuilder arguments = null,
            int expectedExitCode = 0,
            Func<CommandSettings, CommandSettings> settingsCustomization = null)
            => context.Command(
                GetSettings(toolExecutableNames, expectedExitCode, settingsCustomization),
                out standardOutput,
                out standardError,
                arguments);

        /// <summary>
        /// Executes a generic tool/process based on arguments and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="standardError">The standard error.</param>
        /// <param name="arguments">The optional arguments.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> or <paramref name="settings"/> is null.</exception>
        /// <returns>The exit code.</returns>
        /// <example>
        /// <code>
        /// // Reusable tools settings i.e. created in setup.
        /// var settings = new CommandSettings {
        ///         ToolName = ".NET CLI",
        ///         ToolExecutableNames =  new []{ "dotnet", "dotnet.exe" },
        ///      }.WithExpectedExitCode(1);
        ///
        /// // Example with ProcessArgumentBuilder
        /// var exitCode = Command(
        ///     settings,
        ///     out var standardOutput,
        ///     out var standardError,
        ///     new ProcessArgumentBuilder()
        ///         .Append("tool")
        /// );
        ///
        /// Verbose("Exit code: {0}", exitCode);
        /// Information("Output: {0}", standardOutput);
        /// Error("Error: {0}", standardError);
        ///
        ///
        /// // Example with implicit ProcessArgumentBuilder
        /// var implicitExitCode = Command(
        ///     settings,
        ///     out var implicitStandardOutput,
        ///     out var implicitStandardError,
        ///     "tool"
        /// );
        ///
        /// Verbose("Exit code: {0}", implicitExitCode);
        /// Information("Output: {0}", implicitStandardOutput);
        /// Error("Error: {0}", implicitStandardError);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Command")]
        public static int Command(
            this ICakeContext context,
            CommandSettings settings,
            out string standardOutput,
            out string standardError,
            ProcessArgumentBuilder arguments = null)
        {
            var runner = GetRunner(context, settings, ref arguments);

            return runner.RunCommand(arguments, out standardOutput, out standardError);
        }

        private static CommandSettings GetSettings(
            ICollection<string> toolExecutableNames,
            int expectedExitCode,
            Func<CommandSettings, CommandSettings> settingsCustomization)
        {
            if (toolExecutableNames is null || toolExecutableNames.Count < 1)
            {
                throw new ArgumentNullException(nameof(toolExecutableNames));
            }

            var settings = new CommandSettings
            {
                ToolName = toolExecutableNames.First(),
                ToolExecutableNames = toolExecutableNames,
                HandleExitCode = exitCode => exitCode == expectedExitCode
            };

            return settingsCustomization?.Invoke(settings) ?? settings;
        }

        private static CommandRunner GetRunner(ICakeContext context, CommandSettings settings, ref ProcessArgumentBuilder arguments)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            arguments ??= new ProcessArgumentBuilder();

            var runner = new CommandRunner(
                settings,
                context.FileSystem,
                context.Environment,
                context.ProcessRunner,
                context.Tools);

            return runner;
        }
    }
}
