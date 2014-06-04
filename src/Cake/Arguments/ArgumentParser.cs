using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Arguments
{
    internal sealed class ArgumentParser : IArgumentParser
    {
        private readonly ICakeLog _log;
        private readonly List<ArgumentDescription> _descriptions;

        public ArgumentParser(ICakeLog log)
        {
            _log = log;
            _descriptions = new List<ArgumentDescription>();

            AddOption(new[] { "verbosity", "v" },
                            @"-verbosity=value", false,
                            @"Specifies the amount of information to display.",
                            o => o.Verbosity);
        }

        public CakeOptions Parse(IEnumerable<string> args)
       {
            var options = new CakeOptions();
            var isParsingOptions = true;

            foreach (var arg in args)
            {
                if (isParsingOptions)
                {
                    if (IsOption(arg))
                    {
                        if (!ParseOption(arg, options))
                        {
                            return null;
                        }
                        continue;
                    }

                    isParsingOptions = false;

                    // Quoted?
                    var script = arg;
                    if (script.StartsWith("\"", StringComparison.OrdinalIgnoreCase) 
                        && script.EndsWith("\"", StringComparison.OrdinalIgnoreCase))
                    {
                        script = script.Trim('"');
                    }

                    options.Script = new FilePath(script);
                }
                else
                {
                    if (options.Script != null)
                    {
                        _log.Error("More than one build script specified.");
                        return null;
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
            if (arg[0] != '-')
            {
                return false;
            }
            return true;
        }

        private bool ParseOption(string arg, CakeOptions options)
        {
            string name, value;

            var separatorIndex = arg.IndexOfAny(new[] {'='});
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

            foreach (var command in _descriptions)
            {
                foreach (var @switch in command.Names)
                {
                    if (@switch.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        // Is the property a boolean and this is a switch?
                        if (command.Property.PropertyType == typeof (bool) && string.IsNullOrWhiteSpace(value))
                        {
                            command.Property.SetValue(options, true, null);
                            return true;
                        }

                        var converter = TypeDescriptor.GetConverter(command.Property.PropertyType);
                        if (!converter.CanConvertFrom(typeof (string)))
                        {
                            const string format = "Cannot convert '{0}' to an instance of {1}.";
                            _log.Error(format, value, command.Property.PropertyType.FullName);
                            return false;
                        }
                        var convertedValue = converter.ConvertFromInvariantString(value);
                        command.Property.SetValue(options, convertedValue, null);
                        return true;
                    }
                }
            }

            _log.Error("Unknown option: {0}", name);
            return false;
        }

        private void AddOption<TValue>(string[] names, string parameter, bool required, string description, Expression<Func<CakeOptions, TValue>> action)
        {
            var property = GetProperty(action);
            _descriptions.Add(new ArgumentDescription(names, parameter, required, description, property));
        }

        private static PropertyInfo GetProperty<TValue>(Expression<Func<CakeOptions, TValue>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                var member = ((MemberExpression) expression.Body).Member as PropertyInfo;
                if (member != null)
                {
                    return member;
                }
            }
            if (expression.Body.NodeType == ExpressionType.Convert
                && ((UnaryExpression) expression.Body).Operand.NodeType == ExpressionType.MemberAccess)
            {
                var member = ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member as PropertyInfo;
                if (member != null)
                {
                    return member;
                }
            }
            throw new ArgumentException("Argument 'expression' is malformed.", "expression");
        }
    }
}