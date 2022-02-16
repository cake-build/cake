// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ReleaseNotesTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Version_Is_Null()
            {
                // Given, When
                Version version = null;
                var result = Record.Exception(() => new ReleaseNotes(version, Enumerable.Empty<string>(), null));

                // Then
                AssertEx.IsArgumentNullException(result, "version");
            }

            [Fact]
            public void Should_Throw_If_SemVersion_Is_Null()
            {
                // Given, When
                SemVersion semVersion = null;
                var result = Record.Exception(() => new ReleaseNotes(semVersion, Enumerable.Empty<string>(), null));

                // Then
                AssertEx.IsArgumentNullException(result, "semVersion");
            }
        }
    }
}