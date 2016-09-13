// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Frosting.Internal.Arguments
{
    internal static class ArgumentParser
    {
        public static CakeHostOptions Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var options = new CakeHostOptions();

            foreach (var argument in args)
            {
                ParseOption(argument.UnQuote(), options);
            }

            return options;
        }

        private static void ParseOption(string arg, CakeHostOptions options)
        {
            if (!IsOption(arg))
            {
                throw new FrostingException($"Encountered invalid option '{arg}'");
            }

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

            ParseOption(name, value.UnQuote(), options);
        }

        private static void ParseOption(string name, string value, CakeHostOptions options)
        {
            if (name.Equals("working", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("w", StringComparison.OrdinalIgnoreCase))
            {
                options.WorkingDirectory = new DirectoryPath(value);
            }

            if (name.Equals("verbosity", StringComparison.OrdinalIgnoreCase)
                || name.Equals("v", StringComparison.OrdinalIgnoreCase))
            {
                Verbosity verbosity;
                if (!VerbosityParser.TryParse(value, out verbosity))
                {
                    throw new CakeException($"The value '{value}' is not a valid verbosity.");
                }
                options.Verbosity = verbosity;
            }

            if (name.Equals("target", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("t", StringComparison.OrdinalIgnoreCase))
            {
                options.Target = value;
            }

            if (name.Equals("help", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("h", StringComparison.OrdinalIgnoreCase))
            {
                if (ParseBooleanValue(value))
                {
                    options.Command = CakeHostCommand.Help;
                }
            }

            if (name.Equals("dryrun", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("d", StringComparison.OrdinalIgnoreCase))
            {
                if (ParseBooleanValue(value))
                {
                    options.Command = CakeHostCommand.DryRun;
                }
            }

            if (name.Equals("version", StringComparison.OrdinalIgnoreCase))
            {
                if (ParseBooleanValue(value))
                {
                    options.Command = CakeHostCommand.Version;
                }
            }

            if (options.Arguments.ContainsKey(name))
            {
                throw new CakeException($"More than one argument called '{name}' was encountered.");
            }

            options.Arguments.Add(name, value);
        }

        private static bool IsOption(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg))
            {
                return false;
            }
            return arg.StartsWith("--") || arg.StartsWith("-");
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
            throw new CakeException($"Argument value '{value}' is not a valid boolean value.");
        }
    }
}