// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when the value is unexpectedly not of the given type or a derived type.
    /// </summary>
    public class IsAssignableFromException : AssertActualExpectedException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="IsTypeException"/> class.
        /// </summary>
        /// <param name="expected">The expected type</param>
        /// <param name="actual">The actual object value</param>
        public IsAssignableFromException(Type expected, object actual)
            : base(expected, actual == null ? null : actual.GetType(), "Assert.IsAssignableFrom() Failure")
        { }
    }
}
