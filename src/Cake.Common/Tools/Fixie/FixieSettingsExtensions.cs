// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.Fixie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Contains functionality related to Fixie settings.
    /// </summary>
    public static class FixieSettingsExtensions
    {
        /// <summary>
        /// Adds an option to the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The option name.</param>
        /// <param name="values">The option values.</param>
        /// <returns>The same <see cref="FixieSettings"/> instance so that multiple calls can be chained.</returns>
        public static FixieSettings WithOption(this FixieSettings settings, string name, params string[] values)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            IList<string> currentValue;
            currentValue = new List<string>(settings.Options.TryGetValue(name, out currentValue) && currentValue != null
                    ? currentValue.Concat(values)
                    : values);

            settings.Options[name] = currentValue;

            return settings;
        }
    }
}
