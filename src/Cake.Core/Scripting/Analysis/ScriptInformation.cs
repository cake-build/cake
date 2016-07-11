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
        private readonly FilePath _path;
        private readonly List<IScriptInformation> _includes;
        private readonly List<string> _references;
        private readonly List<string> _usingAliases;
        private readonly List<string> _namespaces;
        private readonly List<PackageReference> _addins;
        private readonly List<PackageReference> _tools;
        private readonly List<string> _lines;

        public FilePath Path
        {
            get { return _path; }
        }

        public IList<IScriptInformation> Includes
        {
            get { return _includes; }
        }

        public IList<string> References
        {
            get { return _references; }
        }

        public IList<string> UsingAliases
        {
            get { return _usingAliases; }
        }

        public IList<string> Namespaces
        {
            get { return _namespaces; }
        }

        public IList<PackageReference> Addins
        {
            get { return _addins; }
        }

        public IList<PackageReference> Tools
        {
            get { return _tools; }
        }

        public IList<string> Lines
        {
            get { return _lines; }
        }

        public ScriptInformation(FilePath path)
        {
            _path = path;
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
