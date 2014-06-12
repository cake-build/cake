using Cake.Common.XUnit;
using Xunit;

namespace Cake.Common.Tests.Unit.XUnit
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
        }
    }
}
