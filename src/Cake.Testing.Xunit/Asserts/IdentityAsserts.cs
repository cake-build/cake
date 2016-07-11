// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit.Sdk;

namespace Xunit
{
    public partial class Assert
    {
        /// <summary>
        /// Verifies that two objects are not the same instance.
        /// </summary>
        /// <param name="expected">The expected object instance</param>
        /// <param name="actual">The actual object instance</param>
        /// <exception cref="NotSameException">Thrown when the objects are the same instance</exception>
        public static void NotSame(object expected, object actual)
        {
            if (object.ReferenceEquals(expected, actual))
                throw new NotSameException();
        }

        /// <summary>
        /// Verifies that two objects are the same instance.
        /// </summary>
        /// <param name="expected">The expected object instance</param>
        /// <param name="actual">The actual object instance</param>
        /// <exception cref="SameException">Thrown when the objects are not the same instance</exception>
        public static void Same(object expected, object actual)
        {
            if (!object.ReferenceEquals(expected, actual))
                throw new SameException(expected, actual);
        }
    }
}
