// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools;

public sealed class VisualStudioTests
{
    public sealed class TheGetYearAndEditionRootPathsMethod
    {
        [Theory]
        [InlineData("2022", "BuildTools", new[] { "/Program86/Microsoft Visual Studio/2022/BuildTools", "/Program/Microsoft Visual Studio/2022/BuildTools" })]
        [InlineData("18", "BuildTools", new[] { "/Program86/Microsoft Visual Studio/18/BuildTools", "/Program/Microsoft Visual Studio/18/BuildTools" })]
        [InlineData("2022", "Enterprise", new[] { "/Program/Microsoft Visual Studio/2022/Enterprise" })]
        [InlineData("18", "Community", new[] { "/Program/Microsoft Visual Studio/18/Community" })]
        [InlineData("2019", "BuildTools", new[] { "/Program86/Microsoft Visual Studio/2019/BuildTools" })]
        [InlineData("2017", "Enterprise", new[] { "/Program86/Microsoft Visual Studio/2017/Enterprise" })]
        public void Should_Return_All_Possible_Root_Paths(string year, string edition, string[] expected)
        {
            // Given
            var environment = FakeEnvironment.CreateWindowsEnvironment();
            environment.SetSpecialPath(SpecialPath.ProgramFilesX86, "/Program86");
            environment.SetSpecialPath(SpecialPath.ProgramFiles, "/Program");

            // When
            var result = VisualStudio.GetYearAndEditionRootPaths(environment, year, edition);

            // Then
            Assert.Equal(expected, result.Select(path => path.FullPath));
        }
    }
}
