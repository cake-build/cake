// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Microsoft.Extensions.Time.Testing;

namespace Cake.Testing.Tests.Fixtures
{
    public sealed class FakeFileSystemFixture
    {
        private static readonly DateTimeOffset StartDateTimeOffset = new (2014, 5, 27, 19, 56, 1, TimeSpan.FromHours(2));
        private static readonly DateTimeOffset TestCreationDateTimeOffset = new (2025, 10, 25, 20, 33, 4, TimeSpan.FromHours(2));
        private static readonly DateTimeOffset TestLastWriteDateTimeOffset = new (2025, 10, 25, 21, 34, 5, TimeSpan.FromHours(2));
        private static readonly DateTimeOffset TestLastAccessDateTimeOffset = new (2025, 10, 25, 22, 35, 6, TimeSpan.FromHours(2));

        public FakeFileSystem FileSystem { get; }
        public FakeEnvironment Environment { get; }
        public FakeTimeProvider TimeProvider { get; }
        public DateTime TestCreationDateTimeUtc { get; }
        public DateTime TestLastWriteDateTimeUtc { get; }
        public DateTime TestLastAccessDateTimeUtc { get; }

        public FakeFileSystemFixture(PlatformFamily platformFamily = PlatformFamily.Windows)
        {
            TestCreationDateTimeUtc = TestCreationDateTimeOffset.UtcDateTime;
            TestLastWriteDateTimeUtc = TestLastWriteDateTimeOffset.UtcDateTime;
            TestLastAccessDateTimeUtc = TestLastAccessDateTimeOffset.UtcDateTime;

            Environment = new FakeEnvironment(platformFamily)
            {
                WorkingDirectory = new DirectoryPath("/")
            };

            var fakeTimeProvider = new FakeTimeProvider(StartDateTimeOffset);
            fakeTimeProvider.SetLocalTimeZone(TimeZoneInfo.CreateCustomTimeZone(nameof(FakeFileSystemFixture), TimeSpan.FromHours(2), nameof(FakeFileSystemFixture), nameof(FakeFileSystemFixture)));
            TimeProvider = fakeTimeProvider;

            FileSystem = new FakeFileSystem(Environment)
            {
                TimeProvider = TimeProvider
            };

            FileSystem.CreateDirectory("/test/build");
            FileSystem.CreateFile("/test/file1.txt")
                .SetContent("Test content");
            FileSystem.CreateFile("/test/build/build.cake")
                .SetContent(
                """
                Information("Hello world");
                """);
            FileSystem.CreateFile("/test/build/helloworld.xml")
                .SetContent(
                """
                <Hello>World</Hello>
                """);
            FileSystem.CreateFile("/test/build/global.json")
                .SetContent(
                """
                { "sdk": { "version": "9.0.306", "rollForward": "latestFeature" } }
                """);
            FileSystem.CreateFile("/test/build/BuildPackage.cs")
                .SetContent("public record BuildPackage(string Id, FilePath PackagePath);");
        }

        public static FakeFileSystemFixture Create(PlatformFamily platformFamily)
        {
            return new FakeFileSystemFixture(platformFamily);
        }
    }
}
