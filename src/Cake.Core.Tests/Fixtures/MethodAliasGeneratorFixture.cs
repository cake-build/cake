// Licensed to the .NET Foundation under one or more agreements.
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
        private readonly MethodInfo[] _methods;

        public MethodAliasGeneratorFixture()
        {
            _methods = typeof(MethodAliasGeneratorData).GetMethods();
        }

        public string Generate(string name)
        {
            var method = _methods.SingleOrDefault(x => x.Name == name);
            return MethodAliasGenerator.Generate(method).NormalizeGeneratedCode();
        }
    }
}