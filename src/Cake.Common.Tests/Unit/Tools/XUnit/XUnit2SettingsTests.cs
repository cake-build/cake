// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tools.XUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.XUnit
{
    public sealed class XUnit2SettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Output_Directory_To_Null_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.Null(settings.OutputDirectory);
            }

            [Fact]
            public void Should_Disable_XML_Report_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.False(settings.XmlReport);
            }

            [Fact]
            public void Should_Disable_HTML_Report_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.False(settings.HtmlReport);
            }

            [Fact]
            public void Should_Enable_Shadow_Copying_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.True(settings.ShadowCopy);
            }

            [Fact]
            public void Should_Set_NoAppDomain_To_False_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.False(settings.NoAppDomain);
            }

            [Fact]
            public void Should_Set_Parallelism_Option_To_None_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.Equal(settings.Parallelism, ParallelismOption.None);
            }

            [Fact]
            public void Should_Set_MaxThreads_Option_To_Null_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.Null(settings.MaxThreads);
            }

            [Fact]
            public void MaxThreads_Must_Not_Be_Negative()
            {
                // Given
                var settings = new XUnit2Settings();

                // When, Then
                Assert.Throws<ArgumentOutOfRangeException>(() => settings.MaxThreads = -1);
            }

            [Fact]
            public void Should_Set_TraitsToInclude_To_Empty_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.Empty(settings.TraitsToInclude);
            }

            [Fact]
            public void Should_Set_TraitsToExclude_To_Empty_By_Default()
            {
                // Given, When
                var settings = new XUnit2Settings();

                // Then
                Assert.Empty(settings.TraitsToExclude);
            }
        }
    }
}
