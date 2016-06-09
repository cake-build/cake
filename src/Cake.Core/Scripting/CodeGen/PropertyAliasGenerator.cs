// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Cake.Core.Annotations;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Responsible for generating script property aliases.
    /// </summary>
    public static class PropertyAliasGenerator
    {
        /// <summary>
        /// Generates a script property alias from the specified method.
        /// The provided method must be an extension method for <see cref="ICakeContext"/>
        /// and it must be decorated with the <see cref="CakePropertyAliasAttribute"/>.
        /// </summary>
        /// <param name="method">The method to generate the code for.</param>
        /// <returns>The generated code.</returns>
        public static string Generate(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            // Perform validation.
            ValidateMethod(method);
            ValidateMethodParameters(method);

            // Get the property alias attribute.
            var attribute = method.GetCustomAttribute<CakePropertyAliasAttribute>();

            // Generate code.
            return attribute.Cache
                ? GenerateCachedCode(method)
                    : GenerateCode(method);
        }

        private static void ValidateMethod(MethodInfo method)
        {
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

            if (!method.IsDefined(typeof(CakePropertyAliasAttribute)))
            {
                const string format = "The method '{0}' is not a property alias.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }
        }

        private static void ValidateMethodParameters(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var parameterCorrect = false;
            if (parameters.Length == 1)
            {
                if (parameters[0].ParameterType == typeof(ICakeContext))
                {
                    parameterCorrect = true;
                }
            }

            if (!parameterCorrect)
            {
                const string format = "The property alias '{0}' has an invalid signature.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }

            if (method.IsGenericMethod)
            {
                const string format = "The property alias '{0}' cannot be generic.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }

            if (method.ReturnType == typeof(void))
            {
                const string format = "The property alias '{0}' cannot return void.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, method.Name));
            }
        }

        private static string GenerateCode(MethodInfo method)
        {
            var builder = new StringBuilder();

            builder.Append("public ");
            builder.Append(GetReturnType(method));
            builder.Append(" ");
            builder.Append(method.Name);
            builder.AppendLine();
            builder.Append("{");
            builder.AppendLine();
            builder.Append("    get");
            builder.AppendLine();
            builder.AppendLine("    {");

            // Property is obsolete?
            var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
            if (obsolete != null)
            {
                var message = GetObsoleteMessage(method, obsolete);

                if (obsolete.IsError)
                {
                    builder.Append("        throw new Cake.Core.CakeException(\"");
                    builder.Append(message);
                    builder.Append("\");");
                    builder.AppendLine();
                    builder.AppendLine("    }");
                    builder.AppendLine("}");
                    builder.AppendLine();
                    builder.AppendLine();

                    return builder.ToString();
                }

                builder.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "        Context.Log.Warning(\"Warning: {0}\");", message));
            }

            builder.Append("        return ");
            builder.Append(method.GetFullName());
            builder.Append("(Context);");
            builder.AppendLine();
            builder.AppendLine("    }");
            builder.AppendLine("}");
            builder.AppendLine();
            builder.AppendLine();

            return builder.ToString();
        }

        private static string GenerateCachedCode(MethodInfo method)
        {
            var builder = new StringBuilder();

            // Property is obsolete?
            var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
            if (obsolete != null)
            {
                if (obsolete.IsError)
                {
                    return GenerateCode(method);
                }
            }

            // Backing field.
            builder.Append("private ");
            builder.Append(GetReturnType(method));
            if (method.ReturnType.GetTypeInfo().IsValueType)
            {
                builder.Append("?");
            }
            builder.Append(" _");
            builder.Append(method.Name);
            builder.Append(";");
            builder.AppendLine();

            // Property
            builder.Append("public ");
            builder.Append(GetReturnType(method));
            builder.Append(" ");
            builder.Append(method.Name);
            builder.AppendLine();
            builder.Append("{");
            builder.AppendLine();
            builder.AppendLine("    get");
            builder.Append("    {");
            builder.AppendLine();

            // Property is obsolete?
            if (obsolete != null)
            {
                var message = GetObsoleteMessage(method, obsolete);
                builder.AppendLine(string.Format(
                    CultureInfo.InvariantCulture,
                    "        Context.Log.Warning(\"Warning: {0}\");", message));
            }

            builder.AppendLine(string.Format(
                CultureInfo.InvariantCulture,
                "        if (_{0}==null)", method.Name));

            builder.Append("        {");
            builder.AppendLine();
            builder.AppendFormat("            _{0} = ", method.Name);
            builder.Append(method.GetFullName());
            builder.Append("(Context);");
            builder.AppendLine();
            builder.Append("        }");
            builder.AppendLine();
            builder.AppendFormat("        return _{0}", method.Name);
            if (method.ReturnType.GetTypeInfo().IsValueType)
            {
                builder.Append(".Value");
            }
            builder.Append(";");
            builder.AppendLine();
            builder.Append("    }");
            builder.AppendLine();
            builder.Append("}");
            builder.AppendLine();
            builder.AppendLine();

            return builder.ToString();
        }

        private static string GetReturnType(MethodInfo method)
        {
            return method.ReturnType.GetFullName();
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
