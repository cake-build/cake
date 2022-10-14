// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Contains functionality related to <see cref="ToolSettings" />.
    /// </summary>
    public static class ToolSettingsExtensions
    {
        /// <summary>
        /// Provides fluent null guarded tool settings action.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="toolSettingsAction">The tools settings action.</param>
        /// <returns>The tools settings.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="toolSettings"/> or <paramref name="toolSettingsAction"/> is null.</exception>
        public static T WithToolSettings<T>(this T toolSettings, Action<T> toolSettingsAction)
               where T : ToolSettings
        {
            if (toolSettings is null)
            {
                throw new ArgumentNullException(nameof(toolSettings));
            }

            if (toolSettingsAction is null)
            {
                throw new ArgumentNullException(nameof(toolSettingsAction));
            }

            toolSettingsAction.Invoke(toolSettings);

            return toolSettings;
        }

        /// <summary>
        /// Sets the tool path.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="toolPath">The tool path.</param>
        /// <returns>The tools settings.</returns>
        public static T WithToolPath<T>(this T toolSettings, FilePath toolPath)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.ToolPath = toolPath);

        /// <summary>
        /// Sets the tool timeout.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="toolTimeout">The tool timeout.</param>
        /// <returns>The tools settings.</returns>
        public static T WithToolTimeout<T>(this T toolSettings, TimeSpan toolTimeout)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.ToolTimeout = toolTimeout);

        /// <summary>
        /// Sets the tool working directory.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="workingDirectory">The tool working directory.</param>
        /// <returns>The tools settings.</returns>
        public static T WithWorkingDirectory<T>(this T toolSettings, DirectoryPath workingDirectory)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.WorkingDirectory = workingDirectory);

        /// <summary>
        /// Sets whether the tool should use a working directory or not.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="noWorkingDirectory">Flag for no working directory (default true).</param>
        /// <returns>The tools settings.</returns>
        public static T WithNoWorkingDirectory<T>(this T toolSettings, bool noWorkingDirectory = true)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.NoWorkingDirectory = noWorkingDirectory);

        /// <summary>
        /// Sets the tool argument customization delegate.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="argumentCustomization">The tool argument customization delegate.</param>
        /// <returns>The tools settings.</returns>
        public static T WithArgumentCustomization<T>(this T toolSettings, Func<ProcessArgumentBuilder, ProcessArgumentBuilder> argumentCustomization)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.ArgumentCustomization = argumentCustomization);

        /// <summary>
        /// Sets or adds tool environment variable.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="key">The tool environment variable key.</param>
        /// <param name="value">The tool environment variable value.</param>
        /// <returns>The tools settings.</returns>
        public static T WithEnvironmentVariable<T>(this T toolSettings, string key, string value)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.EnvironmentVariables[key] = value);

        /// <summary>
        /// Sets delegate whether the exit code from the tool process causes an exception to be thrown.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="handleExitCode">The tool handle exit code delegate.</param>
        /// <returns>The tools settings.</returns>
        public static T WithHandleExitCode<T>(this T toolSettings, Func<int, bool> handleExitCode)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.HandleExitCode = handleExitCode);

        /// <summary>
        /// Sets a delegate which is executed after the tool process was started.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="postAction">The tool argument customization delegate.</param>
        /// <returns>The tools settings.</returns>
        public static T WithPostAction<T>(this T toolSettings, Action<IProcess> postAction)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.PostAction = postAction);

        /// <summary>
        /// Sets a delegate to configure the process settings.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="setupProcessSettings">The setup process settings delegate.</param>
        /// <returns>The tools settings.</returns>
        public static T WithSetupProcessSettings<T>(this T toolSettings, Action<ProcessSettings> setupProcessSettings)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.SetupProcessSettings = setupProcessSettings);

        /// <summary>
        /// Sets expected exit code using <see cref="WithHandleExitCode{T}(T, Func{int, bool})"/>.
        /// </summary>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="expectExitCode">The tool expected exit code.</param>
        /// <returns>The tools settings.</returns>
        public static T WithExpectedExitCode<T>(this T toolSettings, int expectExitCode)
               where T : ToolSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.HandleExitCode = exitCode => exitCode == expectExitCode);
    }
}