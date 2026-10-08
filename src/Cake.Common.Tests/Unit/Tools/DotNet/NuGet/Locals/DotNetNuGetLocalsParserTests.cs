// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Locals;

namespace Cake.Common.Tests.Unit.Tools.DotNet.NuGet.Locals;

public sealed class DotNetNuGetLocalsParserTests
{
    [Fact]
    public async Task Should_Parse_List_All()
    {
        // Given
        var lines = new[]
        {
            "http-cache: C:\\Users\\username\\AppData\\Local\\NuGet\\v3-cache",
            "global-packages: C:\\Users\\username\\.nuget\\packages\\",
            "temp: C:\\Users\\username\\AppData\\Local\\Temp\\NuGetScratch",
            "plugins-cache: C:\\Users\\username\\AppData\\Local\\NuGet\\plugins-cache"
        };

        // When
        var result = DotNetNuGetLocalsParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Single_Folder()
    {
        // Given
        var lines = new[]
        {
            "http-cache: C:\\Users\\username\\AppData\\Local\\NuGet\\v3-cache"
        };

        // When
        var result = DotNetNuGetLocalsParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Return_Empty_Result_For_Null_Or_Empty_Input()
    {
        // Given
        IEnumerable<string> empty = [];

        // When
        var nullResult = DotNetNuGetLocalsParser.Parse(null);
        var emptyResult = DotNetNuGetLocalsParser.Parse(empty);

        // Then
        await Verify(new { Null = nullResult, Empty = emptyResult });
    }
}
