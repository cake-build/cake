using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Cake.Core.Annotations;
using Cake.Core.Extensions;

namespace Cake.Core.Scripting.CodeGen
{
    public static class PropertyAliasGenerator
    {
        public static string Generate(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            Debug.Assert(method.DeclaringType != null); // Resharper

            if (!method.DeclaringType.IsStatic())
            {
                const string format = "The type '{0}' is not static.";
                throw new CakeException(string.Format(format, method.DeclaringType.FullName));
            }
            if (!method.IsDefined(typeof(ExtensionAttribute)))
            {
                const string format = "The method '{0}' is not an extension method.";
                throw new CakeException(string.Format(format, method.Name));
            }
            if (!method.IsDefined(typeof(CakePropertyAliasAttribute)))
            {
                const string format = "The method '{0}' is not a property alias.";
                throw new CakeException(string.Format(format, method.Name));
            }

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
                throw new CakeException(string.Format(format, method.Name));   
            }

            if (method.IsGenericMethod)
            {
                const string format = "The property alias '{0}' cannot be generic.";
                throw new CakeException(string.Format(format, method.Name));
            }

            if (method.ReturnType == typeof (void))
            {
                const string format = "The property alias '{0}' cannot return void.";
                throw new CakeException(string.Format(format, method.Name));                
            }

            return GenerateCode(method);
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

        private static string GetReturnType(MethodInfo method)
        {
            return method.ReturnType.GetFullName();
        }
    }
}
