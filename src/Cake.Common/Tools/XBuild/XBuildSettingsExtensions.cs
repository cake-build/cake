// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Diagnostics;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// Contains functionality related to XBuild settings.
    /// </summary>
    public static class XBuildSettingsExtensions
    {
        /// <summary>
        /// Adds a XBuild target to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="target">The XBuild target.</param>
        /// <returns>The same <see cref="XBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static XBuildSettings WithTarget(this XBuildSettings settings, string target)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Targets.Add(target);
            return settings;
        }

        /// <summary>
        /// Sets the tool version.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="version">The version.</param>
        /// <returns>The same <see cref="XBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static XBuildSettings UseToolVersion(this XBuildSettings settings, XBuildToolVersion version)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.ToolVersion = version;
            return settings;
        }

        /// <summary>
        /// Adds a property to the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The property name.</param>
        /// <param name="values">The property values.</param>
        /// <returns>The same <see cref="XBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static XBuildSettings WithProperty(this XBuildSettings settings, string name, params string[] values)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            IList<string> currentValue;
            currentValue = new List<string>(
                settings.Properties.TryGetValue(name, out currentValue) && currentValue != null
                    ? currentValue.Concat(values)
                    : values);

            settings.Properties[name] = currentValue;

            return settings;
        }

        /// <summary>
        /// Sets the configuration.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The same <see cref="XBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static XBuildSettings SetConfiguration(this XBuildSettings settings, string configuration)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Configuration = configuration;
            return settings;
        }

        /// <summary>
        /// Sets the build log verbosity.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="verbosity">The build log verbosity.</param>
        /// <returns>The same <see cref="XBuildSettings"/> instance so that multiple calls can be chained.</returns>
        public static XBuildSettings SetVerbosity(this XBuildSettings settings, Verbosity verbosity)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            settings.Verbosity = verbosity;
            return settings;
        }
    }
}
