// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.SpecFlow.StepDefinitionReport;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SpecFlow.StepDefinitionReport
{
    public sealed class SpecFlowStepDefinitionReporterTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Project_File_Is_Null()
            {
                // Given
                var fixture = new SpecFlowStepDefinitionReporterFixture();
                fixture.ProjectFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new SpecFlowStepDefinitionReporterFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Append_Out()
            {
                // Given
                var fixture = new SpecFlowStepDefinitionReporterFixture();
                fixture.Settings.Out = "/Working/out.html";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("stepdefinitionreport \"/Working/Tests.csproj\" " +
                             "/out:\"/Working/out.html\"", result.Args);
            }

            [Fact]
            public void Should_Append_XsltFile()
            {
                // Given
                var fixture = new SpecFlowStepDefinitionReporterFixture();
                fixture.Settings.XsltFile = "/Working/template.xslt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("stepdefinitionreport \"/Working/Tests.csproj\" " +
                             "/xsltFile:\"/Working/template.xslt\"", result.Args);
            }

            [Fact]
            public void Should_Append_BinFolder()
            {
                // Given
                var fixture = new SpecFlowStepDefinitionReporterFixture();
                fixture.Settings.BinFolder = "/Working/some-folder";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("stepdefinitionreport \"/Working/Tests.csproj\" " +
                             "/binFolder:\"/Working/some-folder\"", result.Args);
            }
        }
    }
}