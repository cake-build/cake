using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Cake.Extensions
{
    internal static class MethodInfoExtensions
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
                parameterList[i] = parameters[i].ParameterType.GetFullName(includeParameterNamespace);
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
