// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.Scripting.CodeGen;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting.CodeGen
{
    public sealed class ParameterFormatterTests
    {
        private readonly ParameterFormatter _parameterFormatter = new ParameterFormatter();

        [Fact]
        public void Should_Throw_If_Parameter_Info_Is_Null()
        {
            // When
            var result = Record.Exception(() => _parameterFormatter.FormatName((ParameterInfo)null));

            // Then
            AssertEx.IsArgumentNullException(result, "parameterInfo");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("   ")]
        public void Should_Throw_If_Parameter_Name_Is_Null_Or_Whitespace(string arg)
        {
            // When
            var result = Record.Exception(() => _parameterFormatter.FormatName(arg));

            // Then
            AssertEx.IsArgumentException(result, "parameterName", "Parameter name cannot be null or whitespace");
        }

        [Theory]
        [InlineData("abstract", "@abstract")]
        [InlineData("as", "@as")]
        [InlineData("base", "@base")]
        [InlineData("bool", "@bool")]
        [InlineData("break", "@break")]
        [InlineData("byte", "@byte")]
        [InlineData("case", "@case")]
        [InlineData("catch", "@catch")]
        [InlineData("char", "@char")]
        [InlineData("checked", "@checked")]
        [InlineData("class", "@class")]
        [InlineData("const", "@const")]
        [InlineData("continue", "@continue")]
        [InlineData("decimal", "@decimal")]
        [InlineData("default", "@default")]
        [InlineData("delegate", "@delegate")]
        [InlineData("do", "@do")]
        [InlineData("double", "@double")]
        [InlineData("else", "@else")]
        [InlineData("enum", "@enum")]
        [InlineData("event", "@event")]
        [InlineData("explicit", "@explicit")]
        [InlineData("extern", "@extern")]
        [InlineData("false", "@false")]
        [InlineData("finally", "@finally")]
        [InlineData("fixed", "@fixed")]
        [InlineData("float", "@float")]
        [InlineData("for", "@for")]
        [InlineData("foreach", "@foreach")]
        [InlineData("goto", "@goto")]
        [InlineData("if", "@if")]
        [InlineData("implicit", "@implicit")]
        [InlineData("in", "@in")]
        [InlineData("int", "@int")]
        [InlineData("interface", "@interface")]
        [InlineData("internal", "@internal")]
        [InlineData("is", "@is")]
        [InlineData("lock", "@lock")]
        [InlineData("long", "@long")]
        [InlineData("namespace", "@namespace")]
        [InlineData("new", "@new")]
        [InlineData("null", "@null")]
        [InlineData("object", "@object")]
        [InlineData("operator", "@operator")]
        [InlineData("out", "@out")]
        [InlineData("override", "@override")]
        [InlineData("params", "@params")]
        [InlineData("private", "@private")]
        [InlineData("protected", "@protected")]
        [InlineData("public", "@public")]
        [InlineData("readonly", "@readonly")]
        [InlineData("ref", "@ref")]
        [InlineData("return", "@return")]
        [InlineData("sbyte", "@sbyte")]
        [InlineData("sealed", "@sealed")]
        [InlineData("short", "@short")]
        [InlineData("sizeof", "@sizeof")]
        [InlineData("stackalloc", "@stackalloc")]
        [InlineData("static", "@static")]
        [InlineData("string", "@string")]
        [InlineData("struct", "@struct")]
        [InlineData("switch", "@switch")]
        [InlineData("this", "@this")]
        [InlineData("throw", "@throw")]
        [InlineData("true", "@true")]
        [InlineData("try", "@try")]
        [InlineData("typeof", "@typeof")]
        [InlineData("uint", "@uint")]
        [InlineData("ulong", "@ulong")]
        [InlineData("unchecked", "@unchecked")]
        [InlineData("unsafe", "@unsafe")]
        [InlineData("ushort", "@ushort")]
        [InlineData("using", "@using")]
        [InlineData("virtual", "@virtual")]
        [InlineData("void", "@void")]
        [InlineData("volatile", "@volatile")]
        [InlineData("while", "@while")]
        public void Should_Format_Reserved_Keywords_Correctly(string parameterName, string expectedParameterName)
        {
            // When
            var result = _parameterFormatter.FormatName(parameterName);

            // Then
            Assert.Equal(expectedParameterName, result);
        }

        [Theory]
        [InlineData("testParameter", "testParameter")]
        public void Should_Format_Variable_Names_Correctly(string parameterName, string expectedParameterName)
        {
            // When
            var result = _parameterFormatter.FormatName(parameterName);

            // Then
            Assert.Equal(expectedParameterName, result);
        }
    }
}
