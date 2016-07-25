// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakePlatform"/>.
    /// </summary>
    public static class CakePlatformExtensions
    {
        /// <summary>
        /// Determines whether the specified platform is a Unix platform.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns><c>true</c> if the platform is a Unix platform; otherwise <c>false</c>.</returns>
        public static bool IsUnix(this ICakePlatform platform)
        {
            if (platform == null)
            {
                throw new ArgumentNullException("platform");
            }
            return Machine.IsUnix(platform.Family);
        }
    }
}