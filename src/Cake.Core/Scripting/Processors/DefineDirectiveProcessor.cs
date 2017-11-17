// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class DefineDirectiveProcessor : LineProcessor
    {
        public override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            replacement = null;

            if (!line.StartsWith("#define", StringComparison.Ordinal))
            {
                return false;
            }

            context.Current.Defines.Add(line);
            return true;
        }
    }
}