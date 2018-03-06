// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Solution.Project.Properties
{
    /// <summary>
    /// Contains functionality related to AssemblyInfo settings.
    /// </summary>
    public static class AssemblyInfoSettingsExtensions
    {
        /// <summary>
        /// Adds a custom attribute to the AssemblyInfo settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The name of the custom attribute.</param>
        /// <param name="namespace">The namespace for the custom attribute.</param>
        /// <param name="value">The value for the attribute.</param>
        /// <returns>The same <see cref="AssemblyInfoSettings"/> instance so that multiple calls can be chained.</returns>
        public static AssemblyInfoSettings AddCustomAttribute(this AssemblyInfoSettings settings, string name, string @namespace, string value)
        {
            if (settings.CustomAttributes == null)
            {
                settings.CustomAttributes = new List<AssemblyInfoCustomAttribute>();
            }
            settings.CustomAttributes.Add(new AssemblyInfoCustomAttribute() { Name = name, NameSpace = @namespace, Value = value });
            return settings;
        }

        /// <summary>
        /// Adds a meta data attribute to the AssemblyInfo settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="key">The key of the meta data attribute.</param>
        /// <param name="value">The value for the attribute.</param>
        /// <returns>The same <see cref="AssemblyInfoSettings"/> instance so that multiple calls can be chained.</returns>
        public static AssemblyInfoSettings AddMetadataAttribute(this AssemblyInfoSettings settings, string key, string value)
        {
            if (settings.MetaDataAttributes == null)
            {
                settings.MetaDataAttributes = new List<AssemblyInfoMetadataAttribute>();
            }
            settings.MetaDataAttributes.Add(new AssemblyInfoMetadataAttribute { Key = key, Value = value });
            return settings;
        }
    }
}