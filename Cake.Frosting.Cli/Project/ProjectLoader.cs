// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.Loader;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting.Cli.Reflection;
using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.DependencyModel;

namespace Cake.Frosting.Cli.Project
{
    internal sealed class ProjectLoader
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public ProjectLoader(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        public Assembly Load(ProjectContext context)
        {
            var root = _environment.WorkingDirectory;
            var binaryPath = root.Combine("bin").Combine("Release").Combine(context.TargetFramework.GetShortFolderName());
            var assemblyPath = binaryPath.CombineWithFilePath(context.ProjectFile.Name + ".dll").MakeAbsolute(_environment);

            Console.WriteLine("Loading ./{0}...", root.GetRelativePath(assemblyPath).FullPath);
            var loader = new AssemblyLoader(_fileSystem, assemblyPath.GetDirectory());
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath.FullPath);
            DependencyContext.Default.Merge(DependencyContext.Load(assembly));
            foreach (var reference in assembly.GetReferencedAssemblies())
            {
                try
                {
                    loader.LoadFromAssemblyName(reference);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Assembly.Load(reference);
                }
            }

            return assembly;
        }
    }
}
