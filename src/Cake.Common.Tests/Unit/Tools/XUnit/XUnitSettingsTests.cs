// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.XUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.XUnit
{
    public sealed class XUnitSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Output_Directory_To_Null_By_Default()
            {
                // Given, When
                var settings = new XUnitSettings();

                // Then
                Assert.Null(settings.OutputDirectory);
            }

            [Fact]
            public void Should_Disable_XML_Report_By_Default()
            {
                // Given, When
                var settings = new XUnitSettings();

                // Then
                Assert.False(settings.XmlReport);
            }

            [Fact]
            public void Should_Disable_HTML_Report_By_Default()
            {
                // Given, When
                var settings = new XUnitSettings();

                // Then
                Assert.False(settings.HtmlReport);
            }

            [Fact]
            public void Should_Enable_Shadow_Copying_By_Default()
            {
                // Given, When
                var settings = new XUnitSettings();

                // Then
                Assert.True(settings.ShadowCopy);
            }

            [Fact]
            public void Should_Disable_Silent_Mode_By_Default()
            {
                // Given, When
                var settings = new XUnitSettings();

                // Then
                Assert.False(settings.Silent);
            }
        }
    }
}
