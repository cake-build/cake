using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="MethodInfo"/>.
    /// </summary>
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Gets the signature for a method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="includeMethodNamespace">if set to <c>true</c>, include method namespace.</param>
        /// <param name="includeParameterNamespace">if set to <c>true</c>, include parameter namespace.</param>
        /// <returns>The method signature.</returns>
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

        /// <summary>
        /// Gets the full name of a method.
        /// </summary>
        /// <param name="method">The method to get the full name for.</param>
        /// <returns>The full name.</returns>
        public static string GetFullName(this MethodInfo method)
        {
            Debug.Assert(method.DeclaringType != null); // Resharper
            return string.Concat(method.DeclaringType.FullName, ".", method.Name);
        }

        /// <summary>
        /// Gets the namespace of the method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The namespace of the method.</returns>
        public static string GetNamespace(this MethodInfo method)
        {
            Debug.Assert(method.DeclaringType != null); // Resharper
            return method.DeclaringType.Namespace;
        }
    }
}
