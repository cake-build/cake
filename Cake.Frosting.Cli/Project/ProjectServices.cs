// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Frosting.Cli.Project
{
    internal sealed class ProjectServices
    {
        public ProjectBuilder Builder { get; }

        public ProjectLoader Loader { get; }

        public ProjectLocator Locator { get; }

        public ProjectServices(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            Builder = new ProjectBuilder();
            Loader = new ProjectLoader(fileSystem, environment);
            Locator = new ProjectLocator();
        }
    }
}
