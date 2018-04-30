// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cake.Testing
{
    /// <summary>
    /// Contains utilities for testing members of types.
    /// </summary>
    public static class MemberTestingUtils
    {
        /// <summary>
        /// Searches all loaded assemblies for public types assignable to the specified base class type
        /// and returns all pairs of desired members and instances with which to test them.
        /// </summary>
        /// <typeparam name="TMemberInfo">The type of <see cref="MemberInfo"/> that will be tested.</typeparam>
        /// <param name="baseType">The base type to filter by.</param>
        /// <param name="declaredMemberSelector">For each unique type, chooses the declared members of interest.</param>
        /// <param name="manuallyCreatedInstances">
        /// Instances to test against, for types that are abstract or missing a default constructor, or when
        /// there is a need to test against one or more custom-constructed instances for a certain type.
        /// </param>
        /// <returns>All pairs of desired members and instances with which to test them.</returns>
        /// <exception cref="ArgumentNullException">A required argument was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="baseType"/> is not a class.</exception>
        /// <exception cref="InvalidOperationException">
        /// Either <paramref name="declaredMemberSelector"/> returned a member which was not declared in the specified type,
        /// or a type was not able to be instantiated and <paramref name="manuallyCreatedInstances"/> does not contain
        /// an instance of the type.
        /// </exception>
        public static IEnumerable<MemberTestInfo<TMemberInfo>> GetMembersToTest<TMemberInfo>(
            Type baseType,
            Func<Type, IEnumerable<TMemberInfo>> declaredMemberSelector,
            params object[] manuallyCreatedInstances)
            where TMemberInfo : MemberInfo
        {
            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            if (!baseType.IsClass)
            {
                throw new ArgumentException("BaseType", nameof(baseType));
            }

            if (declaredMemberSelector == null)
            {
                throw new ArgumentNullException(nameof(declaredMemberSelector));
            }

            var typesAndMembers = (
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                where !assembly.IsDynamic
                from type in assembly.GetExportedTypes()
                where type == baseType || type.IsSubclassOf(baseType)
                select new { type, members = GetDeclaredMembers(type) }).ToList();

            var instancesByExactType = new Dictionary<Type, List<object>>();
            AddManuallyCreatedInstances();
            CreateMissingInstances();

            return CreateAllPairsOfInstancesAndMembers();

            IReadOnlyList<TMemberInfo> GetDeclaredMembers(Type type)
            {
                var members = declaredMemberSelector.Invoke(type);
                if (members == null)
                {
                    return Array.Empty<TMemberInfo>();
                }

                var declaredMembers = new List<TMemberInfo>();

                foreach (var member in members)
                {
                    if (member.DeclaringType != type)
                    {
                        throw new InvalidOperationException($"The declared member selector returned a member for {type} which is declared by {member.DeclaringType}.");
                    }

                    declaredMembers.Add(member);
                }

                return declaredMembers;
            }

            void AddManuallyCreatedInstances()
            {
                foreach (var instance in manuallyCreatedInstances)
                {
                    if (instance == null)
                    {
                        continue;
                    }

                    var type = instance.GetType();
                    if (!instancesByExactType.TryGetValue(type, out var list))
                    {
                        instancesByExactType.Add(type, list = new List<object>(1));
                    }

                    list.Add(instance);
                }
            }

            void CreateMissingInstances()
            {
                foreach (var typeAndMembers in typesAndMembers)
                {
                    if (typeAndMembers.type.IsAbstract || instancesByExactType.ContainsKey(typeAndMembers.type))
                    {
                        continue;
                    }

                    if (typeAndMembers.members.Count == 0 && !HasAbstractBase(typeAndMembers.type))
                    {
                        // No need for an instance of this type since it has none of its own members which
                        // we are interested in and it can’t be needed to test members of an abstract base type.
                        continue;
                    }

                    var defaultConstructor = typeAndMembers.type.GetConstructor(Type.EmptyTypes);
                    if (defaultConstructor == null)
                    {
                        throw new InvalidOperationException($"{typeAndMembers.type} does not have a public default constructor and no instance with the exact type was manually specified.");
                    }

                    instancesByExactType.Add(
                        typeAndMembers.type,
                        new List<object>(1) { defaultConstructor.Invoke(null) });
                }
            }

            bool HasAbstractBase(Type type)
            {
                do
                {
                    type = type.BaseType;
                    if (type.IsAbstract)
                    {
                        return true;
                    }
                }
                while (type != baseType);

                return false;
            }

            IEnumerable<MemberTestInfo<TMemberInfo>> CreateAllPairsOfInstancesAndMembers()
            {
                var membersToTest = new List<MemberTestInfo<TMemberInfo>>();

                foreach (var typeAndMembers in typesAndMembers)
                {
                    if (typeAndMembers.members.Count == 0)
                    {
                        continue;
                    }

                    foreach (var member in typeAndMembers.members)
                    {
                        var foundInstance = false;

                        foreach (var other in instancesByExactType)
                        {
                            if (other.Key == typeAndMembers.type || other.Key.IsSubclassOf(typeAndMembers.type))
                            {
                                foreach (var instance in other.Value)
                                {
                                    membersToTest.Add(new MemberTestInfo<TMemberInfo>(instance, member));
                                    foundInstance = true;
                                }
                            }
                        }

                        if (!foundInstance)
                        {
                            throw new InvalidOperationException($"{typeAndMembers.type} is abstract and no instance of an inheriting type was manually specified.");
                        }
                    }
                }

                return membersToTest;
            }
        }
    }
}
