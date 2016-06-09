// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Analysis
{
    internal sealed class ScriptAnalyzerContext : IScriptAnalyzerContext
    {
        private readonly Action<IScriptAnalyzerContext> _callback;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly Stack<ScriptInformation> _stack;
        private readonly List<string> _lines;
        private readonly HashSet<FilePath> _processedScripts;
        private ScriptInformation _root;
        private ScriptInformation _current;

        public IScriptInformation Script
        {
            get { return _current; }
        }

        public IReadOnlyList<string> Lines
        {
            get { return _lines; }
        }

        public ScriptAnalyzerContext(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeLog log,
            Action<IScriptAnalyzerContext> callback)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _callback = callback;
            _processedScripts = new HashSet<FilePath>(new PathComparer(_environment));
            _stack = new Stack<ScriptInformation>();
            _lines = new List<string>();
        }

        public void Analyze(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Get the absolute path to the script and make
            // sure that it exists.
            path = path.IsRelative ? path.MakeAbsolute(_environment) : path;
            if (!_fileSystem.Exist(path))
            {
                const string format = "Could not find script '{0}'.";
                var message = string.Format(CultureInfo.InvariantCulture, format, path.FullPath);
                throw new CakeException(message);
            }

            if (_processedScripts.Contains(path))
            {
                _log.Debug("Script '{0}' has already been processed.", path.FullPath);
                return;
            }
            _processedScripts.Add(path);

            // Set as the current script.
            Push(path);

            // Analyze the script.
            _callback(this);

            // Pop the current script.
            Pop();
        }

        public void AddScriptLine(string line)
        {
            _lines.Add(line);
            _current.Lines.Add(line);
        }

        public void Push(FilePath path)
        {
            var script = new ScriptInformation(path);

            if (_root == null)
            {
                _root = script;
            }

            if (_current != null)
            {
                _current.Includes.Add(script);
            }

            _current = script;
            _stack.Push(_current);

            InsertLineDirective();
        }

        public void Pop()
        {
            _stack.Pop();
            if (_stack.Count > 0)
            {
                _current = _stack.Peek();

                InsertLineDirective();
            }
        }

        private void InsertLineDirective()
        {
            if (_current != null)
            {
                var position = Math.Max(1, _current.Lines.Count + 1);
                _lines.Add(string.Format(CultureInfo.InvariantCulture,
                    "#line {0} \"{1}\"", position, _current.Path.FullPath));
            }
        }
    }
}
