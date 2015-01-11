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
        /// The provided method must be an extensionmethod for <see cref="ICakeContext"/>
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
            Debug.Assert(method.DeclaringType != null); // Resharper

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
            builder.Append("{");
            builder.Append("get{return ");
            builder.Append(method.GetFullName());
            builder.Append("(GetContext());");
            builder.Append("}}");

            return builder.ToString();
        }

        private static string GenerateCachedCode(MethodInfo method)
        {
            var builder = new StringBuilder();

            // Backing field.
            builder.Append("private ");
            builder.Append(GetReturnType(method));
            if (method.ReturnType.IsValueType)
            {
                builder.Append("?");
            }
            builder.Append(" _");
            builder.Append(method.Name);
            builder.Append(";");
            builder.Append("\n");

            // Property
            builder.Append("public ");
            builder.Append(GetReturnType(method));
            builder.Append(" ");
            builder.Append(method.Name);
            builder.Append("{");
            builder.Append("get{");
            builder.AppendFormat("if(_{0}==null)", method.Name);
            builder.Append("{");
            builder.AppendFormat("_{0}=", method.Name);
            builder.Append(method.GetFullName());
            builder.Append("(GetContext());");
            builder.Append("}");
            builder.AppendFormat("return _{0}", method.Name);
            if (method.ReturnType.IsValueType)
            {
                builder.Append(".Value");
            }
            builder.Append(";");
            builder.Append("}");
            builder.Append("}");

            return builder.ToString();
        }

        private static string GetReturnType(MethodInfo method)
        {
            return method.ReturnType.GetFullName();
        }
    }
}
