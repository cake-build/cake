// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

            /*
             Possible permutations:
             T : class
             T : class, new(),
             T : SomeClass
             T : ISomeInterface
             T : SomeClass, ISomeInterface
             T : SomeClass, ISomeInterface, new()
             T : struct
             T : struct, ISomeInterface

             Cannot specify both ClassType and [struct|class]
             Cannot specify `struct, new()`
             */

            foreach (var argument in method.GetGenericMethodDefinition().GetGenericArguments())
            {
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
                    if (paramAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                    {
                        tokens.Add("class");
                    }

                    // iterate type constraints
                    foreach (var constraint in argument.GetTypeInfo().GetGenericParameterConstraints())
                    {
                        tokens.Add(constraint.GetFullName());
                    }

                    // default constructor has to come last, can't be used in conjunction w/ struct
                    if (paramAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                    {
                        tokens.Add("new()");
                    }
                }

                if (tokens.Count > 0)
                {
                    builder.AppendLine();
                    builder.AppendFormat("where {0} : {1}", argument.Name, string.Join(", ", tokens));
                }
            }
        }
    }
}