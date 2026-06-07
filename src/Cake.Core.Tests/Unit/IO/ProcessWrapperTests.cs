using System.Diagnostics;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessWrapperTests
    {
        public sealed class The_WaitForExit_Method
        {
            [Fact]
            public void Should_Not_Kill_Process_If_Timed_Wait_Expires()
            {
                // Given
                var fixture = new ProcessWrapperFixture
                {
                    Process = StartLongRunningProcess()
                };
                var wrapper = fixture.CreateProcessWrapper();

                try
                {
                    // When
                    var result = wrapper.WaitForExit(1);

                    // Then
                    fixture.Process.Refresh();
                    Assert.False(result);
                    Assert.False(fixture.Process.HasExited);
                }
                finally
                {
                    StopProcess(fixture.Process);
                }
            }
        }

        public sealed class The_StandardOutputReceived_Method
        {
            [Fact]
            public void Should_Pass_StandardOutput_Through_Handler()
            {
                // Given
                var receivedMessage = string.Empty;
                var fixture = new ProcessWrapperFixture();
                fixture.StandartOutputHandler = (s) => receivedMessage = s;
                var wrapper = fixture.CreateProcessWrapper();

                // When
                wrapper.StandardOutputReceived("message");

                // Then
                Assert.Equal("message", receivedMessage);
            }
        }

        public sealed class The_StandardErrorReceived_Method
        {
            [Fact]
            public void Should_Pass_StandardError_Through_Handler()
            {
                // Given
                var receivedMessage = string.Empty;
                var fixture = new ProcessWrapperFixture();
                fixture.StandardErrorHandler = (s) => receivedMessage = s;
                var wrapper = fixture.CreateProcessWrapper();

                // When
                wrapper.StandardErrorReceived("message");

                // Then
                Assert.Equal("message", receivedMessage);
            }
        }

        private static Process StartLongRunningProcess()
        {
            var startInfo = System.OperatingSystem.IsWindows()
                ? new ProcessStartInfo("powershell.exe", "-NoProfile -Command Start-Sleep -Seconds 30")
                : new ProcessStartInfo("/bin/sleep", "30");

            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            return Process.Start(startInfo);
        }

        private static void StopProcess(Process process)
        {
            if (process.HasExited)
            {
                return;
            }

            process.Kill(entireProcessTree: true);
            process.WaitForExit(5000);
        }
    }
}
