using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Arguments
{
    internal sealed class ArgumentParser : IArgumentParser
    {
        private readonly ICakeLog _log;
        private readonly IFileSystem _fileSystem;
        private readonly VerbosityParser _verbosityParser;

        public ArgumentParser(ICakeLog log, IFileSystem fileSystem)
        {
            _log = log;
            _fileSystem = fileSystem;
            _verbosityParser = new VerbosityParser();
        }

        public CakeOptions Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var options = new CakeOptions();
            var isParsingOptions = false;

            var arguments = args.ToList();

            // If we don't have any arguments, search for a default script.
            if (arguments.Count == 0)
            {
                options.Script = GetDefaultScript();
            }

            foreach (var arg in arguments)
            {
                var value = arg.UnQuote();

                if (isParsingOptions)
                {
                    if (IsOption(value))
                    {
                        if (!ParseOption(value, options))
                        {
                            return null;
                        }
                    }
                    else
                    {
                        _log.Error("More than one build script specified.");
                        return null;
                    }
                }
                else
                {
                    try
                    {
                        // If they didn't provide a specific build script, search for a default.
                        if (IsOption(arg))
                        {
                            // Make sure we parse the option
                            if (!ParseOption(value, options))
                            {
                                return null;
                            }

                            options.Script = GetDefaultScript();
                            continue;
                        }

                        // Quoted?
                        options.Script = new FilePath(value);
                    }
                    finally
                    {
                        // Start parsing options.
                        isParsingOptions = true;
                    }
                }
            }

            return options;
        }

        private static bool IsOption(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }
            return arg.StartsWith("--") || arg.StartsWith("-");
        }

        private bool ParseOption(string arg, CakeOptions options)
        {
            string name, value;

            var nameIndex = arg.StartsWith("--") ? 2 : 1;
            var separatorIndex = arg.IndexOfAny(new[] { '=' });
            if (separatorIndex < 0)
            {
                name = arg.Substring(nameIndex);
                value = string.Empty;
            }
            else
            {
                name = arg.Substring(nameIndex, separatorIndex - nameIndex);
                value = arg.Substring(separatorIndex + 1);
            }

            if (value.Length > 2)
            {
                if (value[0] == '\"' && value[value.Length - 1] == '\"')
                {
                    value = value.Substring(1, value.Length - 2);
                }
            }

            return ParseOption(name, value, options);
        }

        private bool ParseOption(string name, string value, CakeOptions options)
        {
            if (name.Equals("verbosity", StringComparison.OrdinalIgnoreCase)
                || name.Equals("v", StringComparison.OrdinalIgnoreCase))
            {
                // Parse verbosity.
                options.Verbosity = _verbosityParser.Parse(value);
            }

            if (name.Equals("showdescription", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("s", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowDescription = true;
            }

            if (name.Equals("dryrun", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("noop", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("whatif", StringComparison.OrdinalIgnoreCase))
            {
                options.PerformDryRun = true;
            }

            if (name.Equals("help", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("?", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowHelp = true;
            }

            if (name.Equals("version", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("ver", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowVersion = true;
            }

            if (options.Arguments.ContainsKey(name))
            {
                _log.Error("Multiple arguments with the same name ({0}).", name);
                return false;
            }

            options.Arguments.Add(name, value);
            return true;
        }

        private readonly string[] _defaultScriptNameConventions =
        {
            "build.cake",
            "default.cake",
            "bake.cake",
            ".cakefile"
        };

        private FilePath GetDefaultScript()
        {
            _log.Verbose("Searching for default build script...");

            // Search for default cake scripts in order
            foreach (var defaultScriptNameConvention in _defaultScriptNameConventions)
            {
                var currentFile = new FilePath(defaultScriptNameConvention);
                var file = _fileSystem.GetFile(currentFile);
                if (file != null && file.Exists)
                {
                    _log.Verbose("Found default build script: {0}", defaultScriptNameConvention);
                    return currentFile;
                }
            }
            return null;
        }
    }
}