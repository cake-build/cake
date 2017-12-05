using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Composition;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;

namespace Cake.Commands
{
    internal sealed class BootstrapCommand : ICommand
    {
        private readonly IScriptAnalyzer _analyzer;
        private readonly ICakeConfiguration _configuration;
        private readonly IScriptProcessor _processor;
        private readonly ICakeEnvironment _environment;

        // Delegate factory used by Autofac.
        public delegate BootstrapCommand Factory();

        public BootstrapCommand(
            IScriptAnalyzer analyzer,
            ICakeConfiguration configuration,
            IScriptProcessor processor,
            ICakeEnvironment environment)
        {
            _analyzer = analyzer;
            _configuration = configuration;
            _processor = processor;
            _environment = environment;
        }

        public bool Execute(CakeOptions options)
        {
            // Get the script path and the root.
            var path = options.Script.MakeAbsolute(_environment);
            var root = path.GetDirectory();

            // Analyze the script.
            var result = _analyzer.Analyze(path);
            if (!result.Succeeded)
            {
                var messages = string.Join("\n", result.Errors.Select(s => $"{root.GetRelativePath(s.File).FullPath}, line #{s.Line}: {s.Message}"));
                throw new AggregateException($"Bootstrapping failed for '{path}'.\n{messages}");
            }

            // Install modules.
            _processor.InstallModules(
                result.Modules,
                _configuration.GetModulePath(root, _environment));

            // Success.
            return true;
        }
    }
}
