// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors.Loading
{
    internal sealed class FileLoadDirectiveProvider : ILoadDirectiveProvider
    {
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

            // Get the current script path.
            var current = context.Current.Path.GetDirectory();
            path = path?.MakeAbsolute(current);

            // Analyze the script.
            context.Analyze(path);
        }
    }
}