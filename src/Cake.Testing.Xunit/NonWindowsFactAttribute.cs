// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Testing.Xunit
{
    /// <summary>
    /// Marks a test method as a fact that should only run on non-Windows platforms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class NonWindowsFactAttribute : PlatformRestrictedFactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonWindowsFactAttribute"/> class.
        /// </summary>
        /// <param name="reason">The reason why the test is skipped on Windows.</param>
        public NonWindowsFactAttribute(string reason = null)
            : base(PlatformFamily.Windows, true, reason)
        {
        }
    }
}