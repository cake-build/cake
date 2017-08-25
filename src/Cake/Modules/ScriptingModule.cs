﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Scripting.Roslyn;

namespace Cake.Modules
{
    internal sealed class ScriptingModule : ICakeModule
    {
        private readonly ICakeLog _log;
        private readonly CakeOptions _options;

        public ScriptingModule(CakeOptions options, ICakeLog log)
        {
            _log = log;
            _options = options ?? new CakeOptions();
        }

        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            if (_options.Mono)
            {
                _log.Warning("The Mono script engine has been removed so the expected behavior might differ. " +
                    "See Release Notes for Cake 0.22.0 for more information.");
            }

            registrar.RegisterType<RoslynScriptEngine>().As<IScriptEngine>().Singleton();
        }
    }
}