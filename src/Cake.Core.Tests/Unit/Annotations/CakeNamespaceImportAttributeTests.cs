// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting;
using Cake.Core.Tests.Fixtures;
using Cake.Testing;
using NSubstitute;
using Xunit;

[assembly: CakeNamespaceImport("assembly-level namespace import 1")]
[assembly: CakeNamespaceImport("assembly-level namespace import 2")]

namespace Cake.Core.Tests.Unit.Annotations
{
    [CakeNamespaceImport("class-level namespace import 1")]
    [CakeNamespaceImport("class-level namespace import 2")]
    public static class CakeNamespaceImportAttributeTests
    {
        [CakeMethodAlias]
        [CakeNamespaceImport("method-level namespace import 1")]
        [CakeNamespaceImport("method-level namespace import 2")]
        public static void DummyAlias(this ICakeContext context)
        {
        }

        [Theory]
        [InlineData("method")]
        [InlineData("class")]
        [InlineData("assembly")]
        public static void Imports_from_addin_into_session(string level)
        {
            var fixture = new ScriptRunnerFixture();
            fixture.AliasFinder = new ScriptAliasFinder(fixture.Log);

            // Necessary because the ScriptConventions class starts returning null in the assemblies list
            // when it uses a test double assembly loader and tries to load System.Runtime
            fixture.ScriptConventions = Substitute.For<IScriptConventions>();
            fixture.ScriptConventions.GetDefaultAssemblies(Arg.Any<DirectoryPath>()).Returns(Array.Empty<Assembly>());

            var thisAssembly = typeof(CakeNamespaceImportAttributeTests).GetTypeInfo().Assembly;
            var thisAssemblyPath = new FilePath(thisAssembly.Location);
            fixture.FileSystem.CreateFile(thisAssemblyPath);
            fixture.AssemblyLoader.Load(Arg.Is<FilePath>(v => v.FullPath == thisAssemblyPath.FullPath), Arg.Any<bool>()).Returns(thisAssembly);

            fixture.ScriptProcessor.InstallAddins(Arg.Any<IReadOnlyCollection<PackageReference>>(), Arg.Any<DirectoryPath>())
                .Returns(new[] { thisAssemblyPath });

            fixture.CreateScriptRunner().Run(fixture.Host, fixture.Script, fixture.ArgumentDictionary);

            fixture.Session.Received().ImportNamespace($"{level}-level namespace import 1");
            fixture.Session.Received().ImportNamespace($"{level}-level namespace import 2");
        }

        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Namespace_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new CakeNamespaceImportAttribute((string)null));

                // Then
                AssertEx.IsArgumentNullException(result, "namespace");
            }
        }

        public sealed class TheType
        {
            private readonly AttributeUsageAttribute _attributeUsage;

            public TheType()
            {
                // Given, When
                _attributeUsage = (AttributeUsageAttribute)typeof(CakeNamespaceImportAttribute).GetTypeInfo().GetCustomAttribute(typeof(AttributeUsageAttribute));
            }

            [Fact]
            public void Should_Be_Able_To_Target_Method()
            {
                // Then
                Assert.True(_attributeUsage.ValidOn.HasFlag(AttributeTargets.Method));
            }

            [Fact]
            public void Should_Be_Able_To_Target_Class()
            {
                // Then
                Assert.True(_attributeUsage.ValidOn.HasFlag(AttributeTargets.Class));
            }

            [Fact]
            public void Should_Be_Able_To_Target_Assembly()
            {
                // Then
                Assert.True(_attributeUsage.ValidOn.HasFlag(AttributeTargets.Assembly));
            }
        }
    }
}