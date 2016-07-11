// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Composition;
using Cake.Core.Scripting;
using Cake.Scripting.Mono;
using Cake.Scripting.Roslyn;
using Cake.Scripting.Roslyn.Nightly;
using Cake.Scripting.Roslyn.Stable;

namespace Cake.Modules
{
    internal sealed class ScriptingModule : ICakeModule
    {
        private readonly CakeOptions _options;

        public ScriptingModule(CakeOptions options)
        {
            _options = options ?? new CakeOptions();
        }

        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            // Are we running on Mono?
            var mono = _options.Mono;
            if (!mono)
            {
                mono = Type.GetType("Mono.Runtime") != null;
            }

            if (mono)
            {
                // Mono compiler
                registry.RegisterType<MonoScriptEngine>().As<IScriptEngine>().Singleton();
            }
            else
            {
                // Roslyn
                registry.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().Singleton();

                if (_options.PerformDebug)
                {
                    // Debug
                    registry.RegisterType<DebugRoslynScriptSessionFactory>().As<RoslynScriptSessionFactory>().Singleton();
                    registry.RegisterType<DebugRoslynNightlyScriptSessionFactory>().As<RoslynNightlyScriptSessionFactory>().Singleton();
                }
                else
                {
                    // Default
                    registry.RegisterType<DefaultRoslynScriptSessionFactory>().As<RoslynScriptSessionFactory>().Singleton();
                    registry.RegisterType<DefaultRoslynNightlyScriptSessionFactory>().As<RoslynNightlyScriptSessionFactory>().Singleton();
                }
            }
        }
    }
}
