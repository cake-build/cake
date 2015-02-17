using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null"); // Resharper
            if (!method.DeclaringType.IsStatic())
            {
                const string format = "The type '{0}' is not static.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.DeclaringType.FullName));
            }
            if (!method.IsDefined(typeof(ExtensionAttribute)))
            {
                const string format = "The method '{0}' is not an extension method.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }
            if (!method.IsDefined(typeof(CakeMethodAliasAttribute)))
            {
                const string format = "The method '{0}' is not a method alias.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }
            return GenerateCode(method);
        }

        private static string GenerateCode(MethodInfo method)
        {
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
            builder.Append("{");

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

            // End method.
            builder.Append("}");

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
                yield return "GetContext()";
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
    }
}
