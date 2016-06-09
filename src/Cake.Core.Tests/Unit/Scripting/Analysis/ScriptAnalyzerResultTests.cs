// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.Analysis
{
    public sealed class ScriptAnalyzerResultTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Populate_References_From_Provided_Script()
            {
                // Given
                var script = new ScriptInformation("./build.cake");
                script.References.Add("A.dll");
                var other = new ScriptInformation("./other.cake");
                other.References.Add("B.dll");
                script.Includes.Add(other);

                // When
                var result = new ScriptAnalyzerResult(script, new List<string>());

                // Then
                Assert.Equal(2, result.References.Count);
                Assert.Contains("A.dll", result.References);
                Assert.Contains("B.dll", result.References);
            }

            [Fact]
            public void Should_Populate_Namespaces_From_Provided_Script()
            {
                // Given
                var script = new ScriptInformation("./build.cake");
                script.Namespaces.Add("A.B");
                var other = new ScriptInformation("./other.cake");
                other.Namespaces.Add("C.D");
                script.Includes.Add(other);

                // When
                var result = new ScriptAnalyzerResult(script, new List<string>());

                // Then
                Assert.Equal(2, result.Namespaces.Count);
                Assert.Contains("A.B", result.Namespaces);
                Assert.Contains("C.D", result.Namespaces);
            }

            [Fact]
            public void Should_Populate_Using_Aliases_From_Provided_Script()
            {
                // Given
                var script = new ScriptInformation("./build.cake");
                script.UsingAliases.Add("using A = B;");
                var other = new ScriptInformation("./other.cake");
                other.UsingAliases.Add("using C = D;");
                script.Includes.Add(other);

                // When
                var result = new ScriptAnalyzerResult(script, new List<string>());

                // Then
                Assert.Equal(2, result.UsingAliases.Count);
                Assert.Contains("using A = B;", result.UsingAliases);
                Assert.Contains("using C = D;", result.UsingAliases);
            }

            [Fact]
            public void Should_Populate_Addins_From_Provided_Script()
            {
                // Given
                var script = new ScriptInformation("./build.cake");
                script.Addins.Add(new PackageReference("nuget:?package=First.Package"));
                var other = new ScriptInformation("./other.cake");
                other.Addins.Add(new PackageReference("nuget:?package=Second.Package"));
                script.Includes.Add(other);

                // When
                var result = new ScriptAnalyzerResult(script, new List<string>());

                // Then
                Assert.Equal(2, result.Addins.Count);
                Assert.Contains(result.Addins, package => package.OriginalString == "nuget:?package=First.Package");
                Assert.Contains(result.Addins, package => package.OriginalString == "nuget:?package=Second.Package");
            }

            [Fact]
            public void Should_Populate_Tools_From_Provided_Script()
            {
                // Given
                var script = new ScriptInformation("./build.cake");
                script.Tools.Add(new PackageReference("nuget:?package=First.Package"));
                var other = new ScriptInformation("./other.cake");
                other.Tools.Add(new PackageReference("nuget:?package=Second.Package"));
                script.Includes.Add(other);

                // When
                var result = new ScriptAnalyzerResult(script, new List<string>());

                // Then
                Assert.Equal(2, result.Tools.Count);
                Assert.Contains(result.Tools, package => package.OriginalString == "nuget:?package=First.Package");
                Assert.Contains(result.Tools, package => package.OriginalString == "nuget:?package=Second.Package");
            }
        }
    }
}
