using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Arguments
{
    internal sealed class ArgumentParser : IArgumentParser
    {
        private readonly ICakeLog _log;
        private readonly IFileSystem _fileSystem;

        private readonly string[] _defaultScriptNameConventions =
        {
            "build.cake",
            "default.cake",
            "bake.cake",
            ".cakefile"
        };

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

            string joinedCommandLine;
            var result = new CakeOptions();
            var firstArgument = args.FirstOrDefault() ?? string.Empty;
            if (firstArgument.StartsWith("-"))
            {
                joinedCommandLine = string.Join(" ", args);
            }
            else
            {
                if (string.IsNullOrEmpty(firstArgument))
                {
                    result.Script = GetDefaultScript();
                }
                else
                {
                    var cakeFileArgument = firstArgument;
                    if (cakeFileArgument.StartsWith("\""))
                    {
                        cakeFileArgument = cakeFileArgument.Trim('\"');
                    }
                    result.Script = new FilePath(cakeFileArgument);
                }
                joinedCommandLine = string.Join(" ", args.Skip(1));
            }

            var parser = new UnixCommandParser();
            List<KeyValuePair<string, string>> parsedResult;
            try
            {
                parsedResult = parser.Parse(joinedCommandLine);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }

            var duplicateKeynames = parsedResult
                .GroupBy(x => x.Key)
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .Distinct()
                .ToList();

            if (duplicateKeynames.Any())
            {
                _log.Error("Using a argument-name more than once is not allowed: " +
                           string.Join(", ", duplicateKeynames));
                return null;
            }

            // Store all the options in the dictionary.
            foreach (var kv in parsedResult)
            {
                var key = kv.Key;
                result.Arguments.Add(string.Join(string.Empty, key.SkipWhile(x => x == '-')), kv.Value);
            }

            // Reflect the options and assign values. Keep track of which properties are assigned
            // so to prevent double assiging using alternative.
            var propertyOptions = result.GetType()
                .GetProperties()
                .Where(x => x.CanWrite)
                .Select(x => new
                {
                    Property = x,
                    Aliases = x.GetCustomAttributes<CommandArgumentNameAttribute>()
                })
                .Where(x => x.Aliases.Count() != 0)
                .ToList();

            var doublePropertyNames = propertyOptions.SelectMany(x => x.Aliases)
                .GroupBy(x => x.Name.ToLower())
                .Where(x => x.Count() > 1)
                .Select(x => x.Key)
                .Distinct()
                .ToList();

            if (doublePropertyNames.Any())
            {
                // Since this checks the data-class we throw an exception instead of an error.
                // This will result in most of the unit tests to fail.
                const string format = "Double command argument names are found in {0}: {1}";
                var typeName = result.GetType().Name;
                var propertyNames = string.Join(", ", doublePropertyNames);
                throw new Exception(string.Format(CultureInfo.InvariantCulture, format, typeName, propertyNames));
            }

            foreach (var item in propertyOptions)
            {
                // 1. Find the arguments that can be applied
                // 2. Check if there is only one argument (we don't support multiple arguments).
                //    We already did a similar check but we didn't take in account the aliases.
                // 3. If property-type is boolean and no argument set it to true
                //    If property-type is boolean and a string is used then convert the string (use english!)
                // 4. If property-type is an enum then try and convert to value-name to enum-value
                // 5. If property-type is an integer then try and convert the string to a number (use english)
                // 6. If property-type is a string then just store the string
                // 7. If property-type is unknown the use an error.

                // 1. Find the arguments that can be applied
                var applicableArguments = parsedResult
                    .Where(x => item.Aliases.Any(y => CompareKey(x.Key, y.Name)))
                    .ToList();

                if (applicableArguments.Count == 0)
                {
                    // Skip this property since no values are provided.
                    continue;
                }

                // 2. Check if there is only one argument (we don't support multiple arguments).
                //    We already did a similar check but we didn't take in account the aliases.
                if (applicableArguments.Count() > 1)
                {
                    var keyNames = applicableArguments.Select(x => x.Key);
                    _log.Error("Multiple arguments found that use an alias to double assign on {0} using {1}",
                        item.Property.Name, string.Join(", ", keyNames));
                    return null;
                }

                var singleValue = applicableArguments.Single().Value;

                // 3. If property-type is boolean and no argument set it to true
                //    If property-type is boolean and a string is used then convert the string (use english!)
                if (item.Property.PropertyType == typeof(bool))
                {
                    if (singleValue != null)
                    {
                        var value = Convert.ToBoolean(singleValue, CultureInfo.InvariantCulture.NumberFormat);
                        item.Property.SetValue(result, value);
                    }
                    else
                    {
                        item.Property.SetValue(result, true);
                    }
                    continue;
                }

                // 4. If property-type is an enum then try and convert to value-name to enum-value
                if (item.Property.PropertyType.IsEnum)
                {
                    if (singleValue == null)
                    {
                        _log.Error("Argument-value required for {0}", item.Property.Name);
                        return null;
                    }

                    if (singleValue.Length == 1)
                    {
                        // This is a short value.
                        var groupedNames = Enum.GetNames(item.Property.PropertyType).ToDictionary(x => x[0], x => x);
                        var filteredGroup = groupedNames.Where(x => CompareKey(x.Key.ToString(), singleValue)).ToList();
                        if (filteredGroup.Any())
                        {
                            singleValue = filteredGroup.Single().Value;
                        }
                    }

                    try
                    {
                        var convertedEnum = Enum.Parse(item.Property.PropertyType, singleValue, true);
                        item.Property.SetValue(result, convertedEnum);
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex.Message);
                        return null;
                    }

                    continue;
                }

                // 5. If property-type is an integer then try and convert the string to a number (use english)
                if (item.Property.PropertyType == typeof(int))
                {
                    var convertedValue = Convert.ToInt32(singleValue, CultureInfo.InvariantCulture.NumberFormat);
                    item.Property.SetValue(result, convertedValue);
                    continue;
                }

                // 6. If property-type is a string then just store the string
                if (item.Property.PropertyType == typeof(string))
                {
                    item.Property.SetValue(result, singleValue);
                    continue;
                }

                // 7. If property-type is unknown the use an error.
                _log.Error("The property \"{0}\" is of unsupported type {1}", item.Property.Name,
                    item.Property.PropertyType.Name);
                return null;
            }

            return result;
        }

        private static bool CompareKey(string key_a, string key_b)
        {
            return string.Compare(key_a, key_b, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

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