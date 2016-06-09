// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tooling;
using Cake.Testing.Fixtures;

// ReSharper disable once CheckNamespace
namespace Cake.Testing
{
    /// <summary>
    /// Contains extension methods for <see cref="ToolFixture{TToolSettings,TFixtureResult}"/>.
    /// </summary>
    public static class ToolFixtureExtensions
    {
        /// <summary>
        /// Ensures that the tool do not exist under the tool settings tool path.
        /// </summary>
        /// <typeparam name="TToolSettings">The type of the tool settings.</typeparam>
        /// <typeparam name="TFixtureResult">The type of the fixture result.</typeparam>
        /// <param name="fixture">The fixture.</param>
        public static void GivenDefaultToolDoNotExist<TToolSettings, TFixtureResult>(
            this ToolFixture<TToolSettings, TFixtureResult> fixture)
            where TToolSettings : ToolSettings, new()
            where TFixtureResult : ToolFixtureResult
        {
            var file = fixture.FileSystem.GetFile(fixture.DefaultToolPath);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Ensures that the tool exist under the tool settings tool path.
        /// </summary>
        /// <typeparam name="TToolSettings">The type of the tool settings.</typeparam>
        /// <typeparam name="TFixtureResult">The type of the fixture result.</typeparam>
        /// <param name="fixture">The fixture.</param>
        public static void GivenSettingsToolPathExist<TToolSettings, TFixtureResult>(
            this ToolFixture<TToolSettings, TFixtureResult> fixture)
            where TToolSettings : ToolSettings, new()
            where TFixtureResult : ToolFixtureResult
        {
            if (fixture.Settings.ToolPath != null)
            {
                var path = fixture.Settings.ToolPath.MakeAbsolute(fixture.Environment);
                fixture.FileSystem.CreateFile(path);
            }
        }

        /// <summary>
        /// Ensures that the tool's process is unable to start.
        /// </summary>
        /// <typeparam name="TToolSettings">The type of the tool settings.</typeparam>
        /// <typeparam name="TFixtureResult">The type of the fixture result.</typeparam>
        /// <param name="fixture">The fixture.</param>
        public static void GivenProcessCannotStart<TToolSettings, TFixtureResult>(
            this ToolFixture<TToolSettings, TFixtureResult> fixture)
            where TToolSettings : ToolSettings, new()
            where TFixtureResult : ToolFixtureResult
        {
            fixture.ProcessRunner.Process = null;
        }

        /// <summary>
        /// Ensures that the tool process exits with the given exit code.
        /// </summary>
        /// <typeparam name="TToolSettings">The type of the tool settings.</typeparam>
        /// <typeparam name="TFixtureResult">The type of the fixture result.</typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="exitCode">The exit code.</param>
        public static void GivenProcessExitsWithCode<TToolSettings, TFixtureResult>(
            this ToolFixture<TToolSettings, TFixtureResult> fixture, int exitCode)
            where TToolSettings : ToolSettings, new()
            where TFixtureResult : ToolFixtureResult
        {
            fixture.ProcessRunner.Process.SetExitCode(exitCode);
        }
    }
}
