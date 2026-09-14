// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cake.Core.Scripting.CodeGen
{
    /// <summary>
    /// Responsible for generating generic parameter constraints on generated generic methods.
    /// </summary>
    internal sealed class GenericParameterConstraintEmitter
    {
        private const string ClassConstraint = "class";
        private const string NullableClassConstraint = "class?";
        private const string NotNullConstraint = "notnull";

        internal static string Emit(MethodInfo method)
        {
            var builder = new StringBuilder();
            BuildGenericConstraints(method, builder);
            return builder.ToString().Trim();
        }

        internal static void BuildGenericConstraints(MethodInfo method, StringBuilder builder)
        {
            if (!method.IsGenericMethod)
            {
                return;
            }

            foreach (var argument in method.GetGenericMethodDefinition().GetGenericArguments())
            {
                var tokens = BuildConstraintTokens(argument);

                if (tokens.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendFormat("where {0} : {1}", argument.Name, string.Join(", ", tokens));
                }
            }
        }

        /// <summary>
        /// Returns <c>true</c> when any emitted constraint of <paramref name="method"/>
        /// only compiles inside a nullable annotations context.
        /// </summary>
        /// <param name="method">The alias method.</param>
        /// <returns><c>true</c> if <c>notnull</c> or <c>class?</c> will be emitted.</returns>
        internal static bool HasNullableAwareConstraint(MethodInfo method)
        {
            if (!method.IsGenericMethod)
            {
                return false;
            }

            foreach (var argument in method.GetGenericMethodDefinition().GetGenericArguments())
            {
                foreach (var token in BuildConstraintTokens(argument))
                {
                    if (token is NullableClassConstraint or NotNullConstraint)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static List<string> BuildConstraintTokens(Type argument)
        {
            /*
             Possible permutations:
             T : class
             T : class?
             T : class, new(),
             T : notnull
             T : SomeClass
             T : ISomeInterface
             T : SomeClass, ISomeInterface
             T : SomeClass, ISomeInterface, new()
             T : struct
             T : struct, ISomeInterface

             Cannot specify both ClassType and [struct|class]
             Cannot specify `struct, new()`
             */

            var paramAttributes = argument.GetTypeInfo().GenericParameterAttributes;
            var tokens = new List<string>();

            if (paramAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
            {
                tokens.Add("struct");

                // iterate type constraints; it's possible that you can have ` where T : struct, ISomeInterface`
                foreach (var constraint in argument.GetTypeInfo().GetGenericParameterConstraints())
                {
                    // however, the struct constraint will return System.ValueType.
                    // it's not necessarily to emit that as syntax in a generated method
                    if (constraint == typeof(System.ValueType))
                    {
                        continue;
                    }

                    tokens.Add(constraint.GetFullName());
                }
            }
            else
            {
                // if it's declared a struct, we can't use any other constraints (inherits/implements or default ctor)

                // special considerations? reference/value
                var hasTypeConstraint = paramAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint);
                if (hasTypeConstraint)
                {
                    // `class?` and `class` only differ by the nullable metadata of the type parameter
                    tokens.Add(NullableAnnotationContext.GetNullableFlag(argument) == NullableAnnotationContext.Annotated
                        ? NullableClassConstraint
                        : ClassConstraint);
                }

                // iterate type constraints
                foreach (var constraint in argument.GetTypeInfo().GetGenericParameterConstraints())
                {
                    hasTypeConstraint = true;
                    tokens.Add(constraint.GetFullName());
                }

                // default constructor has to come last, can't be used in conjunction w/ struct
                if (paramAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                {
                    tokens.Add("new()");
                }

                // `notnull` leaves no trace other than the nullable metadata of the type parameter,
                // and is only needed when no type constraint already implies it. It's a primary
                // constraint, so it has to come before `new()`.
                if (!hasTypeConstraint &&
                    NullableAnnotationContext.GetNullableFlag(argument) == NullableAnnotationContext.NotAnnotated)
                {
                    tokens.Insert(0, NotNullConstraint);
                }
            }

            return tokens;
        }
    }
}