// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace Cake.Testing
{
    /// <summary>
    /// A <see cref="MemberInfo"/> paired with an instance against which to test.
    /// </summary>
    /// <typeparam name="TMemberInfo">The type of <see cref="MemberInfo"/> that will be tested.</typeparam>
    public struct MemberTestInfo<TMemberInfo> where TMemberInfo : MemberInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberTestInfo{TMemberInfo}"/> struct.
        /// </summary>
        /// <param name="instance">The instance against which to test the <paramref name="member"/>.</param>
        /// <param name="member">The member to test.</param>
        public MemberTestInfo(object instance, TMemberInfo member)
        {
            Instance = instance;
            Member = member;
        }

        /// <summary>
        /// Gets the instance against which to test the <see cref="Member"/>.
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// Gets the member to test.
        /// </summary>
        public TMemberInfo Member { get; }

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            var instanceToString = Instance.ToString();
            var instanceDisplay = instanceToString == Instance.GetType().ToString() ? Instance.GetType().Name : instanceToString;
            return $"{instanceDisplay}.{Member.Name}";
        }
    }
}
