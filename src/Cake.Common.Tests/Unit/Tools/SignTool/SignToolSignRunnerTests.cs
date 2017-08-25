// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.SignTool;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SignTool
{
    public sealed class SignToolSignRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.ProcessRunner = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "processRunner");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Paths_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.AssemblyPaths = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Assembly_Do_Not_Exist()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.FileSystem.GetFile("/Working/a.dll").Delete();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: The assembly '/Working/a.dll' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_No_Timestamp_Server_URL_Has_Been_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.TimeStampUri = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Timestamp server URL is required but not specified.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_Path_And_Thumbprint_And_Subject_Name_Are_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;
                fixture.Settings.CertThumbprint = null;
                fixture.Settings.CertSubjectName = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: One of Certificate path, Certificate thumbprint or Certificate subject name is required but neither are specified.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_Path_And_Thumbprint_Are_Both_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertThumbprint = "123";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Certificate path and Certificate thumbprint cannot be specified together.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_Path_And_Subject_Name_Are_Both_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertSubjectName = "abc";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Certificate path and Certificate subject name cannot be specified together.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_Thumbprint_And_Password_Are_Both_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;
                fixture.Settings.Password = "123";
                fixture.Settings.CertThumbprint = "123";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Certificate thumbprint and Password cannot be specified together.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_Subject_Name_And_Password_Are_Both_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;
                fixture.Settings.Password = "123";
                fixture.Settings.CertSubjectName = "abc";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: Certificate subject name and Password cannot be specified together.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Certificate_File_Do_Not_Exist()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.FileSystem.GetFile("/Working/cert.pfx").Delete();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SignTool SIGN: The certificate '/Working/cert.pfx' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Not_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.Password = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Use_Default_Tool_Path_If_None_Is_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/signtool.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Tool_Path_If_Specified()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.ToolPath = "/Working/other/signtool.exe";
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/other/signtool.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /f \"/Working/cert.pfx\" /p secret \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_CertPath_And_No_Password()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.Password = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /f \"/Working/cert.pfx\" \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_Description()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.Description = "DescriptionTest";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /f \"/Working/cert.pfx\" /p secret /d \"DescriptionTest\" \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_DescriptionUri()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.DescriptionUri = new Uri("https://example.com");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /f \"/Working/cert.pfx\" /p secret /du \"https://example.com/\" \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_Thumbprint()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;
                fixture.Settings.Password = null;
                fixture.Settings.CertThumbprint = "ThumbprintTest";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /sha1 \"ThumbprintTest\" \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_Subject_Name()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;
                fixture.Settings.Password = null;
                fixture.Settings.CertSubjectName = "SubjectNameTest";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /n \"SubjectNameTest\" \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_Sha256_Digest_Algorithm()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.DigestAlgorithm = SignToolDigestAlgorithm.Sha256;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /fd sha256 /t \"https://t.com/\" /f \"/Working/cert.pfx\" /p secret \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_RFC6131_Timestamp_Uri_And_Sha256_Timestamp_Algorithm()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.TimeStampDigestAlgorithm = SignToolDigestAlgorithm.Sha256;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /tr \"https://t.com/\" /td sha256 /f \"/Working/cert.pfx\" /p secret \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_Append_Signature()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.AppendSignature = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /f \"/Working/cert.pfx\" /p secret /as \"/Working/a.dll\"", result.Args);
            }

            [Fact]
            public void Should_Call_Sign_Tool_With_Correct_Parameters_With_Thumbprint_And_Multiple_Assemblies()
            {
                // Given
                var fixture = new SignToolSignRunnerFixture();
                fixture.Settings.CertPath = null;
                fixture.Settings.Password = null;
                fixture.Settings.CertThumbprint = "ThumbprintTest";
                fixture.AssemblyPaths = new[]
                {
                    new FilePath("./a.dll"),
                    new FilePath("./foo/b.dll")
                };
                fixture.FileSystem.CreateFile("/Working/foo/b.dll");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("SIGN /t \"https://t.com/\" /sha1 \"ThumbprintTest\" \"/Working/a.dll\" \"/Working/foo/b.dll\"", result.Args);
            }
        }
    }
}