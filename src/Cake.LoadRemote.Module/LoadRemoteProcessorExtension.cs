﻿using System;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

namespace Cake.LoadRemote.Module
{
    public sealed class LoadRemoteProcessorExtension : ProcessorExtension
    {
        readonly Regex _matchDirectiveName = new Regex("^(#load|#l)", RegexOptions.Compiled);
        readonly UriDirectiveProcessor _processor;
        public override IScriptRunnerExtension ScriptRunnerExtension { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadRemoteProcessorExtension"/> class.
        /// </summary>
        /// <param name="cakeContext">The <see cref="ICakeContext"/>.</param>
        /// <param name="environment">The <see cref="ICakeEnvironment"/>.</param>
        /// <param name="cakeLog">The <see cref="ICakeLog"/>.</param>
        /// <param name="scriptProcessor">The <see cref="IScriptProcessor"/>.</param>
        public LoadRemoteProcessorExtension(ICakeContext cakeContext, ICakeEnvironment environment, ICakeLog cakeLog, IScriptProcessor scriptProcessor) : base(cakeContext, environment, cakeLog, scriptProcessor)
        {
            _processor = new LoadRemoteProcessor(this, environment);
            ScriptRunnerExtension = new LoadRemoteScriptRunnerExtension(this, cakeContext, environment, cakeLog, scriptProcessor);
        }

        public override bool Process(IScriptAnalyzerContext analyzer, string line, out string replacement)
        {
            // How to Support both "#l" and "#load", replace to Constants.DirectiveName and process it with the LoadRemoteProcessor.
            line = line.Trim();
            line = _matchDirectiveName.Replace(line, Constants.DirectiveName);

            // Set the replacement line to the modified line.
            replacement = string.Concat("// ", line);

            // We need a out value to call the processor.
            string outValue;

            // This is a nuget reference use the NugetScript processor.
            return _processor.Process(analyzer, line, out outValue);
        }

        public override bool CanProcessDirective(string alias, string value)
        {
            if ((alias.Equals("#l", StringComparison.Ordinal) || alias.Equals("#load", StringComparison.Ordinal)) && value.Contains("nuget:"))
            {
                return true;
            }

            return false;
        }
    }
}
