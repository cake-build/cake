// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Cake.Testing;
using NSubstitute;
using Xunit;
using static VerifyXunit.Verifier;

namespace Cake.Core.Tests.Unit.Scripting;

public sealed class ScriptConventionsTests
{
    public sealed class TheGetDefaultDefinesMethod
    {
        [Theory]
        [InlineData("net10.0")]
        [InlineData("net11.0")]
        [InlineData("net12.0")]
        [InlineData("netstandard2.0")]
        public async Task Should_Return_Sdk_Style_Defines(string tfm)
        {
            // Given
            var conventions = CreateConventions(tfm);

            // When
            var defines = conventions.GetDefaultDefines();

            // Then
            await Verify(defines);
        }

        [Theory]
        [InlineData(8)]
        public async Task Should_Return_Cake_Or_Greater_Defines_From_Cake_7(int cakeMajor)
        {
            // Given
            var conventions = CreateConventions("net10.0", cakeMajor);

            // When
            var defines = conventions.GetDefaultDefines();

            // Then
            await Verify(defines);
        }

        private static ScriptConventions CreateConventions(string tfm, int cakeMajor = 7)
        {
            var (frameworkName, isCoreClr) = tfm switch
            {
                "net10.0" => (".NETCoreApp,Version=v10.0", true),
                "net11.0" => (".NETCoreApp,Version=v11.0", true),
                "net12.0" => (".NETCoreApp,Version=v12.0", true),
                "netstandard2.0" => (".NETStandard,Version=v2.0", true),
                _ => throw new ArgumentOutOfRangeException(nameof(tfm), tfm, null)
            };

            var environment = FakeEnvironment.CreateUnixEnvironment();
            var fileSystem = new FakeFileSystem(environment);
            var runtime = new FakeRuntime
            {
                BuiltFramework = new FrameworkName(frameworkName),
                CakeVersion = new Version(cakeMajor, 0, 0),
                IsCoreClr = isCoreClr
            };

            return new ScriptConventions(
                fileSystem,
                Substitute.For<IAssemblyLoader>(),
                runtime,
                Substitute.For<IReferenceAssemblyResolver>());
        }
    }
}
