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
using Xunit.Abstractions;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeRuntimeTests
    {
        public sealed class TheBuiltFrameworkProperty
        {
            private ITestOutputHelper TestOutputHelper { get; }

            public TheBuiltFrameworkProperty(ITestOutputHelper testOutputHelper)
            {
                TestOutputHelper = testOutputHelper;
            }

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
                var expect = string.Concat(".NETCoreApp,Version=v",
#if NET5_0
                                "5.0");
#elif NETCOREAPP2_0
                                "2.0");
#elif NETCOREAPP2_1
                                "2.1");
#elif NETCOREAPP2_2
                                "2.2");
#elif NETCOREAPP3_0
                                "3.0");
#elif NETCOREAPP3_1
                                "3.1");
#endif
                // ToDo: Enable mocking runtime resolution, temp work around for missing runtimes.
                switch (expect)
                {
                    case ".NETCoreApp,Version=v2.0":
                    {
                        switch (framework.FullName)
                        {
                            case ".NETCoreApp,Version=v2.1":
                            case ".NETCoreApp,Version=v3.0":
                            case ".NETCoreApp,Version=v3.1":
                            case ".NETCoreApp,Version=v5.0":
                                {
                                    TestOutputHelper.WriteLine("Expect changed from {0} to {1}.", expect, framework.FullName);
                                    expect = framework.FullName;
                                    break;
                                }
                        }
                        break;
                    }
                    case ".NETCoreApp,Version=v2.1":
                    {
                        switch (framework.FullName)
                        {
                            case ".NETCoreApp,Version=v3.0":
                            case ".NETCoreApp,Version=v3.1":
                            case ".NETCoreApp,Version=v5.0":
                            {
                                TestOutputHelper.WriteLine("Expect changed from {0} to {1}.", expect, framework.FullName);
                                expect = framework.FullName;
                                break;
                            }
                        }
                        break;
                    }
                    case ".NETCoreApp,Version=v3.0":
                    {
                        switch (framework.FullName)
                        {
                            case ".NETCoreApp,Version=v3.1":
                            case ".NETCoreApp,Version=v5.0":
                            {
                                TestOutputHelper.WriteLine("Expect changed from {0} to {1}.", expect, framework.FullName);
                                expect = framework.FullName;
                                break;
                            }
                        }
                        break;
                    }
                    case ".NETCoreApp,Version=v3.1":
                    {
                        switch (framework.FullName)
                        {
                            case ".NETCoreApp,Version=v5.0":
                                {
                                    TestOutputHelper.WriteLine("Expect changed from {0} to {1}.", expect, framework.FullName);
                                    expect = framework.FullName;
                                    break;
                                }
                        }
                        break;
                    }
                }
                Assert.Equal(expect, framework.FullName);
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
