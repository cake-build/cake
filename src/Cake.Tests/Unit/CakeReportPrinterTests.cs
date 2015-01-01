using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeReportPrinterTests
    {
        public sealed class TheWriteMethod
        {
            [Fact]
            public void Should_Throw_If_Report_Is_Null()
            {
                // Given
                var console = Substitute.For<IConsole>();
                var printer = new CakeReportPrinter(console);

                // When
                var result = Record.Exception(() => printer.Write(null));

                // Then
                Assert.IsArgumentNullException(result, "report");
            }
        }
    }
}
