using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    public static class MethodInfoExtensions
    {
        public static string GetSignature(this MethodInfo method, 
            bool includeMethodNamespace = true, bool includeParameterNamespace = false)
        {
            var builder = new StringBuilder();
            builder.Append(includeMethodNamespace ? method.GetFullName() : method.Name);
            builder.Append("(");
            var parameters = method.GetParameters();
            var parameterList = new string[parameters.Length];
            for (var i = 0; i < parameterList.Length; i++)
            {
                var isParams = parameters[i].IsDefined(typeof (ParamArrayAttribute));
                var signature = parameters[i].ParameterType.GetFullName(includeParameterNamespace);
                signature = isParams ? string.Concat("params ", signature) : signature;

                parameterList[i] = signature;
            }
            builder.Append(string.Join(", ", parameterList));
            builder.Append(")");
            return builder.ToString();
        }

        public static string GetFullName(this MethodInfo method)
        {
            Debug.Assert(method.DeclaringType != null); // Resharper
            return string.Concat(method.DeclaringType.FullName, ".", method.Name);
        }
    }
}
