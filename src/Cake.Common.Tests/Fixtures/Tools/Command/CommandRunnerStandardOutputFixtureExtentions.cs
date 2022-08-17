// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tests.Fixtures.Tools.Command
{
    internal static class CommandRunnerStandardOutputFixtureExtentions
    {
        public static T GivenStandardOutput<T>(this T fixture, params string[] standardOutput)
           where T : CommandRunnerStandardOutputFixture
        {
            fixture.ProcessRunner.Process.SetStandardOutput(standardOutput);
            return fixture;
        }

        public static T GivenStandardError<T>(this T fixture, params string[] standardError)
           where T : CommandRunnerStandardOutputFixture
        {
            fixture.ProcessRunner.Process.SetStandardError(standardError);
            return fixture;
        }
    }
}
