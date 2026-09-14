// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Parses compiler-embedded nullable metadata and wraps generated alias members
    /// that need a nullable annotations context.
    /// </summary>
    internal static class NullableAnnotationContext
    {
        internal const byte Oblivious = 0;
        internal const byte NotAnnotated = 1;
        internal const byte Annotated = 2;

        private const string NullableAttributeName = "System.Runtime.CompilerServices.NullableAttribute";
        private const string NullableContextAttributeName = "System.Runtime.CompilerServices.NullableContextAttribute";

        /// <summary>
        /// Gets the nullable flag of a generic type parameter, falling back to the
        /// nullable context of the declaring method, type or module.
        /// </summary>
        /// <param name="genericParameter">The generic type parameter.</param>
        /// <returns><see cref="Oblivious"/>, <see cref="NotAnnotated"/> or <see cref="Annotated"/>.</returns>
        internal static byte GetNullableFlag(Type genericParameter)
        {
            ArgumentNullException.ThrowIfNull(genericParameter);

            foreach (var attribute in genericParameter.GetCustomAttributesData())
            {
                if (TryGetNullableFlags(attribute, out var flags) && flags.Length > 0)
                {
                    return flags[0];
                }
            }

            return GetNullableContextFlag(genericParameter);
        }

        /// <summary>
        /// Formats a parameter type including inner nullable reference annotations.
        /// </summary>
        /// <param name="parameter">The parameter to format.</param>
        /// <returns>The generated type name.</returns>
        internal static string Format(ParameterInfo parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);

            var type = parameter.ParameterType;
            var info = new NullabilityInfoContext().Create(parameter);
            if (type.IsByRef)
            {
                type = type.GetElementType();
                info = info.ElementType ?? info;
            }

            return Format(type, info, GetPositionFlag(parameter));
        }

        /// <summary>
        /// Formats a method return type including inner nullable reference annotations.
        /// </summary>
        /// <param name="method">The method whose return type to format.</param>
        /// <returns>The generated type name.</returns>
        internal static string FormatReturn(MethodInfo method)
        {
            ArgumentNullException.ThrowIfNull(method);

            if (method.ReturnType == typeof(void))
            {
                return "void";
            }

            if (method.ReturnTypeCustomAttributes.GetCustomAttributes(typeof(System.Runtime.CompilerServices.DynamicAttribute), true).Any())
            {
                return "dynamic";
            }

            return Format(
                method.ReturnType,
                new NullabilityInfoContext().Create(method.ReturnParameter),
                GetPositionFlag(method.ReturnParameter));
        }

        /// <summary>
        /// Returns <c>true</c> when generated alias source for <paramref name="method"/>
        /// needs a nullable annotations context.
        /// </summary>
        /// <param name="method">The alias method.</param>
        /// <returns><c>true</c> if the generated member should be wrapped.</returns>
        internal static bool Requires(MethodInfo method)
        {
            ArgumentNullException.ThrowIfNull(method);

            return HasNullableReferenceAnnotation(method)
                   || GenericParameterConstraintEmitter.HasNullableAwareConstraint(method);
        }

        /// <summary>
        /// Wraps generated alias source with <c>#nullable enable</c> / <c>#nullable restore</c>
        /// when <paramref name="method"/> requires a nullable annotations context.
        /// </summary>
        /// <param name="method">The alias method.</param>
        /// <param name="code">The generated member source.</param>
        /// <returns>The original or wrapped source.</returns>
        internal static string WrapIfRequired(MethodInfo method, string code)
        {
            return Requires(method) ? Wrap(code) : code;
        }

        /// <summary>
        /// Wraps generated alias source with <c>#nullable enable</c> / <c>#nullable restore</c>.
        /// </summary>
        /// <param name="code">The generated member source.</param>
        /// <returns>The wrapped source.</returns>
        internal static string Wrap(string code)
        {
            var builder = new StringBuilder();
            builder.AppendLine("#nullable enable");
            builder.Append(code);
            if (code.Length == 0 || code[code.Length - 1] != '\n')
            {
                builder.AppendLine();
            }
            builder.AppendLine("#nullable restore");
            return builder.ToString();
        }

        private static bool TryGetNullableFlags(CustomAttributeData attribute, out byte[] flags)
        {
            flags = null;
            if (attribute?.AttributeType.FullName != NullableAttributeName)
            {
                return false;
            }

            if (attribute.ConstructorArguments.Count != 1)
            {
                return false;
            }

            return TryReadFlags(attribute.ConstructorArguments[0].Value, out flags);
        }

        private static bool HasNullableReferenceAnnotation(MethodInfo method)
        {
            var context = new NullabilityInfoContext();

            if (method.ReturnType != typeof(void) &&
                HasNullableReference(method.ReturnType, context.Create(method.ReturnParameter), GetPositionFlag(method.ReturnParameter)))
            {
                return true;
            }

            foreach (var parameter in method.GetParameters().Skip(1))
            {
                var type = parameter.ParameterType;
                var info = context.Create(parameter);
                if (type.IsByRef)
                {
                    type = type.GetElementType();
                    info = info.ElementType ?? info;
                }

                if (HasNullableReference(type, info, GetPositionFlag(parameter)))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasNullableReference(Type type, NullabilityInfo info, byte? positionFlag)
        {
            if (type == null || info == null)
            {
                return false;
            }

            if (IsAnnotated(type, info, positionFlag))
            {
                return true;
            }

            if (type.IsArray && info.ElementType != null)
            {
                return HasNullableReference(type.GetElementType(), info.ElementType, null);
            }

            if (type.IsGenericType && info.GenericTypeArguments is { Length: > 0 })
            {
                var arguments = type.GetGenericArguments();
                var length = Math.Min(arguments.Length, info.GenericTypeArguments.Length);
                for (var i = 0; i < length; i++)
                {
                    if (HasNullableReference(arguments[i], info.GenericTypeArguments[i], null))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static string Format(Type type, NullabilityInfo info, byte? positionFlag)
        {
            if (type.IsGenericParameter)
            {
                return type.Name + NullableSuffix(type, info, positionFlag);
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                var element = info.ElementType != null
                    ? Format(elementType, info.ElementType, null)
                    : elementType.GetFullName();
                return element + GetArrayBrackets(type) + NullableSuffix(type, info, positionFlag);
            }

            if (type.GetTypeInfo().IsGenericType)
            {
                return FormatGenericType(type, info, positionFlag);
            }

            var name = (type.FullName ?? type.Name).Replace('+', '.');
            return name + NullableSuffix(type, info, positionFlag);
        }

        private static string FormatGenericType(Type type, NullabilityInfo info, byte? positionFlag)
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(type.Namespace))
            {
                builder.Append(type.Namespace);
                builder.Append('.');
            }

            var name = type.Name;
            var arity = name.IndexOf('`');
            builder.Append(arity >= 0 ? name[..arity] : name);
            builder.Append('<');

            var arguments = type.GenericTypeArguments;
            var argumentInfos = info.GenericTypeArguments;
            for (var i = 0; i < arguments.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(i < argumentInfos.Length
                    ? Format(arguments[i], argumentInfos[i], null)
                    : arguments[i].GetFullName());
            }

            builder.Append('>');
            builder.Append(NullableSuffix(type, info, positionFlag));
            return builder.ToString();
        }

        private static string GetArrayBrackets(Type type)
        {
            var rank = type.GetArrayRank();
            return rank == 1 ? "[]" : "[" + new string(',', rank - 1) + "]";
        }

        private static string NullableSuffix(Type type, NullabilityInfo info, byte? positionFlag)
        {
            return IsAnnotated(type, info, positionFlag) ? "?" : string.Empty;
        }

        private static bool IsAnnotated(Type type, NullabilityInfo info, byte? positionFlag)
        {
            if (type.IsValueType)
            {
                return false;
            }

            // A type parameter that may itself be a nullable reference type is reported as nullable
            // by NullabilityInfoContext wherever it's used, so `T` can't be told apart from `T?` that
            // way. The nullable flag of the position decides instead, which is only known for the
            // outermost type; nested usages such as `IList<T>` are treated as not annotated.
            if (type.IsGenericParameter && GetNullableFlag(type) == Annotated)
            {
                return positionFlag == Annotated;
            }

            return IsNullableState(info);
        }

        private static bool IsNullableState(NullabilityInfo info)
        {
            return info.ReadState == NullabilityState.Nullable ||
                   info.WriteState == NullabilityState.Nullable;
        }

        private static byte GetPositionFlag(ParameterInfo parameter)
        {
            foreach (var attribute in parameter.GetCustomAttributesData())
            {
                if (TryGetNullableFlags(attribute, out var flags) && flags.Length > 0)
                {
                    return flags[0];
                }
            }

            return parameter.Member != null
                ? GetScopeContextFlag(parameter.Member)
                : Oblivious;
        }

        private static byte GetNullableContextFlag(Type genericParameter)
        {
            var scope = (MemberInfo)genericParameter.DeclaringMethod ?? genericParameter.DeclaringType;
            return scope != null
                ? GetScopeContextFlag(scope)
                : Oblivious;
        }

        private static byte GetScopeContextFlag(MemberInfo member)
        {
            for (var scope = member; scope != null; scope = scope.DeclaringType)
            {
                if (TryGetContextFlag(scope.GetCustomAttributesData(), out var flag))
                {
                    return flag;
                }
            }

            return TryGetContextFlag(CustomAttributeData.GetCustomAttributes(member.Module), out var moduleFlag)
                ? moduleFlag
                : Oblivious;
        }

        private static bool TryGetContextFlag(IList<CustomAttributeData> attributes, out byte flag)
        {
            foreach (var attribute in attributes)
            {
                if (attribute.AttributeType.FullName == NullableContextAttributeName &&
                    TryGetContextFlag(attribute, out flag))
                {
                    return true;
                }
            }

            flag = Oblivious;
            return false;
        }

        private static bool TryGetContextFlag(CustomAttributeData attribute, out byte flag)
        {
            flag = Oblivious;
            if (attribute.ConstructorArguments.Count != 1)
            {
                return false;
            }

            var value = attribute.ConstructorArguments[0].Value;
            if (value is byte b)
            {
                flag = b;
                return true;
            }

            return false;
        }

        private static bool TryReadFlags(object value, out byte[] flags)
        {
            flags = null;
            if (TryReadFlag(value, out var flag))
            {
                flags = new[] { flag };
                return true;
            }

            if (value is IList<CustomAttributeTypedArgument> list)
            {
                flags = new byte[list.Count];
                for (var i = 0; i < list.Count; i++)
                {
                    if (!TryReadFlag(list[i].Value, out flags[i]))
                    {
                        flags = null;
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private static bool TryReadFlag(object value, out byte flag)
        {
            switch (value)
            {
                case byte b:
                    flag = b;
                    return true;
                case int i when i >= byte.MinValue && i <= byte.MaxValue:
                    flag = (byte)i;
                    return true;
                default:
                    flag = Oblivious;
                    return false;
            }
        }
    }
}
