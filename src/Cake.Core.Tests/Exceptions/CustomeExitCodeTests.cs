using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Cake.Core.Tests.Exceptions
{
    public class CustomeExitCodeTests
    {
        [Fact]
        public void Should_Return_Default_ExitCode()
        {
            var exception = new CakeException();
            Assert.Equal(1, exception.ExitCode);
        }

        [Fact]
        public void Should_Return_Custom_ExitCode()
        {
            var exception = new CakeException(5);
            Assert.Equal(5, exception.ExitCode);
        }
    }
}
