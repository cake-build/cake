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
        }
    }
}
