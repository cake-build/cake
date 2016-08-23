// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.Scripting;

#if NETCORE
using Cake.Scripting.XPlat;
#else
using Cake.Scripting.Mono;
using Cake.Scripting.Roslyn;
using Cake.Scripting.Roslyn.Nightly;
using Cake.Scripting.Roslyn.Stable;
#endif

namespace Cake.Modules
{
    internal sealed class ScriptingModule : ICakeModule
    {
        private readonly CakeOptions _options;

        public ScriptingModule(CakeOptions options)
        {
            _options = options ?? new CakeOptions();
        }

        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

#if NETCORE
            registrar.RegisterType<XPlatScriptEngine>().As<IScriptEngine>().Singleton();
#else
            // Are we running on Mono?
            var mono = _options.Mono;
            if (!mono)
            {
                mono = Type.GetType("Mono.Runtime") != null;
            }

            if (mono)
            {
                // Mono compiler
                registrar.RegisterType<MonoScriptEngine>().As<IScriptEngine>().Singleton();
            }
            else
            {
                // Roslyn
                registrar.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().Singleton();

                if (_options.PerformDebug)
                {
                    // Debug
                    registrar.RegisterType<DebugRoslynScriptSessionFactory>().As<RoslynScriptSessionFactory>().Singleton();
                    registrar.RegisterType<DebugRoslynNightlyScriptSessionFactory>().As<RoslynNightlyScriptSessionFactory>().Singleton();
                }
                else
                {
                    // Default
                    registrar.RegisterType<DefaultRoslynScriptSessionFactory>().As<RoslynScriptSessionFactory>().Singleton();
                    registrar.RegisterType<DefaultRoslynNightlyScriptSessionFactory>().As<RoslynNightlyScriptSessionFactory>().Singleton();
                }
            }
#endif
        }
    }
}