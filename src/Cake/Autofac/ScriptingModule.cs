using System;
using Autofac;
using Cake.Core.Scripting;
using Cake.Scripting.Mono;
using Cake.Scripting.Roslyn;
using Cake.Scripting.Roslyn.Nightly;
using Cake.Scripting.Roslyn.Stable;

namespace Cake.Autofac
{
    internal sealed class ScriptingModule : Module
    {
        private readonly CakeOptions _options;

        public ScriptingModule(CakeOptions options)
        {
            _options = options ?? new CakeOptions();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Are we running on Mono?
            var mono = Type.GetType("Mono.Runtime") != null;
            if (!mono)
            {
                // Not using the mono compiler, but do we want to?
                if (_options.Arguments.ContainsKey("mono"))
                {
                    mono = true;
                }
            }

            if (mono)
            {
                // Mono compiler
                builder.RegisterType<MonoScriptEngine>().As<IScriptEngine>().SingleInstance();
            }
            else
            {
                // Roslyn
                builder.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().SingleInstance();
                builder.RegisterType<RoslynScriptSessionFactory>().SingleInstance();
                builder.RegisterType<RoslynNightlyScriptSessionFactory>().SingleInstance();
            }
        }
    }
}
