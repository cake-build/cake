// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Responsible for generating parameter tokens in method alias generation.
    /// </summary>
    internal sealed class ParameterEmitter
    {
        private static readonly ParameterFormatter _parameterFormatter = new ParameterFormatter();

        internal static string Emit(ParameterInfo parameter, bool includeType)
        {
            return string.Join(string.Empty, BuildParameterTokens(parameter, includeType));
        }

        private static IEnumerable<string> BuildParameterTokens(ParameterInfo parameter, bool includeType)
        {
            if (parameter.IsOut)
            {
                yield return "out ";
            }
            else if (parameter.ParameterType.IsByRef)
            {
                yield return "ref ";
            }
            if (includeType)
            {
                if (parameter.IsDefined(typeof(ParamArrayAttribute)))
                {
                    yield return "params ";
                }

                // if the parameter has attributes specified
                var customAttrs = parameter.GetCustomAttributesData();
                if (customAttrs.Count > 0)
                {
                    // filter out the any custom parameter attributes that will be emitted by other means.
                    var exclusions = new[]
                    {
                        typeof(OptionalAttribute),
                        typeof(OutAttribute),
                        typeof(ParamArrayAttribute),
                        typeof(DecimalConstantAttribute)
                    };

                    foreach (var item in customAttrs.Where(p => !exclusions.Contains(p.AttributeType)))
                    {
                        var attributeType = item.AttributeType.GetFullName();
                        if (item.AttributeType.Name.EndsWith("Attribute", StringComparison.OrdinalIgnoreCase))
                        {
                            attributeType = attributeType.Substring(0, attributeType.LastIndexOf("Attribute", StringComparison.OrdinalIgnoreCase));
                        }

                        if (item.ConstructorArguments.Count < 1 && item.NamedArguments.Count < 1)
                        {
                            // basic case, empty constructor, just emit the decoration
                            yield return $"[{attributeType}] ";
                        }
                        else
                        {
                            // has ctor or named parameters.  we'll need to enumerate those options,
                            // keeping in mind that we have to normalize the type names where it's appropriate.
                            yield return $"[{attributeType}(";
                            if (item.ConstructorArguments.Count > 0)
                            {
                                yield return string.Join(", ", item.ConstructorArguments.Select(NormalizeCustomAttributeTypedArgument));
                            }

                            if (item.NamedArguments.Count > 0)
                            {
                                if (item.ConstructorArguments.Count > 0)
                                {
                                    yield return ", ";
                                }

                                yield return string.Join(", ", item.NamedArguments.Select(x => $"{x.MemberName} = {NormalizeCustomAttributeTypedArgument(x.TypedValue)}"));
                            }
                            yield return ")] ";
                        }
                    }
                }

                // if the parameter is 'out' (or implicitly, by ref),
                // use GetElementType to get the correct value for codegen (instead of IDisposable& or similar)
                if (parameter.ParameterType.IsByRef)
                {
                    yield return parameter.ParameterType.GetElementType().GetFullName();
                }
                else
                {
                    yield return parameter.ParameterType.GetFullName();
                }
                yield return " ";
            }

            yield return _parameterFormatter.FormatName(parameter);

            // GH-1166; add support for specifying default parameter values
            if (includeType && parameter.IsOptional)
            {
                yield return " = ";
                yield return BuildDefaultParameterValueToken(parameter);
            }
        }

        private static string NormalizeCustomAttributeTypedArgument(CustomAttributeTypedArgument arg)
        {
            if (arg.ArgumentType == null)
            {
                return arg.ToString();
            }

            var normalizedTypeName = arg.ArgumentType.GetFullName();

            if (arg.ArgumentType.IsEnum)
            {
                // casting the value to int in the case of an enum
                // solves the same issue as the same workaround solves in
                // BuildDefaultParameterValueToken, below, on linux/mac
                return $"({normalizedTypeName}){(int)arg.Value}";
            }

            if (arg.Value == null)
            {
                return $"({normalizedTypeName})null";
            }

            if (arg.ArgumentType == typeof(string))
            {
                return $"\"{arg.Value}\"";
            }

            if (arg.ArgumentType == typeof(char))
            {
                return $"'{arg.Value}'";
            }

            if (arg.ArgumentType == typeof(Type))
            {
                return $"typeof({normalizedTypeName})";
            }

            if (!arg.ArgumentType.IsArray)
            {
                return $"({normalizedTypeName}){arg.Value}";
            }

            // if it's an array, compose those tokens.
            var argList = arg.Value as IList<CustomAttributeTypedArgument>;
            var arrayType = arg.ArgumentType.GetElementType().GetFullName();
            return $"new {arrayType}[{argList.Count}] {{ {string.Join(", ", argList.Select(NormalizeCustomAttributeTypedArgument))} }}";
        }

        private static string BuildDefaultParameterValueToken(ParameterInfo parameter)
        {
            var type = parameter.ParameterType;
            var value = parameter.RawDefaultValue;

            // this addresses the issue seen in Mono where both RawDefaultValue and DefaultValue
            // return System.Reflection.Missing when parameter type is Nullable<decimal>
            if (value is Missing && type == typeof(decimal?))
            {
                var attr = parameter.GetCustomAttribute(typeof(DecimalConstantAttribute)) as DecimalConstantAttribute;
                if (attr != null)
                {
                    value = attr.Value;
                }
            }

            // if the default value is null, just return the literal "null"
            if (value == null)
            {
                return "null";
            }

            if (type.IsSubclassOfRawGeneric(typeof(Nullable<>)))
            {
                // this is really only needing to account for char? and bool?
                // unwrap the type and use the same logic as non-nullable by calling the BuildParameterValueToken method
                var innerType = Nullable.GetUnderlyingType(type);
                return $"({innerType.GetFullName()}){BuildParameterValueToken(innerType, value)}";
            }

            if (typeof(Enum).IsAssignableFrom(type) || IsNumeric(type))
            {
                // this works around an issue in Mono where the value coming from RawDefaultValue
                // or DefaultValue will print as EnumValue instead of the underlying numeric.
                // arguably, this may be the more correct implementation.
                if (typeof(Enum).IsAssignableFrom(type))
                {
                    value = (int)value;
                }

                // nullable numerics are handled in the previous block, so just cast it and use the value.
                return string.Format(CultureInfo.InvariantCulture, "({0}){1}",
                    type.GetFullName(),
                    value);
            }

            // if it's not a special case, just pass the call
            return BuildParameterValueToken(type, value);
        }

        private static string BuildParameterValueToken(Type type, object value)
        {
            if (type == typeof(bool))
            {
                return value.ToString().ToLower();
            }

            if (type == typeof(string))
            {
                // fix quotes, wrap string in quotes
                var s = ((string)value).Replace("\"", "\\\"");
                return $"\"{s}\"";
            }

            if (type == typeof(char))
            {
                return $"'{value}'";
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}", value);
        }

        private static readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        private static bool IsNumeric(Type myType)
        {
            return _numericTypes.Contains(myType);
        }
    }
}
