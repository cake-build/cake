// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Scripting.Processors.Loading;
using Cake.Core.Tests.Fixtures;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.Processors
{
    public sealed class LoadDirectiveProcessorTests
    {
        [Theory]
        [InlineData("#l \"utils.cake\"")]
        [InlineData("#load \"utils.cake\"")]
        public void Should_Process_Single_Script_Reference_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.Providers.Add(new FileLoadDirectiveProvider());
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/utils.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(1, result.Script.Includes.Count);
            Assert.Equal("/Working/utils.cake", result.Script.Includes[0].Path.FullPath);
        }

        [Theory]
        [InlineData("#l \"utils.cake\"\n#l \"other.cake\"")]
        [InlineData("#load \"utils.cake\"\n#load \"other.cake\"")]
        public void Should_Process_Multiple_Script_References_Found_In_Source(string source)
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.Providers.Add(new FileLoadDirectiveProvider());
            fixture.GivenScriptExist("/Working/script.cake", source);
            fixture.GivenScriptExist("/Working/utils.cake", "Console.WriteLine();");
            fixture.GivenScriptExist("/Working/other.cake", "Console.WriteLine();");

            // When
            var result = fixture.Analyze("/Working/script.cake");

            // Then
            Assert.Equal(2, result.Script.Includes.Count);
            Assert.Equal("/Working/utils.cake", result.Script.Includes[0].Path.FullPath);
            Assert.Equal("/Working/other.cake", result.Script.Includes[1].Path.FullPath);
        }

        [Fact]
        public void Should_Insert_Line_Directives_When_Processing_Load_Directives()
        {
            // Given
            var fixture = new ScriptAnalyzerFixture();
            fixture.Providers.Add(new FileLoadDirectiveProvider());
            fixture.GivenScriptExist("/Working/a.cake", "int x=0;\n#l b.cake\nint y=2;");
            fixture.GivenScriptExist("/Working/b.cake", "int z=1;\n#l c.cake\nint p=4;");
            fixture.GivenScriptExist("/Working/c.cake", "int o=3;\n#r d.dll");

            // When
            var result = fixture.Analyze("/Working/a.cake");

            // Then
            Assert.Equal(13, result.Lines.Count);
            Assert.Equal(result.Lines[0], "#line 1 \"/Working/a.cake\"");
            Assert.Equal(result.Lines[1], "int x=0;");
            Assert.Equal(result.Lines[2], "#line 1 \"/Working/b.cake\"");
            Assert.Equal(result.Lines[3], "int z=1;");
            Assert.Equal(result.Lines[4], "#line 1 \"/Working/c.cake\"");
            Assert.Equal(result.Lines[5], "int o=3;");
            Assert.Equal(result.Lines[6], "// #r d.dll");
            Assert.Equal(result.Lines[7], "#line 2 \"/Working/b.cake\"");
            Assert.Equal(result.Lines[8], "// #l c.cake");
            Assert.Equal(result.Lines[9], "int p=4;");
            Assert.Equal(result.Lines[10], "#line 2 \"/Working/a.cake\"");
            Assert.Equal(result.Lines[11], "// #l b.cake");
            Assert.Equal(result.Lines[12], "int y=2;");
        }
    }
}
