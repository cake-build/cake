using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public ArgumentParser(ICakeLog log, IFileSystem fileSystem)
        {
            _log = log;
            _fileSystem = fileSystem;
        }

        public CakeOptions Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            var options = new CakeOptions();

            var arguments = args.ToList();

            // If we don't have any arguments, search for a default script.
            if (arguments.Count == 0)
            {
                options.Script = GetDefaultScript();
            }

            var processedArguments = new List<Argument>();
            uint position = 0;
            foreach (var arg in arguments)
            {
                var value = arg.UnQuote();
                processedArguments.Add(IsOption(arg) ? ParseOption(arg, position) : new Argument { Key = string.Empty, Value = value, Position = position });
                position++;
            }

            foreach (var arg in processedArguments)
            {
                if (!ProcessOption(arg, options))
                {
                    return null;
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
            return arg[0] == '-';
        }

        private Argument ParseOption(string arg, uint position)
        {
            string name, value;

            var separatorIndex = arg.IndexOfAny(new[] { '=' });
            if (separatorIndex < 0)
            {
                name = arg.Substring(1);
                value = string.Empty;
            }
            else
            {
                name = arg.Substring(1, separatorIndex - 1);
                value = arg.Substring(separatorIndex + 1);
            }

            if (value.Length > 2)
            {
                if (value[0] == '\"' && value[value.Length - 1] == '\"')
                {
                    value = value.Substring(1, value.Length - 2);
                }
            }
            return new Argument { Key = name, Value = value, Position = position };
        }

        private bool ProcessOption(Argument argument, CakeOptions options)
        {
            if (string.IsNullOrWhiteSpace(argument.Key))
            {
                // If it's at position zero, it's either a file or target.
                switch (argument.Position)
                {
                    case 0:
                        // If we have a dot, we're a script file.
                        if (argument.Value.Contains("."))
                        {
                            options.Script = new FilePath(argument.Value);
                        }
                        else
                        {
                            options.Arguments.Add("Target", argument.Value);
                            options.Script = GetDefaultScript();
                        }
                        break;
                    case 1:
                        // If we have a dot, we're a script file.
                        if (argument.Value.Contains("."))
                        {
                            if (options.Script != null)
                            {
                                _log.Error("More than one build script specified.");
                                return false;
                            }

                            _log.Error("The script path must be first argument.");
                            return false;
                        }

                        if (options.Arguments.ContainsKey("Target"))
                        {
                            _log.Error("Attempted to add two targets: \"{0}\" and \"{1}\".", options.Arguments["Target"],
                                argument.Value);
                            return false;
                        }
                        options.Arguments.Add("Target", argument.Value);
                        break;
                    default:
                        _log.Error("Attempted to add unknown argument \"{0}\" at position {1}.", argument.Value,
                            argument.Position);
                        return false;
                }
            }
            else
            {
                switch (argument.Key.ToLowerInvariant())
                {
                    case "verbosity":
                    case "v":
                        // Parse verbosity.
                        var converter = TypeDescriptor.GetConverter(typeof(Verbosity));
                        var verbosity = converter.ConvertFromInvariantString(argument.Value);
                        if (verbosity != null)
                        {
                            options.Verbosity = (Verbosity)verbosity;
                        }
                        break;
                    case "showdescription":
                    case "s":
                        options.ShowDescription = true;
                        break;
                    case "dryrun":
                    case "noop":
                    case "whatif":
                        options.PerformDryRun = true;
                        break;
                    case "help":
                    case "?":
                        options.ShowHelp = true;
                        break;
                    case "version":
                    case "ver":
                        options.ShowVersion = true;
                        break;
                    default:
                        if (options.Arguments.ContainsKey(argument.Key))
                        {
                            _log.Error("Multiple arguments with the same name ({0}).", argument.Key);
                            return false;
                        }

                        options.Arguments.Add(argument.Key, argument.Key);
                        break;
                }
            }
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