// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetLoadDirectiveProviderFixtureResult
    {
        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }
        public List<FilePath> AnalyzedFiles { get; set; }

        public NuGetLoadDirectiveProviderFixtureResult()
        {
            AnalyzedFiles = new List<FilePath>();
        }
    }
}