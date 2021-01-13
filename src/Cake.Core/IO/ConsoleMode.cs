using System;
using System.Runtime.InteropServices;

namespace Cake.Core.IO
{
    internal sealed class ConsoleMode
    {
#pragma warning disable SA1310 // Field names should not contain underscore
        private const int STD_OUTPUT_HANDLE = -11;
#pragma warning restore SA1310 // Field names should not contain underscore

        private readonly uint? _previousConsoleMode;

        public static ConsoleMode None { get; } = new ConsoleMode();

        public static ConsoleMode GetCurrent(ICakeEnvironment environment)
        {
            // We only need to reset the Console Mode on Windows
            if (environment.Platform.Family != PlatformFamily.Windows)
            {
                return None;
            }

            var @out = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(@out, out var currentConsoleMode))
            {
                return None;
            }

            return new ConsoleMode(currentConsoleMode);
        }

        private ConsoleMode()
        {
        }

        public ConsoleMode(uint consoleMode)
        {
            _previousConsoleMode = consoleMode;
        }

        public void Reset()
        {
            if (!_previousConsoleMode.HasValue)
            {
                // We're not running on Windows or the GetConsoleMode call in GetCurrent failed
                return;
            }

            var @out = GetStdHandle(STD_OUTPUT_HANDLE);
            if (GetConsoleMode(@out, out var currentConsoleMode) && currentConsoleMode != _previousConsoleMode)
            {
                SetConsoleMode(@out, _previousConsoleMode.Value);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}
