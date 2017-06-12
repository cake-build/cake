// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools.DupFinder;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DupFinder
{
    public sealed class DupFinderRunnerTests
    {
        public sealed class TheRunMethodWithFiles
        {
            [Fact]
            public void Should_Throw_If_Files_Are_Null()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.FilePaths = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "filePaths");
            }

            [Fact]
            public void Should_Find_Inspect_Code_Runner()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/dupfinder.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("DupFinder: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("DupFinder: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Use_Provided_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.FilePaths.Clear();
                fixture.FilePaths.Add(new FilePath("./Test.sln"));
                fixture.FilePaths.Add(new FilePath("./Test.csproj"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test.sln\" \"/Working/Test.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Set_Output_File()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.OutputFile = new FilePath("build/dupfinder.xml");

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/output=/Working/build/dupfinder.xml\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_OutputFile_Contains_Duplicates_And_Set_To_Throw()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.OutputFile = new FilePath("build/duplicates.xml");
                fixture.Settings.ThrowExceptionOnFindingDuplicates = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Duplicates found in code base.");
            }

            [Fact]
            public void Should_Set_Debug_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.Debug = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/debug \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Discard_Cost()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.DiscardCost = 50;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/discard-cost=50 \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Discard_Fields_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.DiscardFieldsName = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/discard-fields \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Discard_Literals_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.DiscardLiterals = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/discard-literals \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Discard_Local_Vars_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.DiscardLocalVariablesName = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/discard-local-vars \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Discard_Types_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.DiscardTypes = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/discard-types \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Idle_Priority_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.IdlePriority = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/idle-priority \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Exclude_By_Comment()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.ExcludeFilesByStartingCommentSubstring = new[] { "test", "test1" };

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/exclude-by-comment=test;test1\" \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Exclude_Code_Regions()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.ExcludeCodeRegionsByNameSubstring = new[] { "generated code", "test" };

                // Then
                var restult = fixture.Run();

                // Then
                Assert.Equal("\"/exclude-code-regions=generated code;test\" " +
                             "\"/Working/Test.sln\"", restult.Args);
            }

            [Fact]
            public void Should_Set_Exclude_Pattern()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.ExcludePattern = new[] { "*Tests.cs", "*Test.cs" };

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/exclude=*Tests.cs;*Test.cs\" \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_MSBuild_Properties()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.MsBuildProperties = new Dictionary<string, string>();
                fixture.Settings.MsBuildProperties.Add("TreatWarningsAsErrors", "true");
                fixture.Settings.MsBuildProperties.Add("Optimize", "false");

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/properties:TreatWarningsAsErrors=true\" " +
                             "\"/properties:Optimize=false\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Normalize_Types_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.NormalizeTypes = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/normalize-types \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Caches_Home()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.CachesHome = DirectoryPath.FromString("caches/");

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/caches-home=/Working/caches\" \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Show_Stats_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.ShowStats = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/show-stats \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Show_Text_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Settings.ShowText = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("/show-text \"/Working/Test.sln\"", result.Args);
            }
        }

        public sealed class TheRunFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Config_File_Is_Null()
            {
                // Given
                var fixture = new DupFinderRunnerConfigFixture();
                fixture.ConfigPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "configFile");
            }

            [Fact]
            public void Should_Use_Provided_Config_File()
            {
                // Given
                var fixture = new DupFinderRunnerConfigFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/config=/Working/Config.xml\"", result.Args);
            }
        }
    }
}