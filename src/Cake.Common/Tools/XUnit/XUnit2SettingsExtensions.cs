// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Common.Tools.XUnit
{
    /// <summary>
    /// Contains functionality related to XUnit2 settings.
    /// </summary>
    public static class XUnit2SettingsExtensions
    {
        /// <summary>
        /// Adds a trait to the settings, to include in test execution.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The trait name.</param>
        /// <param name="values">The trait values.</param>
        /// <returns>The same <see cref="XUnit2Settings"/> instance so that multiple calls can be chained.</returns>
        public static XUnit2Settings IncludeTrait(this XUnit2Settings settings, string name, params string[] values)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Any(v => v == null))
            {
                throw new ArgumentException("values may not contain a null value.", nameof(values));
            }

            if (!settings.TraitsToInclude.ContainsKey(name))
            {
                settings.TraitsToInclude.Add(name, new List<string>());
            }

            foreach (var value in values.Where(v => v != null))
            {
                settings.TraitsToInclude[name].Add(value);
            }

            return settings;
        }

        /// <summary>
        /// Adds a trait to the settings, to exclude in test execution.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="name">The trait name.</param>
        /// <param name="values">The trait values.</param>
        /// <returns>The same <see cref="XUnit2Settings"/> instance so that multiple calls can be chained.</returns>
        public static XUnit2Settings ExcludeTrait(this XUnit2Settings settings, string name, params string[] values)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Any(v => v == null))
            {
                throw new ArgumentException("values may not contain a null value.", nameof(values));
            }

            if (!settings.TraitsToExclude.ContainsKey(name))
            {
                settings.TraitsToExclude.Add(name, new List<string>());
            }

            foreach (var value in values.Where(v => v != null))
            {
                settings.TraitsToExclude[name].Add(value);
            }

            return settings;
        }

        /// <summary>
        /// Adds a namespace to the settings, to include in test execution.  Namespace should be fully qualified; i.e., MyNameSpace.MySubNamespace.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="namespaceToInclude">The namespace to include.</param>
        /// <returns>The same <see cref="XUnit2Settings"/> instance so that multiple calls can be chained.</returns>
        public static XUnit2Settings IncludeNamespace(this XUnit2Settings settings, string namespaceToInclude)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrEmpty(namespaceToInclude))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(namespaceToInclude));
            }

            settings.NamespacesToInclude.Add(namespaceToInclude);

            return settings;
        }

        /// <summary>
        /// Adds a class name to the settings, to include in test execution. Class name should be fully qualified; i.e., MyNameSpace.MyClassName.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="classNameToInclude">The class name to include.</param>
        /// <returns>The same <see cref="XUnit2Settings"/> instance so that multiple calls can be chained.</returns>
        public static XUnit2Settings IncludeClass(this XUnit2Settings settings, string classNameToInclude)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrEmpty(classNameToInclude))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(classNameToInclude));
            }

            settings.ClassesToInclude.Add(classNameToInclude);

            return settings;
        }

        /// <summary>
        /// Adds a method name to the settings, to include in test execution. Method name should be fully qualified; i.e., MyNameSpace.MyClassName.MyMethod.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="methodNameToInclude">The method name to include.</param>
        /// <returns>The same <see cref="XUnit2Settings"/> instance so that multiple calls can be chained.</returns>
        public static XUnit2Settings IncludeMethod(this XUnit2Settings settings, string methodNameToInclude)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrEmpty(methodNameToInclude))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(methodNameToInclude));
            }

            settings.MethodsToInclude.Add(methodNameToInclude);

            return settings;
        }
    }
}