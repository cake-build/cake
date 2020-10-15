// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Frosting.Tests.Data
{
    public class DummyPackageInstaller : IPackageInstaller
    {
        public bool CanInstall(PackageReference package, PackageType type)
        {
            return true;
        }

        public IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path)
        {
            return new List<IFile>();
        }
    }
}
