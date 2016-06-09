// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Composition;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Tooling;

namespace Cake.Core.Modules
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.Core assembly.
    /// </summary>
    public sealed class CoreModule : ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registry.
        /// </summary>
        /// <param name="registry">The container registry.</param>
        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            registry.RegisterType<CakeEngine>().As<ICakeEngine>().Singleton();
            registry.RegisterType<CakeContext>().As<ICakeContext>().Singleton();

            // IO
            registry.RegisterType<FileSystem>().As<IFileSystem>().Singleton();
            registry.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().Singleton();
            registry.RegisterType<Globber>().As<IGlobber>().Singleton();
            registry.RegisterType<ProcessRunner>().As<IProcessRunner>().Singleton();
            registry.RegisterType<NuGetToolResolver>().As<INuGetToolResolver>().Singleton();
            registry.RegisterType<WindowsRegistry>().As<IRegistry>().Singleton();

            // Tooling
            registry.RegisterType<ToolRepository>().As<IToolRepository>().Singleton();
            registry.RegisterType<ToolResolutionStrategy>().As<IToolResolutionStrategy>().Singleton();
            registry.RegisterType<ToolLocator>().As<IToolLocator>().Singleton();

            // Scripting
            registry.RegisterType<ScriptAliasFinder>().As<IScriptAliasFinder>().Singleton();
            registry.RegisterType<ScriptAnalyzer>().As<IScriptAnalyzer>().Singleton();
            registry.RegisterType<ScriptProcessor>().As<IScriptProcessor>().Singleton();
            registry.RegisterType<ScriptConventions>().As<IScriptConventions>().Singleton();
            registry.RegisterType<ScriptRunner>().As<IScriptRunner>().Singleton();
        }
    }
}
