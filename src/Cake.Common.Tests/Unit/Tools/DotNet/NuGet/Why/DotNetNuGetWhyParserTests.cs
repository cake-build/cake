// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Why;

namespace Cake.Common.Tests.Unit.Tools.DotNet.NuGet.Why;

public sealed class DotNetNuGetWhyParserTests
{
    [Fact]
    public async Task Should_Parse_Hwapp_Tests_Sample()
    {
        // Given
        var lines = new[]
        {
            "Project 'hwapp.tests' has the following dependency graph(s) for 'xunit.v3.extensibility.core':",
            string.Empty,
            "  [net10.0]",
            "  └── xunit.v3.mtp-v2@3.2.2 (>= 3.2.2)",
            "      └── xunit.v3.core.mtp-v2@3.2.2 (= 3.2.2)",
            "          ├── xunit.v3.extensibility.core@3.2.2 (= 3.2.2)",
            "          └── xunit.v3.runner.inproc.console@3.2.2 (= 3.2.2)",
            "              └── xunit.v3.extensibility.core@3.2.2 (= 3.2.2)"
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Wrapped_Project_Header()
    {
        // Given
        var lines = new[]
        {
            "Project 'hwapp.tests' has the following dependency graph(s) for ",
            "'xunit.v3.extensibility.core':",
            string.Empty,
            "  [net10.0]                                                                     ",
            "  └── xunit.v3.mtp-v2@3.2.2 (>= 3.2.2)                                          "
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Ascii_Tree_And_Legacy_Version()
    {
        // Given
        var lines = new[]
        {
            "Project 'hwapp.tests' has the following dependency graph(s) for ",
            "'xunit.v3.extensibility.core':",
            string.Empty,
            "  [net10.0]                                                                     ",
            "  `-- xunit.v3.mtp-v2 (v3.2.2)                                                  ",
            "      `-- xunit.v3.core.mtp-v2 (v3.2.2)                                         ",
            "          |-- xunit.v3.extensibility.core (v3.2.2)                              ",
            "          `-- xunit.v3.runner.inproc.console (v3.2.2)                           ",
            "              `-- xunit.v3.extensibility.core (v3.2.2)                          "
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Ansi_Styled_Target_Package()
    {
        // Given
        var lines = new[]
        {
            "Project 'hwapp.tests' has the following dependency graph(s) for 'xunit.v3.extensibility.core':",
            "  [net10.0]",
            "  └── xunit.v3.mtp-v2@3.2.2 (>= 3.2.2)",
            "      └── xunit.v3.core.mtp-v2@3.2.2 (= 3.2.2)",
            "          ├── \u001b[38;5;51mxunit.v3.extensibility.core\u001b[0m@3.2.2 (= 3.2.2)",
            "          └── xunit.v3.runner.inproc.console@3.2.2 (= 3.2.2)",
            "              └── \u001b[38;5;51mxunit.v3.extensibility.core\u001b[0m@3.2.2 (= 3.2.2)"
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Diamond_Siblings()
    {
        // Given
        var lines = new[]
        {
            "Project 'App' has the following dependency graph(s) for 'System.Text.Json':",
            "  [net8.0]",
            "  ├── PackageA@1.0.0 (>= 1.0.0)",
            "  │   └── System.Text.Json@8.0.0 (= 8.0.0)",
            "  └── PackageB@2.0.0 (>= 2.0.0)",
            "      └── System.Text.Json@8.0.0 (= 8.0.0)"
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Rid_Framework()
    {
        // Given
        var lines = new[]
        {
            "Project 'App' has the following dependency graph(s) for 'Runtime.Win':",
            "  [net9.0/win-x64]",
            "  └── Runtime.Win@1.0.0 (>= 1.0.0)",
            "  [net9.0]",
            "  └── Runtime.Win@1.0.0 (>= 1.0.0)"
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Missing_Dependency()
    {
        // Given
        var lines = new[]
        {
            "Project 'hwapp' does not have a dependency on 'Missing.Package'."
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Parse_Multiple_Projects()
    {
        // Given
        var lines = new[]
        {
            "Project 'A' has the following dependency graph(s) for 'Newtonsoft.Json':",
            "  [net10.0]",
            "  └── Newtonsoft.Json@13.0.3 (>= 13.0.3)",
            "Project 'B' does not have a dependency on 'Newtonsoft.Json'"
        };

        // When
        var result = DotNetNuGetWhyParser.Parse(lines);

        // Then
        await Verify(result);
    }

    [Fact]
    public async Task Should_Return_Empty_Result_For_Null_Or_Empty_Input()
    {
        // Given
        IEnumerable<string> empty = [];

        // When
        var nullResult = DotNetNuGetWhyParser.Parse(null);
        var emptyResult = DotNetNuGetWhyParser.Parse(empty);

        // Then
        await Verify(new { Null = nullResult, Empty = emptyResult });
    }
}
