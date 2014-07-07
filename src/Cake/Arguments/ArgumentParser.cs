using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Extensions;

namespace Cake.Arguments
{
    internal sealed class ArgumentParser : IArgumentParser
    {
        private readonly ICakeLog _log;

        public ArgumentParser(ICakeLog log)
        {
            _log = log;
        }

        public CakeOptions Parse(IEnumerable<string> args)
        {
            var options = new CakeOptions();
            var isParsingOptions = false;

            foreach (var arg in args)
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
                        if (IsOption(arg))
                        {
                            _log.Error("First argument must be the build script.");
                            return null;
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
            return arg[0] == '-';
        }

        private bool ParseOption(string arg, CakeOptions options)
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

            return ParseOption(name, value, options);
        }

        private bool ParseOption(string name, string value, CakeOptions options)
        {
            if (name.Equals("verbosity", StringComparison.OrdinalIgnoreCase)
                || name.Equals("v", StringComparison.OrdinalIgnoreCase))
            {
                // Parse verbosity.
                var converter = TypeDescriptor.GetConverter(typeof(Verbosity));
                var verbosity = converter.ConvertFromInvariantString(value);
                if (verbosity != null)
                {
                    options.Verbosity = (Verbosity)verbosity;   
                }                    
            }

            if (name.Equals("showdescription", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("s", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowDescription = true;
            }

            if (name.Equals("help", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("?", StringComparison.OrdinalIgnoreCase))
            {
                options.ShowHelp = true;
            }

            if (options.Arguments.ContainsKey(name))
            {
                _log.Error("Multiple arguments with the same name ({0}).", name);
                return false;
            }

            options.Arguments.Add(name, value);
            return true;
        }
    }
}