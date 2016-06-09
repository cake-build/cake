// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Scripting.Mono.CodeGen.Parsing;
using Xunit;

namespace Cake.Tests.Unit.Scripting.Mono
{
    public sealed class ScriptParserTests
    {
        public sealed class TheGetBlockMethod
        {
            public void Should_Parse_Statement_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("int nop = 0;", block.Content);
                Assert.True(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Task_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("Task(\"A\").Does(() => { int nop = 0; }); int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("Task(\"A\").Does(() => { int nop = 0; });", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_While_Loop_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("while(true) { /* Do magic */ } \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("while(true) { /* Do magic */ }", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_If_While_Loop_Without_Braces_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("while(true) foo++; \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("while(true) foo++;", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_If_Statement_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("if(true) { /* Do magic */ } \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("if(true) { /* Do magic */ }", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_If_Statement_Without_Braces_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("if(true) var foo = 1; \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("if(true) var foo = 1;", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Else_Statement_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("else { /* Do magic */ } \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("else { /* Do magic */ }", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Else_Statement_Without_Braces_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("else var foo = 1; \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("else var foo = 1;", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Else_If_Statement_Without_Braces_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("else if (true) var foo = 1; \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("else if (true) var foo = 1;", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Else_If_Statement_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("else if(true) { /* Do magic */ } \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("else if(true) { /* Do magic */ }", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Switch_Statement_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("switch(a) { case 1: return 0; case 2: return 1; } \n int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("switch(a) { case 1: return 0; case 2: return 1; }", block.Content);
                Assert.False(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Class_As_Block_With_Scope()
            {
                // Given
                var parser = new ScriptParser("public class Foo { } int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("public class Foo { }", block.Content);
                Assert.True(block.HasScope);
            }

            [Fact]
            public void Should_Parse_Array_Initializer_As_Block_Without_Scope()
            {
                // Given
                var parser = new ScriptParser("int[] temp = new { 1 }; int nop = 0;");

                // When
                var block = parser.ParseNext();

                // Then
                Assert.Equal("int[] temp = new { 1 };", block.Content);
                Assert.False(block.HasScope);
            }
        }
    }
}
