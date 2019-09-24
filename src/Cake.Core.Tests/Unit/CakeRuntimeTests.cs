// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeRuntimeTests
    {
        public sealed class TheBuiltFrameworkProperty
        {
            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Return_Correct_Result_For_CoreClr()
            {
                // Given
                var runtime = new CakeRuntime();

                // When
                var framework = runtime.BuiltFramework;

                // Then
#if NETFRAMEWORK
                Assert.Equal(".NETFramework,Version=v4.6.1", framework.FullName);
#elif !NETCOREAPP
                Assert.Equal(".NETStandard,Version=v2.0", framework.FullName);
#else
                try
                {
                    Assert.Equal(".NETCoreApp,Version=v" +

#if NETCOREAPP2_0
                    "2.0",
#elif NETCOREAPP2_1
                    "2.1",
#elif NETCOREAPP2_2
                    "2.2",
#elif NETCOREAPP3_0
                    "3.0",
#endif
                        framework.FullName);
                }
                catch
                {
                    // Temp fix for if only netcore3 installed
                    if (!StringComparer.Ordinal.Equals(framework.FullName, ".NETCoreApp,Version=v3.0"))
                    {
                        throw;
                    }
                }
#endif
            }
        }

        public sealed class TheExecutingFrameworkProperty
        {
            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Return_Correct_Result_For_Clr()
            {
                // Given
                var runtime = new CakeRuntime();

                // When
                var framework = runtime.BuiltFramework;

                // Then
                Assert.Equal(".NETFramework,Version=v4.6.1", framework.FullName);
            }
        }
    }
}
