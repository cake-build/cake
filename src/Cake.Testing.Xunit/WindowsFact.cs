// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class WindowsFact : FactAttribute
    {
        // ReSharper disable once UnusedParameter.Local
        public WindowsFact(string reason = null)
        {
#if __MonoCS__
            Skip = reason ?? "Windows test.";
#endif
        }
    }
}
