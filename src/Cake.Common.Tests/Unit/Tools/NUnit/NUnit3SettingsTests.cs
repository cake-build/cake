// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnit3SettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Not_Enable_Shadow_Copying_By_Default()
            {
                // Given, When
                var settings = new NUnit3Settings();

                // Then
                Assert.False(settings.ShadowCopy);
            }

            [Fact]
            public void Should_Use_Multiple_Processes_By_Default()
            {
                // Given, When
                var settings = new NUnit3Settings();

                // Then
                Assert.Equal(settings.Process, NUnit3ProcessOption.Multiple);
            }

            [Fact]
            public void Should_Use_No_Labels_By_Default()
            {
                // Given, When
                var settings = new NUnit3Settings();

                // Then
                Assert.Equal(settings.Labels, NUnit3Labels.Off);
            }

            [Fact]
            public void Should_Use_Default_AppDomainUsage_By_Default()
            {
                // Given, When
                var settings = new NUnit3Settings();

                // Then
                Assert.Equal(settings.AppDomainUsage, NUnit3AppDomainUsage.Default);
            }
        }
    }
}
