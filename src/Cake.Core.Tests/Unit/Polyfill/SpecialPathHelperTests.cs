// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Polyfill;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.Polyfill;

public sealed class SpecialPathHelperTests
{
    public sealed class TheGetUnixFolderMethod
    {
        [Theory]
        [InlineData(SpecialPath.ProgramFiles)]
        [InlineData(SpecialPath.ProgramFilesX86)]
        public void Should_Return_Usr_Bin_For_Program_Files(SpecialPath path)
        {
            // When
            var result = SpecialPathHelper.GetUnixFolder(path, "/home/cake");

            // Then
            Assert.Equal("/usr/bin", result);
        }

        [Theory]
        [InlineData(SpecialPath.ApplicationData)]
        [InlineData(SpecialPath.LocalApplicationData)]
        public void Should_Return_Home_For_Application_Data(SpecialPath path)
        {
            // When
            var result = SpecialPathHelper.GetUnixFolder(path, "/home/cake");

            // Then
            Assert.Equal("/home/cake", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_Null_For_Application_Data_When_Home_Is_Missing(string home)
        {
            // When
            var result = SpecialPathHelper.GetUnixFolder(SpecialPath.ApplicationData, home);

            // Then
            Assert.Null(result);
        }

        [Theory]
        [InlineData(SpecialPath.Windows)]
        [InlineData(SpecialPath.CommonApplicationData)]
        public void Should_Return_Null_For_Unsupported_Unix_Paths(SpecialPath path)
        {
            // When
            var result = SpecialPathHelper.GetUnixFolder(path, "/home/cake");

            // Then
            Assert.Null(result);
        }
    }

    public sealed class TheGetFolderPathMethod
    {
        [Theory]
        [InlineData(SpecialPath.ProgramFiles)]
        [InlineData(SpecialPath.ProgramFilesX86)]
        public void Should_Return_Usr_Bin_For_Unix_Program_Files(SpecialPath path)
        {
            // Given
            var platform = new FakePlatform(PlatformFamily.Linux);

            // When
            var result = SpecialPathHelper.GetFolderPath(platform, path);

            // Then
            Assert.Equal("/usr/bin", result.FullPath);
        }

        [Theory]
        [InlineData(SpecialPath.Windows)]
        [InlineData(SpecialPath.CommonApplicationData)]
        public void Should_Throw_For_Unsupported_Unix_Paths(SpecialPath path)
        {
            // Given
            var platform = new FakePlatform(PlatformFamily.Linux);

            // When
            var result = Record.Exception(() => SpecialPathHelper.GetFolderPath(platform, path));

            // Then
            AssertEx.IsExceptionWithMessage<NotSupportedException>(
                result,
                $"The special path '{path}' is not supported.");
        }
    }
}
