// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Core.Scripting.Analysis
{
    internal sealed class ScriptInformation : IScriptInformation
    {
        public FilePath Path { get; }
        public IList<IScriptInformation> Includes { get; }
        public IList<string> References { get; }
        public IList<string> UsingAliases { get; }
        public IList<string> Namespaces { get; }
        public IList<string> UsingStaticDirectives { get; }
        public IList<string> Defines { get; }
        public IList<PackageReference> Addins { get; }
        public IList<PackageReference> Tools { get; }
        public IList<PackageReference> Modules { get; }
        public IList<string> Lines { get; }

        public ScriptInformation(FilePath path)
        {
            Path = path;
            Includes = new List<IScriptInformation>();
            References = new List<string>();
            Namespaces = new List<string>();
            UsingAliases = new List<string>();
            UsingStaticDirectives = new List<string>();
            Defines = new List<string>();
            Addins = new List<PackageReference>();
            Tools = new List<PackageReference>();
            Modules = new List<PackageReference>();
            Lines = new List<string>();
        }
    }
}