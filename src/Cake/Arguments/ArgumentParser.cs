// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
        private readonly VerbosityParser _verbosityParser;

        public ArgumentParser(ICakeLog log, VerbosityParser parser)
        {
            _log = log;
            _verbosityParser = parser;
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
            if (arguments.Count == 0)
            {
                // If we don't have any arguments, set a default script.
                options.Script = "./build.cake";
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
                            options.HasError = true;
                            return options;
                        }
                    }
                    else
                    {
                        _log.Error("More than one build script specified.");
                        options.HasError = true;
                        return options;
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
                                options.HasError = true;
                                return options;
                            }

                            options.Script = "./build.cake";
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

            return ParseOption(name, value.UnQuote(), options);
        }

        private bool ParseOption(string name, string value, CakeOptions options)
        {
            if (name.Equals("verbosity", StringComparison.OrdinalIgnoreCase)
                || name.Equals("v", StringComparison.OrdinalIgnoreCase))
            {
                Verbosity verbosity;
                if (!_verbosityParser.TryParse(value, out verbosity))
                {
                    verbosity = Verbosity.Normal;
                    options.HasError = true;
                }
                options.Verbosity = verbosity;
            }

            if (name.Equals("showdescription", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("s", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowDescription = ParseBooleanValue(value);
            }

            if (name.Equals("dryrun", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("noop", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("whatif", StringComparison.OrdinalIgnoreCase))
            {
                options.PerformDryRun = ParseBooleanValue(value);
            }

            if (name.Equals("help", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("?", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowHelp = ParseBooleanValue(value);
            }

            if (name.Equals("version", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("ver", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowVersion = ParseBooleanValue(value);
            }

            if (name.Equals("debug", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("d", StringComparison.OrdinalIgnoreCase))
            {
                options.PerformDebug = ParseBooleanValue(value);
            }

            if (name.Equals("mono", StringComparison.OrdinalIgnoreCase))
            {
                options.Mono = ParseBooleanValue(value);
            }

            if (name.Equals("experimental", StringComparison.OrdinalIgnoreCase))
            {
                options.Experimental = ParseBooleanValue(value);
            }

            if (options.Arguments.ContainsKey(name))
            {
                _log.Error("Multiple arguments with the same name ({0}).", name);
                return false;
            }

            options.Arguments.Add(name, value);
            return true;
        }

        private static bool ParseBooleanValue(string value)
        {
            value = (value ?? string.Empty).UnQuote();
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }
            if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            throw new InvalidOperationException("Argument value is not a valid boolean value.");
        }
    }
}
