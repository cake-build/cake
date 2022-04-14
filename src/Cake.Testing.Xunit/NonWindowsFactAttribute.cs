// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Testing.Xunit
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class NonWindowsFactAttribute : PlatformRestrictedFactAttribute
    {
        public NonWindowsFactAttribute(string reason = null)
            : base(PlatformFamily.Windows, true, reason)
        {
        }
    }
}