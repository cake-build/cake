// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Polyfill;
using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class RuntimeFact : FactAttribute
    {
        public RuntimeFact(TestRuntime runtime)
        {
            if (runtime.HasFlag(TestRuntime.Clr)
            && EnvironmentHelper.GetRuntime() != Runtime.Clr)
            {
                Skip = "Full framework test.";
            }
            else if (runtime.HasFlag(TestRuntime.CoreClr)
            && EnvironmentHelper.GetRuntime() != Runtime.CoreClr)
            {
                Skip = ".NET Core test.";
            }
        }
    }
}
