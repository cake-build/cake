// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Cake.Core.Annotations;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Responsible for generating script method aliases.
    /// </summary>
    public static class MethodAliasGenerator
    {
        /// <summary>
        /// Generates a script method alias from the specified method.
        /// The provided method must be an extension method for <see cref="ICakeContext"/>
        /// and it must be decorated with the <see cref="CakeMethodAliasAttribute"/>.
        /// </summary>
        /// <param name="method">The method to generate the code for.</param>
        /// <returns>The generated code.</returns>
        public static string Generate(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            var isFunction = method.ReturnType != typeof(void);
            var builder = new StringBuilder();
            var parameters = method.GetParameters().Skip(1).ToArray();

            // Generate method signature.
            builder.Append("public ");
            builder.Append(GetReturnType(method));
            builder.Append(" ");
            builder.Append(method.Name);

            if (method.IsGenericMethod)
            {
                // Add generic arguments to proxy signature.
                BuildGenericArguments(method, builder);
            }

            builder.Append("(");
            builder.Append(string.Concat(GetProxyParameters(parameters, true)));
            builder.Append(")");
            builder.AppendLine();
            builder.Append("{");
            builder.AppendLine();

            // Method is obsolete?
            var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
            if (obsolete != null)
            {
                var message = GetObsoleteMessage(method, obsolete);

                if (obsolete.IsError)
                {
                    // Throw an exception.
                    var exception = string.Format(
                        CultureInfo.InvariantCulture,
                        "    throw new Cake.Core.CakeException(\"{0}\");", message);
                    builder.AppendLine(exception);

                    // End method.
                    builder.Append("}");
                    builder.AppendLine();
                    builder.AppendLine();
                    return builder.ToString();
                }

                builder.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "    Context.Log.Warning(\"Warning: {0}\");", message));
            }

            builder.Append("    ");

            if (isFunction)
            {
                // Add return keyword.
                builder.Append("return ");
            }

            // Call extension method.
            builder.Append(method.GetFullName());

            if (method.IsGenericMethod)
            {
                // Add generic arguments to method call.
                BuildGenericArguments(method, builder);
            }

            builder.Append("(");
            builder.Append(string.Concat(GetProxyParameters(parameters, false)));
            builder.Append(");");
            builder.AppendLine();

            // End method.
            builder.Append("}");
            builder.AppendLine();
            builder.AppendLine();

            return builder.ToString();
        }

        private static string GetReturnType(MethodInfo method)
        {
            return method.ReturnType == typeof(void)
                ? "void"
                    : method.ReturnType.GetFullName();
        }

        private static IEnumerable<string> GetProxyParameters(IEnumerable<ParameterInfo> parameters, bool includeType)
        {
            var first = includeType;
            if (!includeType)
            {
                yield return "Context";
            }
            foreach (var parameter in parameters)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    yield return ", ";
                }
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
                    yield return parameter.ParameterType.GetFullName();
                    yield return " ";
                }
                yield return parameter.Name;
            }
        }

        private static void BuildGenericArguments(MethodInfo method, StringBuilder builder)
        {
            builder.Append("<");
            var genericArguments = new List<string>();
            foreach (var argument in method.GetGenericArguments())
            {
                genericArguments.Add(argument.Name);
            }
            builder.Append(string.Join(", ", genericArguments));
            builder.Append(">");
        }

        private static string GetObsoleteMessage(MethodInfo method, ObsoleteAttribute attribute)
        {
            const string format = "The alias {0} has been made obsolete. {1}";
            var message = string.Format(
                CultureInfo.InvariantCulture,
                format, method.Name, attribute.Message);
            return message.Trim();
        }
    }
}
