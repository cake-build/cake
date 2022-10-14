// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tooling;

namespace Cake.Common.Tools.Command
{
    /// <summary>
    /// Contains functionality related to <see cref="CommandRunner" />.
    /// </summary>
    public static class CommandSettingsExtensions
    {
        /// <summary>
        /// Sets the tool executable names.
        /// </summary>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="toolExecutableNames">The tool executable names.</param>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <returns>The tools settings.</returns>
        public static T WithExecutableNames<T>(this T toolSettings, params string[] toolExecutableNames)
           where T : CommandSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.ToolExecutableNames = toolExecutableNames);

        /// <summary>
        /// Sets the tool name.
        /// </summary>
        /// <param name="toolSettings">The tools settings.</param>
        /// <param name="toolName">The tool name.</param>
        /// <typeparam name="T">The ToolSettings type.</typeparam>
        /// <returns>The tools settings.</returns>
        public static T WithToolName<T>(this T toolSettings, string toolName)
            where T : CommandSettings
            => toolSettings.WithToolSettings(toolSettings => toolSettings.ToolName = toolName);
    }
}
