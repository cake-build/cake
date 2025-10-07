// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Testing.Xunit
{
    /// <summary>
    /// Marks a test method as a theory that should only run on Windows platforms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class WindowsTheoryAttribute : PlatformRestrictedTheoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsTheoryAttribute"/> class.
        /// </summary>
        /// <param name="reason">The reason why the test is skipped on non-Windows platforms.</param>
        public WindowsTheoryAttribute(string reason = null)
            : base(PlatformFamily.Windows, false, reason)
        {
        }
    }
}