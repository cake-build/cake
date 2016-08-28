// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Microsoft.DotNet.ProjectModel;

namespace Cake.Frosting.Cli.Project
{
    internal sealed class ProjectLocator
    {
        public ProjectContext GetProject(DirectoryPath path)
        {
            var contexts = ProjectContext.CreateContextForEachFramework(path.FullPath);
            foreach (var context in contexts)
            {
                foreach (var dependency in context.ProjectFile.Dependencies)
                {
                    if (dependency.Name == "Cake.Frosting")
                    {
                        return context;
                    }
                }
            }
            return null;
        }
    }
}
