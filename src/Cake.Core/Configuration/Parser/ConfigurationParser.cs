// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using Cake.Core.IO;

namespace Cake.Core.Configuration.Parser
{
    internal sealed class ConfigurationParser
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public ConfigurationParser(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public IDictionary<string, string> Read(FilePath path)
        {
            path = path.MakeAbsolute(_environment);

            // Make sure that the configuration file exist.
            var file = _fileSystem.GetFile(path);
            if (!file.Exists)
            {
                throw new FileNotFoundException("Unable to find the configuration file.", path.FullPath);
            }

            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream))
            {
                return Read(reader.ReadToEnd());
            }
        }

        private static IDictionary<string, string> Read(string text)
        {
            var tokens = ConfigurationTokenizer.Tokenize(text);
            var section = string.Empty;
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            while (tokens.Current != null)
            {
                switch (tokens.Current.Kind)
                {
                    case ConfigurationTokenKind.Section:
                        section = ParseSection(tokens);
                        break;
                    case ConfigurationTokenKind.Value:
                        var pair = ParseKeyAndValue(tokens, section);
                        result[pair.Key] = pair.Value;
                        break;
                    default:
                        throw new InvalidOperationException("Encountered unexpected token.");
                }
            }
            return result;
        }

        private static string ParseSection(ConfigurationTokenStream tokens)
        {
            var value = tokens.Current.Value;
            if (ContainsWhiteSpace(value))
            {
                throw new InvalidOperationException("Sections cannot contain whitespace.");
            }
            tokens.Consume();
            return value;
        }

        private static KeyValuePair<string, string> ParseKeyAndValue(ConfigurationTokenStream tokens, string section)
        {
            // Get the key.
            var key = tokens.Current.Value;
            tokens.Consume();
            if (ContainsWhiteSpace(key))
            {
                const string message = "The key '{0}' contains whitespace.";
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, message, key));
            }

            // Expect the equality sign.
            tokens.Expect(ConfigurationTokenKind.Equals, "Expected to find '=' token.");
            tokens.Consume();

            // Get the value.
            tokens.Expect(ConfigurationTokenKind.Value, "Expected to find value.");
            var value = tokens.Current.Value;
            tokens.Consume();

            // Append section to key?
            if (!string.IsNullOrWhiteSpace(section))
            {
                key = string.Concat(section, "_", key);
            }
            return new KeyValuePair<string, string>(key, value);
        }

        private static bool ContainsWhiteSpace(string text)
        {
            foreach (var character in text)
            {
                if (char.IsWhiteSpace(character))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
