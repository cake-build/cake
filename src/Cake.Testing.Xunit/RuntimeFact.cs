// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Testing.Xunit
{
    public sealed class RuntimeFact : FactAttribute
    {
        public RuntimeFact(TestRuntime runtime)
        {
            if (runtime == TestRuntime.Clr)
            {
#if NETCORE
                Skip = "Full framework test.";
#endif
            }
            else if (runtime == TestRuntime.CoreClr)
            {
#if !NETCORE
                Skip = ".NET Core test.";
#endif
            }
        }
    }
}
