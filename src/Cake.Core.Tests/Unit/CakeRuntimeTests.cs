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
        public sealed class TheBuiltFrameworkroperty
        {
            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Return_Correct_Result_For_CoreClr()
            {
                // Given
                var runtime = new CakeRuntime();

                // When
                var framework = runtime.BuiltFramework;

                // Then
                Assert.Equal(".NETStandard,Version=v2.0", framework.FullName);
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
