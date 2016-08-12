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
        private readonly List<IScriptInformation> _includes;
        private readonly List<string> _references;
        private readonly List<string> _usingAliases;
        private readonly List<string> _namespaces;
        private readonly List<PackageReference> _addins;
        private readonly List<PackageReference> _tools;
        private readonly List<string> _lines;

        public FilePath Path { get; }

        public IList<IScriptInformation> Includes => _includes;

        public IList<string> References => _references;

        public IList<string> UsingAliases => _usingAliases;

        public IList<string> Namespaces => _namespaces;

        public IList<PackageReference> Addins => _addins;

        public IList<PackageReference> Tools => _tools;

        public IList<string> Lines => _lines;

        public ScriptInformation(FilePath path)
        {
            Path = path;
            _includes = new List<IScriptInformation>();
            _references = new List<string>();
            _namespaces = new List<string>();
            _usingAliases = new List<string>();
            _addins = new List<PackageReference>();
            _tools = new List<PackageReference>();
            _lines = new List<string>();
        }
    }
}