// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    internal sealed class BreakDirectiveProcessor : LineProcessor
    {
        public BreakDirectiveProcessor(ICakeEnvironment environment)
            : base(environment)
        {
        }

        public override bool Process(IScriptAnalyzerContext context, string line, out string replacement)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            replacement = null;

            if (!line.Trim().Equals("#break", StringComparison.Ordinal))
            {
                return false;
            }

            replacement = @"if (System.Diagnostics.Debugger.IsAttached) { System.Diagnostics.Debugger.Break(); }";
            return true;
        }
    }
}
