﻿using System;
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
        public sealed class TheTargetFrameworkProperty
        {
            [RuntimeFact(TestRuntime.Clr)]
            public void Should_Return_Correct_Result_For_Clr()
            {
                // Given
                var runtime = new CakeRuntime();

                // When
                var framework = runtime.TargetFramework;

                // Then
                Assert.Equal(".NETFramework,Version=v4.6", framework.FullName);
            }

            [RuntimeFact(TestRuntime.CoreClr)]
            public void Should_Return_Correct_Result_For_CoreClr()
            {
                // Given
                var runtime = new CakeRuntime();

                // When
                var framework = runtime.TargetFramework;

                // Then
                Assert.Equal(".NETStandard,Version=v1.6", framework.FullName);
            }
        }
    }
}
