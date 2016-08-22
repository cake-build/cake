﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Cake.Core.Scripting.CodeGen;
using Cake.Core.Tests.Data;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class MethodAliasGeneratorFixture
    {
        private readonly Assembly _assembly;
        private readonly MethodInfo[] _methods;

        public MethodAliasGeneratorFixture()
        {
            _assembly = typeof(MethodAliasGeneratorFixture).GetTypeInfo().Assembly;
            _methods = typeof(MethodAliasGeneratorData).GetMethods();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public string GetExpectedCode(string name)
        {
            var resource = string.Concat("Cake.Core.Tests.Unit.Scripting.CodeGen.Expected.Methods.", name);
            using (var stream = _assembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd().NormalizeGeneratedCode();
                }
            }
        }

        public string Generate(string name)
        {
            var method = _methods.SingleOrDefault(x => x.Name == name);
            return MethodAliasGenerator.Generate(method).NormalizeGeneratedCode();
        }
    }
}