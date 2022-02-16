// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Reflection;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;
using Cake.Core.Scripting.Processors.Loading;
using Cake.Core.Tooling;

namespace Cake.Core.Modules
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.Core assembly.
    /// </summary>
    public sealed class CoreModule : ICakeModule
    {
        /// <inheritdoc/>
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            // Execution
            registrar.RegisterType<CakeEngine>().As<ICakeEngine>().Singleton();
            registrar.RegisterType<CakeContext>().As<ICakeContext>().Singleton();
            registrar.RegisterType<CakeDataService>().As<ICakeDataResolver>().As<ICakeDataService>().Singleton();
            registrar.RegisterType<DefaultExecutionStrategy>().As<IExecutionStrategy>().Singleton();

            // Environment
            registrar.RegisterType<CakeEnvironment>().As<ICakeEnvironment>().Singleton();
            registrar.RegisterType<CakeRuntime>().As<ICakeRuntime>().Singleton();
            registrar.RegisterType<CakePlatform>().As<ICakePlatform>().Singleton();

            // IO
            registrar.RegisterType<FileSystem>().As<IFileSystem>().Singleton();
            registrar.RegisterType<Globber>().As<IGlobber>().Singleton();
            registrar.RegisterType<ProcessRunner>().As<IProcessRunner>().Singleton();
            registrar.RegisterType<NuGetToolResolver>().As<INuGetToolResolver>().Singleton();
            registrar.RegisterType<WindowsRegistry>().As<IRegistry>().Singleton();

            // Reflection
            registrar.RegisterType<AssemblyLoader>().As<IAssemblyLoader>().Singleton();
            registrar.RegisterType<AssemblyVerifier>().As<IAssemblyVerifier>().Singleton();

            // Tooling
            registrar.RegisterType<ToolRepository>().As<IToolRepository>().Singleton();
            registrar.RegisterType<ToolResolutionStrategy>().As<IToolResolutionStrategy>().Singleton();
            registrar.RegisterType<ToolLocator>().As<IToolLocator>().Singleton();

            // Scripting
            registrar.RegisterType<ScriptAliasFinder>().As<IScriptAliasFinder>().Singleton();
            registrar.RegisterType<ScriptAnalyzer>().As<IScriptAnalyzer>().Singleton();
            registrar.RegisterType<ScriptProcessor>().As<IScriptProcessor>().Singleton();
            registrar.RegisterType<ScriptConventions>().As<IScriptConventions>().Singleton();
            registrar.RegisterType<ScriptRunner>().As<IScriptRunner>().Singleton();

            // Load directive providers.
            registrar.RegisterType<FileLoadDirectiveProvider>().As<ILoadDirectiveProvider>().Singleton();
        }
    }
}