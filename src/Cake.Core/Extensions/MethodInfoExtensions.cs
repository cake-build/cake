// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
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
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            var builder = new StringBuilder();
            builder.Append(includeMethodNamespace ? method.GetFullName() : method.Name);
            builder.Append("(");
            var parameters = method.GetParameters();
            var parameterList = new string[parameters.Length];
            for (var i = 0; i < parameterList.Length; i++)
            {
                var isParams = parameters[i].IsDefined(typeof(ParamArrayAttribute));
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
            if (method != null && method.DeclaringType != null)
            {
                return string.Concat(method.DeclaringType.FullName, ".", method.Name);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the namespace of the method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The namespace of the method.</returns>
        public static string GetNamespace(this MethodInfo method)
        {
            if (method != null && method.DeclaringType != null)
            {
                return method.DeclaringType.Namespace;
            }
            return string.Empty;
        }
    }
}
