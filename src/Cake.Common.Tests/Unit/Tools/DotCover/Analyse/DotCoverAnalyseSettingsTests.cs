using Cake.Common.Tools.DotCover;
using Cake.Common.Tools.DotCover.Analyse;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotCover.Analyse
{
    public sealed class DotCoverAnalyseSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Enable_Default_Filters_By_Default()
            {
                // Given, When
                var settings = new DotCoverAnalyseSettings();

                // Then
                Assert.False(settings.DisableDefaultFilters);
            }

            [Fact]
            public void Should_Use_XML_Report_Type_By_Default()
            {
                // Given, When
                var settings = new DotCoverAnalyseSettings();

                // Then
                Assert.Equal(settings.ReportType, DotCoverReportType.XML);
            }
        }
    }
}