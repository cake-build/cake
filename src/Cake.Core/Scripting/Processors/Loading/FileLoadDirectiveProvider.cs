// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Net;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors.Loading
{
    internal sealed class FileLoadDirectiveProvider : ILoadDirectiveProvider
    {
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;

        public FileLoadDirectiveProvider(IGlobber globber, ICakeLog log)
        {
            _globber = globber;
            _log = log;
        }

        public bool CanLoad(IScriptAnalyzerContext context, LoadReference reference)
        {
            return reference.Scheme != null && reference.Scheme.Equals("local", StringComparison.OrdinalIgnoreCase);
        }

        public void Load(IScriptAnalyzerContext context, LoadReference reference)
        {
            FilePath path = null;
            if (reference.Parameters.ContainsKey("path"))
            {
                if (reference.Parameters["path"].Count == 1)
                {
                    path = reference.Parameters["path"].FirstOrDefault();
                }
                else if (reference.Parameters["path"].Count > 1)
                {
                    throw new CakeException("Query string for #load contains more than one parameter 'path'.");
                }
            }

            if (path == null)
            {
                throw new CakeException("Query string for #load is missing parameter 'path'.");
            }

            // URL decode the path.
            path = new FilePath(WebUtility.UrlDecode(path.FullPath));

            // Get the current script path.
            var current = context.Current.Path.GetDirectory();
            path = path.MakeAbsolute(current);

            var expectedExtension = path.HasExtension ? path.GetExtension() : ".cake";
            var files = _globber
                .GetFiles(path.FullPath)
                .Where(file =>
                {
                  var extension = file.GetExtension();
                  return extension != null && (extension.Equals(".cake", StringComparison.OrdinalIgnoreCase) || extension.Equals(expectedExtension, StringComparison.OrdinalIgnoreCase));
                })
                .ToArray();

            if (files.Length == 0)
            {
                // No scripts found.
                _log.Warning("No scripts found at {0}.", path);
                return;
            }

            foreach (var file in files)
            {
                context.Analyze(file);
            }
        }
    }
}