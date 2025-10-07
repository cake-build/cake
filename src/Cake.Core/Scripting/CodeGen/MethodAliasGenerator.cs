// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Cake.Core.Annotations;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Responsible for generating script method aliases.
    /// </summary>
    public static class MethodAliasGenerator
    {
        private static readonly System.Security.Cryptography.SHA256 _hasher = System.Security.Cryptography.SHA256.Create();

        /// <summary>
        /// Generates a script method alias from the specified method.
        /// The provided method must be an extension method for <see cref="ICakeContext"/>
        /// and it must be decorated with the <see cref="CakeMethodAliasAttribute"/>.
        /// </summary>
        /// <param name="method">The method to generate the code for.</param>
        /// <returns>The generated code.</returns>
        public static string Generate(MethodInfo method) => Generate(method, out _);

        /// <summary>
        /// Generates a script method alias from the specified method.
        /// The provided method must be an extension method for <see cref="ICakeContext"/>
        /// and it must be decorated with the <see cref="CakeMethodAliasAttribute"/>.
        /// </summary>
        /// <param name="method">The method to generate the code for.</param>
        /// <param name="hash">The hash of method signature.</param>
        /// <returns>The generated code.</returns>
        public static string Generate(MethodInfo method, out string hash)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var builder = new StringBuilder();
            var parameters = method.GetParameters()[1..];

            // Method is obsolete?
            var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
            var isObsolete = obsolete != null;

            // Generate method signature.
            builder.AppendLine("[System.Diagnostics.DebuggerStepThrough]");
            builder.Append("public ");
            builder.Append(GetReturnType(method));
            builder.Append(' ');
            builder.Append(method.Name);

            if (method.IsGenericMethod)
            {
                // Add generic arguments to proxy signature.
                BuildGenericArguments(method, builder);
            }

            builder.Append('(');
            builder.Append(string.Concat(GetProxyParameters(parameters, true)));
            builder.Append(')');

            // if the method is generic, emit any constraints that might exist.
            if (method.IsGenericMethod)
            {
                GenericParameterConstraintEmitter.BuildGenericConstraints(method, builder);
            }

            hash = Convert.ToHexString(
                    _hasher.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString())));

            builder.AppendLine();

            if (isObsolete && !obsolete.IsError)
            {
                builder.AppendLine("#pragma warning disable 0618");
            }
            builder.Append("    => ");
            if (isObsolete)
            {
                var message = GetObsoleteMessage(method, obsolete);

                if (obsolete.IsError)
                {
                    // Throw an exception.
                    var exception = string.Format(
                        CultureInfo.InvariantCulture,
                        "throw new Cake.Core.CakeException(\"{0}\");", message);
                    builder.AppendLine(exception);

                    // End method.
                    builder.AppendLine();
                    return builder.ToString();
                }
            }

            // Call extension method.
            builder.Append(method.GetFullName());

            if (method.IsGenericMethod)
            {
                // Add generic arguments to method call.
                BuildGenericArguments(method, builder);
            }

            builder.Append('(');
            builder.Append(string.Concat(GetProxyParameters(parameters, false)));
            builder.AppendLine(");");

            if (isObsolete)
            {
                builder.AppendLine("#pragma warning restore 0618");
            }

            // End method.
            builder.AppendLine();

            return builder.ToString();
        }

        private static string GetReturnType(MethodInfo method)
        {
            if (method.ReturnType == typeof(void))
            {
                return "void";
            }

            var isDynamic = method.ReturnTypeCustomAttributes.GetCustomAttributes(typeof(DynamicAttribute), true).Any();
            var isNullable = method.ReturnTypeCustomAttributes.GetCustomAttributes(true).Any(attr => attr.GetType().FullName == "System.Runtime.CompilerServices.NullableAttribute");
            return string.Concat(
                isDynamic ? "dynamic" : method.ReturnType.GetFullName(),
                isNullable ? "?" : string.Empty);
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

                yield return ParameterEmitter.Emit(parameter, includeType);
            }
        }

        private static void BuildGenericArguments(MethodInfo method, StringBuilder builder)
        {
            builder.Append('<');
            var genericArguments = new List<string>();
            foreach (var argument in method.GetGenericArguments())
            {
                genericArguments.Add(argument.Name);
            }
            builder.AppendJoin(", ", genericArguments);
            builder.Append('>');
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