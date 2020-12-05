// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/////////////////////////////////////////////////////////////////////////////////////////////////////
// Portions of this code was ported from the supports-ansi project by Qingrong Ke
// https://github.com/keqingrong/supports-ansi/blob/master/index.js
/////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Cake.Core.Diagnostics
{
    internal static class AnsiDetector
    {
        private static readonly Regex[] _regexes;
        private static readonly Regex _teamCityVersionWithAnsiSupportRegEx;

        static AnsiDetector()
        {
            _regexes = new[]
            {
                new Regex("^xterm"), // xterm, PuTTY, Mintty
                new Regex("^rxvt"), // RXVT
                new Regex("^eterm"), // Eterm
                new Regex("^screen"), // GNU screen, tmux
                new Regex("tmux"), // tmux
                new Regex("^vt100"), // DEC VT series
                new Regex("^vt102"), // DEC VT series
                new Regex("^vt220"), // DEC VT series
                new Regex("^vt320"), // DEC VT series
                new Regex("ansi"), // ANSI
                new Regex("scoansi"), // SCO ANSI
                new Regex("cygwin"), // Cygwin, MinGW
                new Regex("linux"), // Linux console
                new Regex("konsole"), // Konsole
                new Regex("bvterm"), // Bitvise SSH Client
            };

            // TeamCity old version numbers look like 9.1.2, 9.1.6, 10.0.5, etc.
            // TeamCity current version numbers look like 2017.1, 2019.2.1, 2020.2, etc.
            // https://confluence.jetbrains.com/display/TW/Previous+Releases+Downloads
            _teamCityVersionWithAnsiSupportRegEx = new Regex(@"^(\d{2,4}\.|9\.([1-9]\d*))");
        }

        public static bool SupportsAnsi(ICakeEnvironment environment)
        {
            // Prevents the addition of ANSI color if NO_COLOR env. variable is present
            // https://no-color.org
            if (!string.IsNullOrWhiteSpace(environment.GetEnvironmentVariable("NO_COLOR")))
            {
                return false;
            }

            // Github action doesn't setup a correct PTY but supports ANSI.
            if (!string.IsNullOrWhiteSpace(environment.GetEnvironmentVariable("GITHUB_ACTION")))
            {
                return true;
            }

            // Azure Pipelines doesn't set the TERM environment variable but supports ANSI
            // https://github.com/microsoft/azure-pipelines-agent/issues/1569
            if (!string.IsNullOrWhiteSpace(environment.GetEnvironmentVariable("TF_BUILD")))
            {
                return true;
            }

            // TeamCity doesn't set the TERM environment variable but supports ANSI since 9.1
            // https://blog.jetbrains.com/teamcity/2015/07/teamcity-9-1-release-truly-historical-and-very-personal-builds/
            var teamCityVersion = environment.GetEnvironmentVariable("TEAMCITY_VERSION");
            if (!string.IsNullOrWhiteSpace(teamCityVersion) && _teamCityVersionWithAnsiSupportRegEx.IsMatch(teamCityVersion))
            {
                return true;
            }

            // Check if the terminal is of type ANSI/VT100/xterm compatible.
            var term = environment.GetEnvironmentVariable("TERM");
            if (!string.IsNullOrWhiteSpace(term))
            {
                if (_regexes.Any(regex => regex.IsMatch(term)))
                {
                    return true;
                }
            }

            // Running on Windows?
            if (environment.Platform.Family == PlatformFamily.Windows)
            {
                // Running under ConEmu?
                var conEmu = environment.GetEnvironmentVariable("ConEmuANSI");
                if (!string.IsNullOrEmpty(conEmu) && conEmu.Equals("On", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return Windows.SupportsAnsi();
            }

            return false;
        }

        internal static class Windows
        {
#pragma warning disable SA1310 // Field names should not contain underscore
            private const int STD_OUTPUT_HANDLE = -11;
            private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
            private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
#pragma warning restore SA1310 // Field names should not contain underscore

            [DllImport("kernel32.dll")]
            private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

            [DllImport("kernel32.dll")]
            private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();

            public static bool SupportsAnsi()
            {
                try
                {
                    var @out = GetStdHandle(STD_OUTPUT_HANDLE);
                    if (!GetConsoleMode(@out, out uint mode))
                    {
                        // Could not get console mode.
                        return false;
                    }

                    if ((mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == 0)
                    {
                        // Try enable ANSI support.
                        mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                        if (!SetConsoleMode(@out, mode))
                        {
                            // Enabling failed.
                            return false;
                        }
                    }

                    return true;
                }
                catch
                {
                    // All we know here is that we don't support ANSI.
                    // We can't log it since we don't have a log at this point.
                    return false;
                }
            }
        }
    }
}
